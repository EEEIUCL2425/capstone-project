using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private bool isCollidingWithShelf = false;
    private int counter = 0;
    void Update()
    {
        float moveSpeed = 5f;
        float rotateSpeed = 100f;

        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float rotateY = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotateY = -rotateSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotateY = rotateSpeed * Time.deltaTime;
        }

        move_camera(moveX, 0, moveZ);
        rotate_camera(0, rotateY, 0);
        float moveY = 0f;
        if (Input.GetKey(KeyCode.R))
        {
            moveY = moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            moveY = -moveSpeed * Time.deltaTime;
        }

        move_camera(moveX, moveY, moveZ);


    }

    private void OnTriggerEnter(Collider other)
    {
        Shelf shelf = other.GetComponent<Shelf>();
        if (shelf != null)
        {
            isCollidingWithShelf = true;
            Debug.Log("Camera entered shelf trigger.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Shelf shelf = other.GetComponent<Shelf>();
        if (shelf != null)
        {
            isCollidingWithShelf = false;
            Debug.Log("Camera exited shelf trigger.");
        }
    }
    // void OnMouseDown()
    // {
    //     ScreenCapture.CaptureScreenshot("SomeLevel"+counter+".png");
    //     counter++;
    // }

    public void rotate_camera(float pitch, float yaw, float roll)
    {
        transform.Rotate(pitch, yaw, roll, Space.Self);
        //Debug.Log("Rotated camera by " + pitch + ", " + yaw + ", " + roll);
        //Debug.Log("Current camera rotation: " + transform.rotation.eulerAngles);
    }

    public void move_camera(float x, float y, float z)
    {
        transform.Translate(new Vector3(x, y, z), Space.Self);
        //Debug.Log("Moved camera by " + x + ", " + y + ", " + z);
        //Debug.Log("Current camera position: " + transform.position);
        /*
        if (!isCollidingWithShelf)
        {
            transform.Translate(new Vector3(x, y, z), Space.Self);
            Debug.Log("Moved camera by " + x + ", " + y + ", " + z);
            Debug.Log("Current camera position: " + transform.position);
        }
        else
        {
            Debug.Log("Camera movement restricted due to collision with shelf.");
        }
        */
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