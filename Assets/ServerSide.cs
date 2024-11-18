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
        
        // wss = new WebSocketServer("ws://localhost:8080");
        // wss.AddWebSocketService<CommandBehavior>("/commands");
        // wss.Start();
        // Debug.Log("WebSocket server started on ws://localhost:8080/commands...");
        // UnityMainThreadDispatcher.Instance();
        
        
    }

    void OnDestroy()
    {
        // if (wss != null) wss.Stop();
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
        StoreManager shelves = GameObject.FindObjectOfType<StoreManager>();

        switch (command.command)
        {

            //Camera commands

            case "RotateCamera":
                camera.rotate_camera(command.rotation.x, command.rotation.y, command.rotation.z);
                break;

            case "MoveCamera":
                camera.move_camera(command.position.x, command.position.y, command.position.z);
                break;

            //TODO: Add other commands here

            //Gripper commands

            case "MoveGripper":
                gripper.move_gripper(command.position.x, command.position.y, command.position.z);
                break;

            case "RotateGrigpper":
                gripper.rotate_gripper(command.rotation.x, command.rotation.y, command.rotation.z);
                break;

            case "ResetGripper":
                gripper.ResetGripper(command.gripperId);
                break;
            case "Grip":
                gripper.grip();
                break; 

            case "Release":
                gripper.release();
                break;

            case "SpawnShelf":
                shelves.SpawnShelf(command.shelf_id, command.x, command.z, command.length, command.width, command.height, command.thickness, command.level, command.angle);
                break;

            case "BuildPresetStore":
                shelves.BuildPresetStore(command.preset, command.level);
                break;
            case "BuildRandomStore":
                shelves.BuildRandomStore(command.seed, command.StoreLength, command.StoreWidth, command.length, command.width, command.height, command.thickness, command.ShelfDistance, command.level, command.division, command.variation);
                break;
            case "FillShelves":
                shelves.FillShelves(command.seed, command.orientation, command.GroceryProducts);
                break;
            case "SpawnRoom":
                shelves.SpawnRoom(command.length, command.width);
                break;
            case "DestroyAllProducts":
                shelves.DestroyAllProducts();
                break;
            case "DestroyAllShelves":
                shelves.DestroyAllShelves();
                break;
            case "DestroyRoom":
                shelves.DestroyRoom();
                break;
            default:
                Debug.Log("Unknown command: " + command.command);
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
        public string shelf_id;
        public float x;
        public float z;
        public float length;
        public float width;
        public float height;
        public float thickness;
        public int division;
        public int level;
        public float angle;
        public string preset;
        public int seed;
        public float StoreLength;
        public float StoreWidth;
        public float ShelfDistance; 
        public List<string> GroceryProducts;
        public float variation;
        public float orientation;
    }

    [System.Serializable]
    public class Vector3Data
    {
        public float x, y, z;
    }
}