using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class FishData
{
    public string name;
    public string description;
    public string modelPath;
    public int attack;
    public int defense;
    public float minWeight;
    public float maxWeight;
}

[System.Serializable]
public class AquariumFishData
{
    public string name;
    public string description;
    public string modelPath;
    public float weight;
}

[System.Serializable]
public class FishDataList
{
    public List<AquariumFishData> list;
}

public class AquariumIO : MonoBehaviour
{

    public FishDataList aquarium;

    public void SaveAquarium()
    {
        string json = JsonUtility.ToJson(aquarium, true);

        using (StreamWriter writer = new StreamWriter("Assets\\Game\\Data\\aquarium.json"))
        {
            writer.Write(json);
        }
    }

    public void LoadAquarium()
    {
        string json = string.Empty;

        using (StreamReader reader = new StreamReader("Assets\\Game\\Data\\aquarium.json"))
        {
            json = reader.ReadToEnd();
        }

        aquarium = JsonUtility.FromJson<FishDataList>(json);
        Debug.Log(aquarium.list.Count);
    }

    public void AddFishToAquarium(AquariumFishData fish)
    {
        aquarium.list.Add(fish);
    }

    public void RemoveFishFromAquarium(int index)
    {
        if (aquarium.list.Count > 0) aquarium.list.RemoveAt(index);
    }
}