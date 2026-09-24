using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Collections.Generic;// to make my save lists work
using System.IO;// to save and load files
using UnityEngine;

public class global : MonoBehaviour
{
    public static global Instance;
    long plantId = 0;
    long animalId = 0;
    List<long> plantIds = new List<long>();
    List<long> animalIds = new List<long>();
    public List<plants> plantsRunningList = new List<plants>();// create the running list
    public List<plants.PlantData> plantsToSave = new  List<plants.PlantData>();
    long totalPlants = 0;
    long totalAnimals = 0;
    float timer = 1.2f;
    bool timeout = false;
    int plantsPerFrame = 10;
    int x = 0;

    void Awake()
    {
        Instance = this;
    }

    

    void Update()
    {
        for(int i = 0; i < plantsPerFrame; i++)
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
            }
            x--;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timeout = true;
            timer = 1.2f; // restart timer
        }

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


    public void Save()
    {
        //Debug.Log("save pressed!");
        plantsToSave.Clear();
        foreach (plants p in plantsRunningList)
        {
            if(p == null){continue;}
            plantsToSave.Add(p.Data());
        }
        
        SaveData saveData = new SaveData();
        saveData.plants = plantsToSave;

        string json = JsonUtility.ToJson(saveData, true);
        Debug.Log("json: " + json);// json {} is empty
        string path = Application.persistentDataPath + "/saveGame.json";
        File.WriteAllText(path, json);

        //Debug.Log(Application.persistentDataPath);// gives me the path to see my saved data
    }

    [System.Serializable]
    public class SaveData
    {
        public List<plants.PlantData> plants;
        // public List<animals.AnimalData> animals;
    } 


}







// this is where i will save, load, autosave, and delete the game
/*
public class SaveGame : MonoBehaviour
{// maybe i don't want this page
    public List<plants> allPlants = new List<plants>();// create the running list

    public void SaveGame()
{
    SaveData saveData = new SaveData();

    foreach (plants plant in worldManager.Instance.allPlants)
    {
        PlantSaveData plantData = plant.GetSaveData();

        saveData.plants.Add(plantData);
    }

    string json = JsonUtility.ToJson(saveData, true);

    string path =
        Application.persistentDataPath + "/savegame.json";

    File.WriteAllText(path, json);
}

}*/