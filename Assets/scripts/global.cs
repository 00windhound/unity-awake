using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class global : MonoBehaviour
{
    public static global Instance;
    long plantId = 0;
    long animalId = 0;
    List<long> plantIds = new List<long>();
    List<long> animalIds = new List<long>();
    long totalPlants = 0;
    long totalAnimals = 0;

    void Awake()
    {
        Instance = this;
    }

    
    void Update()
    {
        
    }

    public long newPlantId()
    {
        totalPlants++;
        // check if there are any returned plant ids to reuse
        if (plantIds.Count > 0)
        {
            long id = plantIds[0];
            plantIds.RemoveAt(0);
            return id;
        }
        plantId++;
        return plantId;
    }

    public long newAnimalId()
    {
        totalAnimals++;
        // check if there are any returned animal ids to reuse
        if (animalIds.Count > 0)
        {
            long id = animalIds[0];
            animalIds.RemoveAt(0);
            return id;
        }
        animalId++;
        return animalId;
    }

    public void returnPlantId(long id)
    {
        totalPlants--;
        plantIds.Add(id); 
    }

    public void returnAnimalId(long id)
    {
        totalAnimals--;
        animalIds.Add(id);
    }

    public void firstButton()
    {
        Debug.Log("first button pressed!");
    }

}
