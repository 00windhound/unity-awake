using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using UnityEngine;

public class livingThing : MonoBehaviour
{
    int id;
    int age = 0;
    int maxAge = 1000;
    bool MaxAgeReached = false;
    void Start()
    {
        id = newPlantId();
        Debug.Log("i got ID: " + id);
    }

    // Update is called once per frame
    void Update()
    {
       age++;
       if (age > maxAge)
       {
            Debug.log("old age reached");
            MaxAgeReached = true;
       } 
    }
}
