using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
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
    public void SpawnPlant(Vector3 position, plantDNA parentDna)
    {
        GameObject baby = Instantiate(plantPrefab, position, Quaternion.identity);
        plantDNA babyDna = parentDna.Clone();

        // dna mutation
        int mutateGene = Random.Range(0, 2);
        switch(mutateGene)
        {
            case 0:// none
                break;
            case 1:// stem color
                babyDna.stemColor = new Color(
                    Mathf.Clamp01(babyDna.stemColor.r + Random.Range(-0.2f, 0.2f)),
                    Mathf.Clamp01(babyDna.stemColor.g + Random.Range(-0.2f, 0.2f)),
                    Mathf.Clamp01(babyDna.stemColor.b + Random.Range(-0.2f, 0.2f))
                );
                break;
            default:
                break;
        }
        baby.GetComponent<plants>().dna = babyDna;
    }
    
}