using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class livingThing : MonoBehaviour
{
    public int id;
    public int age = 0;
   
    
    
    
    protected virtual void Start()
    {
        id = global.Instance.newPlantId();
        UnityEngine.Debug.Log("i got ID: " + id);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
       age++;
       UnityEngine.Debug.Log("age: " + age);
    }
}
