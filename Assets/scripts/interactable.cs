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


    void Update()
    {
        //Debug.DrawRay(transform.position + Vector3.up * 5f, Vector3.down * 20f, Color.green, 2f);
        Debug.DrawRay(transform.position + UnityEngine.Vector3.up, UnityEngine.Vector3.down * 200f, Color.red, 200f);
    }
    

    public void pickup(Collider carrierCollider)
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

    public void drop(Collider carrierCollider)
    {
        //Debug.Log("Plant position: " + transform.position);
        //Debug.Log("Terrain height: " + Terrain.activeTerrain.SampleHeight(transform.position));
        //Debug.Log("Ground layer: " + LayerMask.GetMask("Ground"));
        //Debug.DrawRay(transform.position + UnityEngine.Vector3.up, UnityEngine.Vector3.down, Color.red, 200f);
        int groundMask = LayerMask.GetMask("ground");
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 5f, Vector3.down, out hit, 100f, groundMask))
        {
            Debug.Log("HIT: " + hit.collider.name);
            transform.position = hit.point;
            if(GetComponent<plants>() == null)
            {
                rb.useGravity = true;
            }
            
        }
        else
        {
            Debug.Log("NO HIT");
            // just throw it
            rb.isKinematic = false; 
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.forward * weight, ForceMode.Impulse); // add force when dropped
        };
        Physics.IgnoreCollision(objectCollider, carrierCollider, false);






        
        
    }
}
