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

    public void setState(GameState newState)
    {
        state = newState;
        Debug.Log(state);
    }
}
