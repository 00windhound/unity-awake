using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactable : MonoBehaviour
{
    public bool canCarry = true;
    //bool isCarried = false;// do i need this?
    bool originalGravity;
    bool originalKinematic;
    Rigidbody rb;
    public float weight = 1f;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            originalGravity = rb.useGravity;
            originalKinematic = rb.isKinematic;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(
            transform.position + Vector3.up * 5f,
            Vector3.down * 20f,
            Color.green,
        2f
        );
    }
    

    public void pickup()
    {
        //isCarried = true;
        if (rb != null && canCarry)
        {
            {
                //disable physics while carried
                rb.isKinematic = true; 
                rb.useGravity = false;
            }
         
        }
    }

    public void drop()
    {
        if(GetComponent<plants>() != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + UnityEngine.Vector3.up, UnityEngine.Vector3.down, out hit, 200f, LayerMask.GetMask("Ground")))
            {
                transform.position = hit.point + Vector3.up * 0.5f; // snap to ground
            }
            {
                transform.position = hit.point;// + Vector3.up * 0.5f; 
            }
            rb.isKinematic = true; 
            rb.useGravity = false;
        }
        else if (rb != null)
        {
            //enable physics when dropped
            rb.isKinematic = false; 
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.forward * weight, ForceMode.Impulse); // add force when dropped
        }
        
        
    }
}
