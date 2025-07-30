using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class FishSpawner : MonoBehaviour
{
	public static FishSpawner instance { get; private set; }

	[SerializeField] public GameObject tempFish;
	[SerializeField] public GameObject fishDataUI;
	[SerializeField] public Slider clashBar;
	[SerializeField] public Aquarium aquarium;
	//[SerializeField] bool spawnOnStart;

	private GameObject currentFish;
	public bool fishSpawned = false;

	private void Awake()
	{
		if (instance != null && instance != this) Destroy(gameObject);
		else instance = this;
	}

	void Start()
    {
		fishDataUI.SetActive(false);
		clashBar.gameObject.SetActive(false);
		clashBar.value = 50;
		//if (spawnOnStart) SpawnFish();
    }

	public void SpawnFish()
    {
		clashBar.value = 50;

		//play animation from pond coordinate
		//have fish jump facing sideways from camera

		Fish fishScript = tempFish.GetComponent<Fish>();
		currentFish = Instantiate(tempFish);
		fishSpawned = true;
	}

	public void EndOfFish(AquariumFishData aquariumFishData)
	{
		aquarium.AddFish(aquariumFishData);
		aquarium.SaveAquarium();

		fishDataUI.SetActive(false);
		fishSpawned = false;
	}

	public void SetFishDataUI()
	{
		TMP_Text nameText = fishDataUI.GetNamedChild("Name").GetComponent<TMP_Text>();
		TMP_Text weightText = fishDataUI.GetNamedChild("Weight").GetComponent<TMP_Text>();
		TMP_Text descriptionText = fishDataUI.GetNamedChild("Description").GetComponent<TMP_Text>();

		Fish fishScript = currentFish.GetComponent<Fish>();
		nameText.text = fishScript.fishData.name;
		weightText.text = "Weight: " + fishScript.fishData.weight.ToString();
		descriptionText.text = fishScript.fishData.description;
	}
}
