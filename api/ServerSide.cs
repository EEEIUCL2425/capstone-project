using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using WebSocketSharp.Server;
using WebSocketSharp;

public class WebSocketHandler : MonoBehaviour
{
    private WebSocketServer wss;

    void Start()
    {
        // Start the WebSocket server on port 8080
        wss = new WebSocketServer("ws://localhost:8080");
        wss.AddWebSocketService<CommandBehavior>("/commands");
        wss.Start();
        Debug.Log("WebSocket server started on ws://localhost:8080/commands...");
        UnityMainThreadDispatcher.Instance();
    }

    void OnDestroy()
    {
        if (wss != null) wss.Stop();
    }
}

public class CommandBehavior : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        Debug.Log("Message received: " + e.Data);
        
        // Run the command on Unity's main thread using the dispatcher
        UnityMainThreadDispatcher.Instance().Enqueue(() => HandleCommand(e.Data));
    }

    private void HandleCommand(string json)
    {
        // Parse the incoming JSON and apply transformations
        CommandData command = JsonUtility.FromJson<CommandData>(json);
        
        Gripper gripper = FindGripperById(command.gripperId);
        Camera camera = GameObject.FindObjectOfType<Camera>();
        if (gripper == null)
        {
            Debug.LogError("Gripper not found.");
            return;
        }

        switch (command.command)
        {

            //Camera commands

            case "rotate_camera":
                camera.rotate_camera(command.rotation.x, command.rotation.y, command.rotation.z);
                break;

            case "move_camera":
                camera.move_camera(command.position.x, command.position.y, command.position.z);
                break;

            //TODO: Add other commands here

            //Gripper commands

            case "move_gripper":
                gripper.move_gripper(command.position.x, command.position.y, command.position.z);
                break;

            case "rotate_gripper":
                gripper.rotate_gripper(command.rotation.x, command.rotation.y, command.rotation.z);
                break;

            case "grip":
                gripper.grip();
                break; 

            case "release":
                gripper.release();
                break;
        }
    }

    private Gripper FindGripperById(string gripperId)
    {
        Gripper[] grippers = GameObject.FindObjectsOfType<Gripper>();
        foreach (Gripper gripper in grippers)
        {
            if (gripper.gripperId == gripperId)
            {
                return gripper;
            }
        }
        return null;
    }

    [System.Serializable]
    public class CommandData
    {
        public string command;
        public string gripperId;
        public Vector3Data position;
        public Vector3Data rotation;
    }

    [System.Serializable]
    public class Vector3Data
    {
        public float x, y, z;
    }
}

