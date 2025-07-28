using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        TITLE,
        CREATION,
        POND,
        FIGHT,
        AQUARIUM
    }

    public static GameManager instance {  get; private set; }
    public GameState state;

    public Aquarium aquarium;

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
        aquarium = gameObject.GetComponent<Aquarium>();
        aquarium.LoadAquarium();
    }

    public void setState(GameState newState)
    {
        state = newState;
        Debug.Log(state);
    }
}
