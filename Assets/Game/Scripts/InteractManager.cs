using UnityEngine;

public class InteractManager : MonoBehaviour
{

	[SerializeField] public GameManager gameManager;
	[SerializeField] public GameObject objectSpawner;

	private static InteractManager _instance;
	private InteractManager()
	{

	}

	private void Awake()
	{
		
		_instance = this;
	}

	public static InteractManager GetInstance()
	{
		if (_instance == null)
		{
			Debug.Log("Fuck you");
		}
		return _instance;
	}

	public void PondSpawned()
	{
		//Debug.Log("Pond Spawned, Creation disabled");
		gameManager.setState(GameManager.GameState.POND);
		objectSpawner.SetActive(false);
	}

	public void PondDestroyed()
	{
		GameObject.FindGameObjectWithTag("Pond").GetComponent<PondManager>().DestroyPond();
		//Debug.Log("Pond Destroyed, Creation Enabled");
		gameManager.setState(GameManager.GameState.CREATION);
		objectSpawner.SetActive(true);
	}

	public void RippleClikced()
	{
		// Begin Ripple Behavior
		gameManager.setState(GameManager.GameState.FISHING);
		Debug.Log("Yo I'm a fish\n Time to fish fish!");
		if (!FishSpawner.instance.fishSpawned) FishSpawner.instance.SpawnFish();
	}

	public void BeginFishFury()
	{
		gameManager.setState(GameManager.GameState.FIGHT);
	}





}