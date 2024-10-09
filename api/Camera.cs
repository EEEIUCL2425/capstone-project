using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    
    public Transform orientation;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        PanCamera(-1, 0); // Pans the camera to the left
    }

    void PanCamera(float xDirection, float yDirection) // This is the important part
    {
        /* Pans the camera given xAxis and yAxis
            xAxis:
                -1 = Left
                1 = Right
            yAxis:
                -1 = Down
                1 = Up 
        */
        float xAngle = xDirection;
        float yAngle = yDirection;

        xRotation += xAngle;
        yRotation -= yAngle;

        yRotation = Mathf.Clamp(yRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(yRotation, xRotation, 0);
        orientation.rotation = Quaternion.Euler(0, xRotation, 0);        

    }
}
