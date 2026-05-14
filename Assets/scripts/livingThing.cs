using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class livingThing : MonoBehaviour
{
    public long id;
    public int age = 0;
   
    
    
    
    protected virtual void Start()
    {
        id = global.Instance.newPlantId();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
       age++;
    }
}
