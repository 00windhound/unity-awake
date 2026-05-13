using System.Diagnostics;
using System.Numerics;
using UnityEngine;


public class plants : MonoBehaviour
{
    public LayerMask groundLayer; 
    int count = 0;

    void Start()
    {

       
    }

    void Update()
    {
        count++;
        
        // if count is a multiple of 10, check if on ground.
        
        if (count % 10 == 0)
        {
            bool seeground = false;
            bool isupright = true;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + UnityEngine.Vector3.up, UnityEngine.Vector3.down, out hit, 10f, groundLayer))
            {
                seeground = true;
            }

            float upright = UnityEngine.Vector3.Dot(transform.up, UnityEngine.Vector3.up);
            if (upright < 0.5f)
            {
                isupright = false;
            }

            UnityEngine.Debug.Log(seeground + " " + isupright);
            if (!seeground || !isupright)
            {
                transform.localScale *= 0.9f; // Shrink plant by 10%
                if (transform.localScale.x < 0.2f)
                {
                    Destroy(gameObject); // destroy small plant
                } 
            }
            
            else if (transform.localScale.x < 1.0f)
            {
                transform.localScale *= 1.01f; // Grow plant by 1%
            }
        }
        
        if (count % 100 == 0)
        {
            
            if (transform.localScale.x >= 1.0f)
            {
                GameObject baby =Instantiate( // create new plant
                    gameObject, 
                    transform.position + new UnityEngine.Vector3( Random.Range(-3f, 3f),0f,Random.Range(-3f, 3f)), 
                    UnityEngine.Quaternion.identity
                );
                baby.transform.localScale = UnityEngine.Vector3.one * 0.2f; // make baby small
            }
        }

        if (count > 1900)
        {
            transform.localScale *= 0.99f; // Shrink plant by 1%
        }
        if (transform.localScale.x < 0.2f)
        {
            Destroy(gameObject); // Destroy old plant
        }
    }


}
