using System.Collections;
using System.Collections.Generic;
//using System.Numerics;

//using System.Threading.Tasks.Dataflow;

//using System.Numerics;
//using System.Threading.Tasks.Dataflow;
using UnityEngine;

public class player : MonoBehaviour
{
    public Transform playerTransform;
    public Transform cameraPivot;
    public Transform cameraTransform;
    public CharacterController controller;
    public float moveSpeed = 5f;
    public float turnSpeed = 100f;
    public float mouseSensitivity = 150; 
    public float keyboardTurnSpeed = 100;
    float yaw;
    float pitch;
    public Transform carryPoint;
    interactable carriedObject;
    bool bulldozing = false;
    bool menuOpen = false;
    float gravity = -20f;
    float verticalVelocity = 0f;
    public int movementStyle = 1;
    //float yaw;// camera left right
    //float pitch;// camera up down
    

   
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerTransform.localScale = new UnityEngine.Vector3(.5f, .5f, .5f);
        //playerTransform.localPosition = new UnityEngine.Vector3(0f, 0f, 0f);
        
    }

    
    void Update()
    {
        float turn = Input.GetAxisRaw("Horizontal");
        float move = Input.GetAxisRaw("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        Vector3 direction;
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            movementStyle++;
            if (movementStyle > 5)
            {
                movementStyle = 1;
            }
        }


        // gravity
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
            {
                verticalVelocity = -2f;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }



        switch (movementStyle)
        {
            case 1:// combo 1: movement where camera looks, rotation face camera, free orbit
                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();
                direction = forward * move + right * turn;
                direction.y = verticalVelocity;
                controller.Move(direction * moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0f, yaw, 0f);
                yaw += mouseX * mouseSensitivity * Time.deltaTime;
                pitch -= mouseY * mouseSensitivity * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, -30f, 70f);
                cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
                break;

            case 2:// combo 4: movement where camera looks, rotate A D, camera fixed
                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();
                direction = forward * move + right * turn;
                direction.y = verticalVelocity;
                controller.Move(direction * moveSpeed * Time.deltaTime);
                transform.Rotate(Vector3.up, turn * turnSpeed * Time.deltaTime);
                pitch -= mouseY * mouseSensitivity * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, -30f, 70f);
                cameraPivot.rotation = Quaternion.Euler(pitch, transform.eulerAngles.y, 0f);
                break;

            case 3:// combo 5: movement where camera looks, rotate face movement, free orbit
                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();
                direction = forward * move + right * turn;
                direction.y = verticalVelocity;
                controller.Move(direction * moveSpeed * Time.deltaTime);
                if (direction.sqrMagnitude > 0.01f)
                {
                    Quaternion target = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, target, 8f * Time.deltaTime);
                }
                yaw += mouseX * mouseSensitivity * Time.deltaTime;
                pitch -= mouseY * mouseSensitivity * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, -30f, 70f);
                cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
                break;

            case 4:// combo 9: movement where body faces, rotate A D, free orbit
                direction = transform.forward * move;
                direction.y = verticalVelocity;
                controller.Move(direction * moveSpeed * Time.deltaTime);
                transform.Rotate(Vector3.up, turn * turnSpeed * Time.deltaTime);
                yaw += mouseX * mouseSensitivity * Time.deltaTime;
                pitch -= mouseY * mouseSensitivity * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, -30f, 70f);
                cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
                break;

            case 5:// combo 10: movement where body faces, rotate A D, camera fixed
                direction = transform.forward * move;
                direction.y = verticalVelocity;
                controller.Move(direction * moveSpeed * Time.deltaTime);
                transform.Rotate(Vector3.up, turn * turnSpeed * Time.deltaTime);
                pitch -= mouseY * mouseSensitivity * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, -30f, 70f);
                cameraPivot.rotation = Quaternion.Euler(pitch, transform.eulerAngles.y, 0f);
                break;
        }











       

        // keep the camera with the player
        cameraPivot.position = transform.position + Vector3.up * 1.5f;






        // pick up, drop, bulldoze objects
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (carriedObject == null){PickupObject();}
            else{DropObject();}
        }
        bulldozing = Input.GetKey(KeyCode.LeftShift);
    }


    void PickupObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 3f))
        {
            interactable interactableObject = hit.collider.GetComponent<interactable>();
            if (interactableObject != null && interactableObject.canCarry)
            {
                carriedObject = interactableObject;
                carriedObject.Pickup(GetComponent<Collider>());
                carriedObject.transform.SetParent(carryPoint);
                carriedObject.transform.localPosition = Vector3.zero;
            }
        }
    }    


    void DropObject()
    {
        if (carriedObject != null)
        {
            carriedObject.transform.SetParent(null);
            carriedObject.Drop(GetComponent<Collider>());
            carriedObject = null;
        }
    }



    void OnCollisionEnter(Collision collision)
    {
        if (bulldozing)
        {
            interactable interactableItem = collision.gameObject.GetComponent<interactable>();
            if (interactableItem != null){interactableItem.bulldoze();}
        }
    }
}