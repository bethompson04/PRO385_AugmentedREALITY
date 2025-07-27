using UnityEngine;

[System.Serializable]
public class AquariumFishData
{
    public string name;
    public string description;
    public string modelPath;
    public int attack;
    public int defense;
    public float minWeight;
    public float maxWeight;
}

public class Aquarium : MonoBehaviour
{
    [SerializeField] DataList<AquariumFishData> aquariumList;

    private void Awake()
    {
        LoadAquarium();
    }

    public void LoadAquarium()
    {
        aquariumList = SystemIO.LoadFile<AquariumFishData>("aquarium_data");
    }

    public void SaveAquarium()
    {
        SystemIO.SaveFile(aquariumList, "aquarium_data");
    }
}
