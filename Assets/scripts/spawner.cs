using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public static spawner InstanceCreator;
    public GameObject plantPrefab;
    public GameObject animalPrefab;


    void Awake()//testing
    {
        InstanceCreator = this;
    }

    // add itterative delay
    public void SpawnPlant(Vector3 position, plantDNA dna)
    {
        GameObject plant = Instantiate(plantPrefab, position, Quaternion.identity);
        plant.GetComponent<plants>().dna = dna;
    }
    
}