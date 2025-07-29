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
	[SerializeField] bool spawnOnStart;

	private GameObject currentFish;

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
		if (spawnOnStart) SpawnFish();
    }

	public void SpawnFish()
    {
		clashBar.value = 50;

		//play animation from pond coordinate
		//have fish jump facing sideways from camera

		Fish fishScript = tempFish.GetComponent<Fish>();
		currentFish = Instantiate(tempFish);
	}

	public void SetFishDataUI()
	{
		TMP_Text nameText = fishDataUI.GetNamedChild("Name").GetComponent<TMP_Text>();
		TMP_Text weightText = fishDataUI.GetNamedChild("Weight").GetComponent<TMP_Text>();
		TMP_Text descriptionText = fishDataUI.GetNamedChild("Description").GetComponent<TMP_Text>();

		Fish fishData = currentFish.GetComponent<Fish>();
		nameText.text = fishData.fishName;
		weightText.text = "Weight: " + fishData.weight.ToString();
		descriptionText.text = fishData.description;
	}
}
