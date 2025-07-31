using UnityEngine;

public class o_InteractManager : MonoBehaviour
{

	[SerializeField] public o_GameManager gameManager;
	[SerializeField] public GameObject objectSpawner;

	private static o_InteractManager _instance;
	private o_InteractManager()
	{

	}

	private void Awake()
	{
		
		_instance = this;
	}

	public static o_InteractManager GetInstance()
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
		gameManager.setState(o_GameManager.o_GameState.POND);
		objectSpawner.SetActive(false);
	}

	public void PondDestroyed()
	{
		GameObject.FindGameObjectWithTag("Pond").GetComponent<o_PondManager>().DestroyPond();
		//Debug.Log("Pond Destroyed, Creation Enabled");
		gameManager.setState(o_GameManager.o_GameState.CREATION);
		objectSpawner.SetActive(true);
	}

	public void RippleClikced()
	{
		// Begin Ripple Behavior
		gameManager.setState(o_GameManager.o_GameState.FISHING);
		Debug.Log("Yo I'm a fish\n Time to fish fish!");
		if (!o_FishSpawner.instance.fishSpawned) o_FishSpawner.instance.SpawnFish();
	}

	public void BeginFishFury()
	{
		gameManager.setState(o_GameManager.o_GameState.FIGHT);
	}

}