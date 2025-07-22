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

    public void setState(GameState newState)
    {
        state = newState;
        Debug.Log(state);
    }

    private void Update()
    {
        InputSystem.onAnyButtonPress.Call(currentAction =>
        {
            switch (instance.state)
            {
                case GameState.TITLE:
                    instance.setState(GameState.POND);
                    break;
                case GameState.POND:
                    instance.setState(GameState.FIGHT);
                    break;
                case GameState.FIGHT:
                    instance.setState(GameState.AQUARIUM);
                    break;
                case GameState.AQUARIUM:
                    instance.setState(GameState.TITLE);
                    break;
            }
        });
        
    }
}
