using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactable : MonoBehaviour
{
    public bool canCarry = true;
    bool originalGravity;
    bool originalKinematic;
    Rigidbody rb;
    Collider objectCollider;
    public float weight = 1f;
    


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        if (rb != null)
        {
            originalGravity = rb.useGravity;
            originalKinematic = rb.isKinematic;
        }
    }


    

    public void Pickup(Collider carrierCollider)
    {
        //isCarried = true;
        if (rb != null && canCarry)
        {
            {
                //disable physics while carried
                rb.isKinematic = true; 
                rb.useGravity = false;
                Physics.IgnoreCollision(objectCollider, carrierCollider, true);
            }
        }
    }

    public void Drop(Collider carrierCollider)
    {
        int groundMask = LayerMask.GetMask("ground");
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 5f, Vector3.down, out hit, 100f, groundMask))
        {
            // hit the ground
            if(GetComponent<plants>() != null)
            {
                transform.position = hit.point;
                rb.isKinematic = true;
                rb.useGravity = true;
            }
            else
            {
                float offset = objectCollider.bounds.extents.y;
                transform.position = hit.point + Vector3.up * offset;
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
        else
        {
            Debug.Log("NO HIT");// just throw it
            rb.isKinematic = false; 
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.forward * weight, ForceMode.Impulse); // add force when dropped
        };

        Physics.IgnoreCollision(objectCollider, carrierCollider, false);
        
    }
}
