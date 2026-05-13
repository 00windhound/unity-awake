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
        
        if (count == 10 || count == 30 || count == 50 || count == 70 || count == 90)
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
            else if (transform.localScale.x < 1.0f)
            {
                transform.localScale *= 1.01f; // Grow plant by 10%
            }
        }
        
        if (count == 200 || count == 400 || count == 600 || count == 800 || count == 1000)
        {
            // spawn baby plant
            Vector3 offset = new Vector3(
            Random.Range(-3f, 3f),
            0
            ,Random.Range(-3f, 3f)
            );

            Instantiate(gameObject, transform.position + offset, Quaternion.identity);
        }

        if (count > 1000)
        {
            transform.localScale *= 0.99f; // Shrink plant by 1%
        }
        if (transform.localScale.x < 0.2f)
        {
            Destroy(gameObject); // Destroy small plant
        }
    }


}
