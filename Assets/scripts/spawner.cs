using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public static spawner InstanceCreator;
    public GameObject plantPrefab;
    public GameObject animalPrefab;

    public int plantsPerFrame = 5;

    Queue<PlantSpawnRequest> spawnQueue =
        new Queue<PlantSpawnRequest>();


    // nested class
    public class PlantSpawnRequest
    {
        public UnityEngine.Vector3 position;
        public plantDNA dna;
        public float timeCreated;
    }


    void Awake()//testing
    {
        InstanceCreator = this;
    }

    // add itterative delay
    public void SpawnPlant(UnityEngine.Vector3 position, plantDNA parentDna)
    {
        PlantSpawnRequest request = new PlantSpawnRequest();
        request.position = position;
        request.dna = parentDna;
        request.timeCreated = Time.time;
        spawnQueue.Enqueue(request);

    }


    void Update()
    {
        //UnityEngine.Debug.Log("spawn queue count: " + spawnQueue.Count);
        if (spawnQueue.Count > 0)// if not too old
        {
            int i = 0;
            while (i <= plantsPerFrame && spawnQueue.Count > 0)
            {
            
                PlantSpawnRequest request = spawnQueue.Dequeue();
                if (Time.time - request.timeCreated > 5f)
                {
                    continue; // skip if too old  
                }
                GameObject baby = Instantiate(plantPrefab, request.position, UnityEngine.Quaternion.identity);
                plantDNA babyDna = request.dna.Clone();

                // dna mutation
                int mutateGene = Random.Range(0, 15);
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
                    case 2:// trunk segment 1
                        babyDna.trunkSegment1Width = Mathf.Clamp(babyDna.trunkSegment1Width + Random.Range(-0.2f, 0.2f), 0.1f, 5f);
                        break;
                    case 3:// trunk segment 2
                        babyDna.trunkSegment2Width = Mathf.Clamp(babyDna.trunkSegment2Width + Random.Range(-0.2f, 0.2f), 0.1f, 5f);
                        break;
                    case 4:// trunk segment 3
                        babyDna.trunkSegment3Width = Mathf.Clamp(babyDna.trunkSegment3Width + Random.Range(-0.2f, 0.2f), 0.1f, 5f);
                        break;
                    case 5:// trunk segment 4
                        babyDna.trunkSegment4Width = Mathf.Clamp(babyDna.trunkSegment4Width + Random.Range(-0.2f, 0.2f), 0.1f, 5f);
                        break;
                    case 6://max height
                        babyDna.maxHeight = Mathf.Clamp(babyDna.maxHeight + Random.Range(-0.2f, 0.2f), 0.4f, 3f);
                        break;
                    case 7://max thickness
                        babyDna.maxThickness = Mathf.Clamp(babyDna.maxThickness + Random.Range(-0.2f, 0.2f), 0.4f, 3f);
                        break;
                    case 8://stick count
                        babyDna.stickCount = Mathf.Clamp(babyDna.stickCount + Random.Range(-1, 2), 0, 10);
                        break;
                    case 9://stick length
                        babyDna.stickLength = Mathf.Clamp(babyDna.stickLength + Random.Range(-0.5f, 0.5f), 0.4f, 6f);
                        break;
                    case 10://stick thickness
                        babyDna.stickThickness = Mathf.Clamp(babyDna.stickThickness + Random.Range(-0.5f, 0.5f), 0.1f, 6f);
                        break;
                    case 11://stick lowest y
                        babyDna.stickLowestY = Mathf.Clamp(babyDna.stickLowestY + Random.Range(-0.1f, 0.1f), 0.01f, 0.9f);
                        break;
                    case 12://stick downward angle
                        babyDna.stickDownwardAngle = Mathf.Clamp(babyDna.stickDownwardAngle + Random.Range(-5f, 6f), 10f, 90f);
                        break;
                    case 13://breeding frequency
                        babyDna.breedingFrequency = Mathf.Clamp(babyDna.breedingFrequency + Random.Range(-1, 1), 1, 100);
                        break;
                    case 14://max age
                        //babyDna.maxAge = Mathf.Clamp(babyDna.maxAge + Random.Range(-500, 501), 2, 10000);
                        break;
                    default:
                        break;
                }
                baby.GetComponent<plants>().dna = babyDna;
                i++;
            }
        }
    }
    
}