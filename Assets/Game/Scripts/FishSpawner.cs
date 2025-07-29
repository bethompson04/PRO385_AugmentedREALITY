using UnityEngine;
using UnityEngine.UI;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] GameObject tempFish;
	[SerializeField] GameObject fishDataUI;
	[SerializeField] Slider clashBar;

	void Start()
    {
		fishDataUI.SetActive(false);
		clashBar.gameObject.SetActive(false);
		clashBar.value = 50;
		SpawnFish();
    }

    public void SpawnFish()
    {
		clashBar.value = 50;

		//play animation from pond coordinate
		//have fish jump facing sideways from camera

		Fish fishScript = tempFish.GetComponent<Fish>();
        fishScript.clashBar = this.clashBar;
		Instantiate(tempFish);
	}
}
