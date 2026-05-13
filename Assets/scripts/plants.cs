using UnityEngine;

public class plants : MonoBehaviour
{
    int count = 0;

    void Start()
    {

       
    }

    void Update()
    {
        count++;
        
        if (count == 10)
        {
            if (transform.rotation.z > 0.1f || transform.rotation.z < -0.1f)
            {
                transform.localScale *= 0.9f; // Shrink plant by 10%
                // if plant is too small, destroy it
                if (transform.localScale.x < 0.2f)
                {
                    // destroy self
                    Destroy(gameObject);
                } 
            }
            else if (transform.localScale.x < 1.0f) // Assuming 1.0 is the original scale
            {
                transform.localScale *= 1.1f; // Grow plant by 10%
            }
        }
        // 

        // if at or above 20 numbers:
        //    spawn baby, reset count


    }
}
