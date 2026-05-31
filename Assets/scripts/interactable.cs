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
        //isCarried = false;
        if (rb != null)
        {
            //enable physics when dropped
            rb.isKinematic = false; 
            rb.useGravity = true;
            rb.AddForce(transform.forward * weight, ForceMode.Impulse); // add force when dropped
        }
        
    }
}
