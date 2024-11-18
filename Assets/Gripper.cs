using UnityEngine;
using System.Collections.Generic;

public class Gripper : MonoBehaviour
{
    public string gripperId;
    private GameObject heldObject;
    private Vector3 heldOffset;
    private List<GameObject> collidingObjects = new List<GameObject>();
    private Animator gripperAnimator;

    void Start()
    {
        gripperAnimator = GetComponent<Animator>();
        if (gripperAnimator == null)
        {
            Debug.LogError("No Animator component found on the gripper.");
        }
    }

    void Update(){
        if (Input.GetKey(KeyCode.I))
        {
            move_gripper(0, 0, 0.05f);
        }
        if (Input.GetKey(KeyCode.K))
        {
            move_gripper(0, 0, -0.05f);
        }
        if (Input.GetKey(KeyCode.J))
        {
            move_gripper(-0.05f, 0, 0);
        }
        if (Input.GetKey(KeyCode.L))
        {
            move_gripper(0.05f, 0, 0);
        }
        if (Input.GetKey(KeyCode.U))
        {
            rotate_gripper(0, -3f, 0);
        }
        if (Input.GetKey(KeyCode.O))
        {
            rotate_gripper(0, 3f, 0);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (heldObject == null)
            {
            grip();
            }
            else
            {
            release();
            }
        }
        if (Input.GetKey(KeyCode.Y))
        {
            move_gripper(0, 0.05f, 0);
        }
        if (Input.GetKey(KeyCode.H))
        {
            move_gripper(0, -0.05f, 0);
        }
        if (Input.GetKey(KeyCode.B))
        {
            rotate_gripper(-3f, 0, 0);
        }
        if (Input.GetKey(KeyCode.N))
        {
            rotate_gripper(3f, 0, 0);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.CompareTag("Grippable"))
        {
            collidingObjects.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            collidingObjects.Remove(other.gameObject);
        }
    }

    public void move_gripper(float x, float y, float z)
    {
        Vector3 new_position = transform.position + new Vector3(x, y, z);
        Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        float distance_to_camera = Vector3.Distance(new_position, mainCamera.transform.position);
        
        if (distance_to_camera <= 0.8)
        {
            transform.Translate(new Vector3(x, y, z), Space.Self);
            //Debug.Log("Moved gripper by " + x + ", " + y + ", " + z);
            //Debug.Log("Current gripper position: " + new_position);
        }
        else
        {
            //Debug.Log("Movement limited to 76.74 cm from the camera.");
            //Debug.Log("Current gripper position: " + transform.position);

        }
    }

    public void rotate_gripper(float pitch, float yaw, float roll)
    {
        //Quaternion new_gripper_rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + pitch, transform.rotation.eulerAngles.y + yaw, transform.rotation.eulerAngles.z + roll);
        transform.Rotate(pitch, yaw, roll, Space.Self);
        //Debug.Log("Rotated gripper by " + pitch + ", " + yaw + ", " + roll);
        //Debug.Log("Current gripper rotation: " + transform.rotation.eulerAngles);
    }

    public void ResetGripper(string GripperId)
    {
        if (gripperId == GripperId)
        {
            Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * 0.8f;
            transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
            Debug.Log("Gripper reset to initial position and rotation.");
        }
        else
        {
            Debug.LogWarning("Gripper ID does not match. Reset aborted.");
        }
    }
    public void grip()
    {
        if (collidingObjects.Count == 0)
        {
            Debug.Log("No objects to grip.");
            return;
        }

        // Find the nearest object
        GameObject nearestObject = null;
        float minDistance = float.MaxValue;
        Vector3 gripperPosition = transform.position;

        foreach (GameObject obj in collidingObjects)
        {
            float distance = Vector3.Distance(gripperPosition, obj.transform.position);
            if (distance < minDistance)
            {
                nearestObject = obj;
                minDistance = distance;
            }
        }

        if (nearestObject == null)
        {
            Debug.Log("No valid objects to grip.");
            return;
        }

        // Calculate the offset between the gripper and the contact point
        Vector3 contactPoint = nearestObject.transform.position;
        heldOffset = transform.InverseTransformPoint(contactPoint);

        // Grip the nearest object using a fixed joint
        Rigidbody rigidbody = nearestObject.GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            Debug.LogError($"The object {nearestObject.name} does not have a Rigidbody component.");
            return;
        }
        heldObject = nearestObject;
        heldObject.transform.SetParent(transform);
        rigidbody.isKinematic = true;
        Debug.Log($"Gripped object: {nearestObject.name}");

        if (gripperAnimator != null)
        {
            gripperAnimator.SetBool("isGrip", true);
        }
    }
        public void release()
    {
        if (heldObject != null)
        {
            Rigidbody rigidbody = heldObject.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = false;
            }
            heldObject.transform.SetParent(null);
            heldObject = null;
            Debug.Log("Released object.");
            if (gripperAnimator != null)
            {
                gripperAnimator.SetBool("isGrip", false);
            }
        }
    }
}