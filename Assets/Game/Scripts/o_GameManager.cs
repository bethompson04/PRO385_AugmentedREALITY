using UnityEngine;

[System.Serializable]
public struct o_PondFishData
{
    public string name;
    public string description;
    public string modelPath;
    public int attack;
    public int defense;
    public float minWeight;
    public float maxWeight;
}

public class o_GameManager : MonoBehaviour
{
    public enum o_GameState
    {
        TITLE,
        CREATION,
        POND,
        FISHING,
        FIGHT,
        AQUARIUM
    }

    public static o_GameManager instance {  get; private set; }
    public o_GameState state;

    public Aquarium aquarium;
    public DataList<o_PondFishData> pondDataList;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        pondDataList = SystemIO.LoadFile<o_PondFishData>("pond_data");
        aquarium.LoadAquarium();
    }

    public void setState(o_GameState newState)
    {
        state = newState;
        Debug.Log(state);
    }

    public void getAquarium()
    {
        if (aquarium == null) aquarium = new Aquarium();
        aquarium.LoadAquarium();
        aquarium.SpawnFish();
    }

    public void deleteAquarium()
    {
        aquarium.DeleteFish();
    }
}
