// this is where i will save, load, autosave, and delete the game
public class SaveGame : MonoBehaviour
{
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

}