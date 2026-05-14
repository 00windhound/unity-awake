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
    

    void Awake()
    {
        Instance = this;
    }

    
    void Update()
    {
        
    }

    public long newPlantId()
    {
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
       plantIds.Add(id); 
    }

    public void returnAnimalId(long id)
    {
        animalIds.Add(id);
    }

}
