using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public Transform cameraPivot;
    public Transform cameraTransform;
    public CharacterController controller;
    public float moveSpeed = 5f;
    public float mouseSensitivity = 150; 
    float yaw;
    float pitch;
    public Transform carryPoint;
    interactable carriedObject;
    bool bulldozing = false;
    bool menuOpen = false;

   
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        /*
        // movement side pass
        // getting input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        // where the camera is pointing
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        // remove up or down direction
        forward.y = 0;
        right.y = 0;
        //making the numbers between 1 and -1 i guess
        forward.Normalize();
        right.Normalize();
        // create move direction
        Vector3 moveDirection = forward * vertical + right * horizontal;
        // rotate player to face move direction
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            //rotate player to face move direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            // smooth rotation, no snapping
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 8f * Time.deltaTime);
        }
        //actually move the player
        controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        */

        // movement turning with A D
        float move = Input.GetAxisRaw("Vertical");
        float turn = Input.GetAxisRaw("Horizontal");

        transform.Rotate(Vector3.up, turn * 120f * Time.deltaTime);
        controller.Move(transform.forward * move * moveSpeed * Time.deltaTime);
        
        // free mouse look
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -30f, 70f);
        cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);


        cameraPivot.position = transform.position;// needed for both movement versions





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