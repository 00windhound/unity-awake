using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Collections.Generic;// to make my save lists work
using System.IO;// to save and load files
using UnityEngine;

public class global : MonoBehaviour
{
    public static global Instance;
    public GameObject plantPrefab;
    public long plantId = 0;
    long animalId = 0;
    List<long> plantIds = new List<long>();
    List<long> animalIds = new List<long>();
    public List<plants> plantsRunningList = new List<plants>();// create the running list
    public List<plants.PlantData> plantsToSave = new  List<plants.PlantData>();
    long totalPlants = 0;
    long totalAnimals = 0;
    float timer = 30f;
    bool timeout = false;
    int x = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Load();
    }
    

    void Update()
    {
        for(int i = 0; i < 20; i++)
        {   
            if (x < 0)
            {
                if (timeout)
                {
                    x = plantsRunningList.Count;
                    timeout = false;
                }
            }
            else if (plantsRunningList[x])
            {
                plantsRunningList[x].UpdatePlant();
                if(timeout){Debug.Log("timeout. " + x + " plants still need to update. adjust plants per frame or timeout length to avoid plant growth delay.");}
            }
            x--;
        }
        timer -= Time.deltaTime;
        if (timer <= 0){timeout = true; timer = 30f;}// restart timer

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
        animalId++; return animalId;
    }

    public void returnPlantId(long id)
    {totalPlants--; plantIds.Add(id);}

    public void returnAnimalId(long id)
    {totalAnimals--; animalIds.Add(id);}


    public void Save()
    {
        plantsToSave.Clear();
        foreach (plants p in plantsRunningList)
        {
            if(p == null){continue;}
            plantsToSave.Add(p.Data());
        }
        
        SaveData saveData = new SaveData();
        saveData.plants = plantsToSave;
        saveData.plantId = plantId;
        saveData.plantIds = plantIds;

        string json = JsonUtility.ToJson(saveData, true);
        
        string path = Application.persistentDataPath + "/saveGame.json";
        File.WriteAllText(path, json);
        Debug.Log("Game saved!");
        //Debug.Log(Application.persistentDataPath);// gives me the path to see my saved data
    }


    public void Load()
    {
        string path = Application.persistentDataPath + "/savegame.json";
        if (!File.Exists(path)){Debug.Log("No save file found."); return;}
        
        string json = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        
        //load plants
        foreach (plants.PlantData data in saveData.plants)
        {
        // create plant and give it the saved data
        GameObject newPlant = Instantiate(plantPrefab, data.position, Quaternion.identity);
        plants plant = newPlant.GetComponent<plants>();

        plant.LoadData(data);
        }

        // load global id data
        plantId = saveData.plantId;
        plantIds = saveData.plantIds;

    }



    [System.Serializable]
    public class SaveData
    {
        public List<plants.PlantData> plants;
        public long plantId;
        public List<long> plantIds;
        // public List<animals.AnimalData> animals;
    } 


}


  