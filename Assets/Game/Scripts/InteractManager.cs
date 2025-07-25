using UnityEngine;

public class InteractManager : MonoBehaviour {

  [SerializeField] public GameManager gameManager;
  [SerializeField] public GameObject objectSpawner;

  private static InteractManager _instance;
  private InteractManager() {

  }

  private void Awake() {
    _instance = this;
  }

  public static InteractManager GetInstance() {
    if (_instance == null) {
      Debug.Log("Fuck you");
    }
    return _instance;
  }
    
  public void PondSpawned() {
    Debug.Log("Pond Spawned, Creation disabled");
    gameManager.setState(GameManager.GameState.POND);
    objectSpawner.SetActive(false);
  }

  public void PondDestroyed() {
    Destroy(GameObject.FindGameObjectWithTag("Pond"));
    Debug.Log("Pond Destroyed, Creation Enabled");
    gameManager.setState(GameManager.GameState.CREATION);
    objectSpawner.SetActive(true);
  }

  public void RippleClikced() {
    // Begin Ripple Behavior
  }

  public void BeginFishFury() {
    gameManager.setState(GameManager.GameState.FIGHT);
  }





}