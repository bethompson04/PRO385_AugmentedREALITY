using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        TITLE,
        POND,
        FIGHT,
        AQUARIUM
    }

    public static GameManager instance { get; private set; }

    [Header("Game Manager")]
    public GameState gameState = GameState.TITLE;
    public PlayerInput playerInput;
    public InputAction touchPositionAction;
    public InputAction touchPressAction;

    [Header("Interaction Manager")]
    [SerializeField] public GameObject objectSpawner;
    public bool pondSpawned = false;

    [Header("Fish Spawner")]
    [SerializeField] public GameObject fishPrefab;
    [SerializeField] public GameObject fishDataUI;
    [SerializeField] public Slider clashBar;
    [SerializeField] GameObject currentFish;

    [Header("Aquarium")]
    [SerializeField] Aquarium aquarium;
    [SerializeField] DataList<PondFishData> pondFishList;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];

        fishDataUI.SetActive(false);
        clashBar.gameObject.SetActive(false);
        clashBar.value = 50;

        pondFishList = new DataList<PondFishData>();
        pondFishList = SystemIO.LoadFile<PondFishData>("pond_data");
        aquarium.LoadAquarium();
    }

    public void setState(int newState)
    {
        gameState = (GameState)newState;
        Debug.Log(gameState);
    }

    public void getAquarium()
    {
        aquarium.LoadAquarium();
        aquarium.SpawnFish();
    }

    public void setPondSpawned()
    {
        pondSpawned=true;
        objectSpawner.SetActive(false);
    }

    public void StartFight()
    {
        if (currentFish == null)
        {
            setState(2);
            Debug.Log("Fish caught!");
            SpawnFish();
        } else
        {
            Debug.Log("Fight not started. Current fish already exists.");
        }
    }

    public void EndFight(AquariumFishData data)
    {
        aquarium.AddFish(data);
        Destroy(currentFish);
        aquarium.SaveAquarium();

        fishDataUI.SetActive(false);
        setState(1);
    }

    public void SpawnFish()
    {
        clashBar.value = 50;

        currentFish = Instantiate(fishPrefab);
        currentFish.GetComponent<PondFish>().setFish(pondFishList.list[0]);
        Debug.Log("Fish created");

        //PondFish fishScript = fishPrefab.GetComponent<PondFish>();
        //Debug.Log("Fish prefab grabbed. || " + fishScript.ToString());
        //fishScript.setFish(pondFishList.list[0]);
        //currentFish = Instantiate(fishPrefab);
    }

    public void SetFishDataUI(float weight)
    {
        TMP_Text nameText = fishDataUI.GetNamedChild("Name").GetComponent<TMP_Text>();
        TMP_Text weightText = fishDataUI.GetNamedChild("Weight").GetComponent<TMP_Text>();
        TMP_Text descriptionText = fishDataUI.GetNamedChild("Description").GetComponent<TMP_Text>();

        PondFish fishScript = currentFish.GetComponent<PondFish>();
        nameText.text = fishScript.fishData.name;
        weightText.text = "Weight: " + weight.ToString();
        descriptionText.text = fishScript.fishData.description;
    }
}
