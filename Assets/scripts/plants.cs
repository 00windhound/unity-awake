using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;


public class plants : MonoBehaviour
{
    public LayerMask groundLayer; 
    public LayerMask plantLayer;
    public Transform model;
    int count = 0;

    void Start()
    {
        // change scale to baby size
        resize(0.2f);
        if(crowded())
        {
            Destroy(gameObject); // destroy plant if too crowded
        }
        count = 0;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + UnityEngine.Vector3.up, UnityEngine.Vector3.down, out hit, 10f, groundLayer))
        {// snap plant to ground
            transform.position = hit.point;
        }
       
    }

    void Update()
    {
        count++;
        
        
        if (count % 10 == 0)
        {
            bool seeground = false;
            bool isupright = true;
            RaycastHit hit;// if on the ground
            if (Physics.Raycast(transform.position + UnityEngine.Vector3.up, UnityEngine.Vector3.down, out hit, 10f, groundLayer))
            {
                seeground = true;
            }

            float upright = UnityEngine.Vector3.Dot(transform.up, UnityEngine.Vector3.up);
            if (upright < 0.5f)
            {
                isupright = false;
            }


            if (!seeground || !isupright)
            {
                resize(0.99f); // Shrink plant by 1%
                if (model.localScale.x < 0.2f)
                {
                    Destroy(gameObject); // destroy small plant
                } 
            }
            
            else if (model.localScale.x < 1.0f)
            {
                if (!crowded())
                {
                    resize(1.01f); // Grow plant by 1%
                }
            }
        }
        
        if (count % 110 == 0)
        {
            
            if (model.localScale.x >= 1.0f)
            {
                GameObject baby =Instantiate( // create new plant
                    gameObject, 
                    transform.position + new UnityEngine.Vector3( Random.Range(-3f, 3f),0f,Random.Range(-3f, 3f)), 
                    UnityEngine.Quaternion.identity
                );
            }
        }

        if (count > 4000)// old age
        {
            resize(0.99f);
            if (model.localScale.x < 0.2f)
            {
            Destroy(gameObject); // Destroy old plant
            }
        }
    }

    public void resize(float change)
    {
        // grow or shrink plant by change percent
        UnityEngine.Vector3 scale = model.localScale;
        scale *= change;
        model.localScale = scale;
        model.localPosition = new UnityEngine.Vector3(0f, scale.y / 2f, 0f);
    }


    public bool crowded()
    {
        float radius = model.localScale.x;
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            radius,
            ~0//changed
        );
        int count = hits.Length;
        return count > 4;
    }
}


