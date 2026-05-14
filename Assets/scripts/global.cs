using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class global : MonoBehaviour
{
    public static global Instance;
    int plantId = 0;
    int animalId = 0;
    

    void Awake()
    {
        Instance = this;
    }

    
    void Update()
    {
        
    }

    public int newPlantId()
    {
        plantId++;
        return plantId;
    }

    public int newAnimalId()
    {
        animalId++;
        return animalId;
    }

}
