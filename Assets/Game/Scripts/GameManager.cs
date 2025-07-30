using UnityEngine;

[System.Serializable]
public struct PondFishData
{
    public string name;
    public string description;
    public string modelPath;
    public int attack;
    public int defense;
    public float minWeight;
    public float maxWeight;
}

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        TITLE,
        CREATION,
        POND,
        FISHING,
        FIGHT,
        AQUARIUM
    }

    public static GameManager instance {  get; private set; }
    public GameState state;

    public Aquarium aquarium;
    public DataList<PondFishData> pondDataList;

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
        pondDataList = SystemIO.LoadFile<PondFishData>("pond_data");
        aquarium.LoadAquarium();
    }

    public void setState(GameState newState)
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
