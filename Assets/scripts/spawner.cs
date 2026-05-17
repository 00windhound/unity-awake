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
        int mutateGene = Random.Range(0, 5);
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
            case 2://max height
                babyDna.maxHeight = Mathf.Clamp(babyDna.maxHeight + Random.Range(-0.2f, 0.2f), 0.4f, 3f);
                break;
            case 3://max thickness
                babyDna.maxThickness = Mathf.Clamp(babyDna.maxThickness + Random.Range(-0.2f, 0.2f), 0.4f, 3f);
                break;
            case 4://breeding frequency
                babyDna.freaquency = Mathf.Clamp(babyDna.freaquency + Random.Range(-10, 10), 50, 300);
                break;
            case 5://max age
                babyDna.maxAge = Mathf.Clamp(babyDna.maxAge + Random.Range(-500, 500), 2000, 10000);
                break;
            default:
                break;
        }
        baby.GetComponent<plants>().dna = babyDna;
    }
    
}