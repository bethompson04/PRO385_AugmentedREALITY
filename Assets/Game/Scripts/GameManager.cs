using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        TITLE,
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

    private void Start()
    {
        //FishData fishData;
    }

    public void setState(GameState newState)
    {
        state = newState;
        Debug.Log(state);
    }
}
