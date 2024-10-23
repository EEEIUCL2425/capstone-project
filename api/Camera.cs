using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public void rotate_camera(float pitch, float yaw, float roll)
    {
        Quaternion new_gripper_rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + pitch, transform.rotation.eulerAngles.y + yaw, transform.rotation.eulerAngles.z + roll);
        transform.rotation = new_gripper_rotation;
        Debug.Log("Rotated camera by " + pitch + ", " + yaw + ", " + roll);
        Debug.Log("Current gripper rotation: " + transform.rotation.eulerAngles);
    }

    public void move_camera(float x, float y, float z)
    {
        transform.position += new Vector3(x, y, z);
        Debug.Log("Moved camera by " + x + ", " + y + ", " + z);
        Debug.Log("Current camera position: " + transform.position);
    }

    /*public void GetCameraRotation()
    {
        Vector3 cameraRotation = Camera.main.transform.rotation.eulerAngles;
        string rotationJson = JsonUtility.ToJson(new Vector3Data { x = cameraRotation.x, y = cameraRotation.y, z = cameraRotation.z });
        Send(rotationJson);
        Debug.Log("Sent camera rotation: " + rotationJson);
    }*/

    private void Send(string message)
    {
        // Implement the send logic here
        // This method should send the message back to the client
    }

    [System.Serializable]
    public class Vector3Data
    {
        public float x, y, z;
    }
}
