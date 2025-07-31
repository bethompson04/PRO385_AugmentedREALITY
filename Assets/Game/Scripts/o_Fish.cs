using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class o_Fish : MonoBehaviour
{
	public enum o_FishState
    {
        SIDEWAYS_INTRO,
        COMBAT,
        END,
		LOSE,
		NONE
    }

	private o_FishSpawner fishSpawner = o_FishSpawner.instance;
	
	[SerializeField] Transform modelParent;
	[SerializeField] GameObject boxingGloves;
	[SerializeField] AudioClip[] clipList;
	[SerializeField] AudioSource fightStartSource;

	private AudioSource audioSource;

    [SerializeField] float victoryLength = 2f;
    private float endTimer = 0;

    [Header("Fish Animation")]
	[SerializeField] public o_FishState fishState = o_FishState.SIDEWAYS_INTRO;
	
	[SerializeField] float lerpedRotationY;
	[SerializeField] float rotationDamping = 5f;
	[SerializeField] float lerpPositionDamping = 5f;
	[SerializeField] float forwardDistance = 0.5f;


	[Header("Fish Data")]
	[SerializeField] public o_PondFishData fishData;
	private float weight = 0;
	
	private float lockout;
	private bool newStateTransition = true;
	private Camera cam;
	private PlayerInput playerInput;
	private InputAction touchPositionAction;
	private InputAction touchPressAction;
	private float endTouchCooldown = 1.0f;
	private float endTouchTimer = 0f;
	private bool canTouchInEndState = false;


	//private bool isPressed;
	private Animator animator;

	private void Awake()
	{
		boxingGloves.SetActive(false);
		playerInput = GetComponent<PlayerInput>();
		touchPressAction = playerInput.actions["TouchPress"];
		touchPositionAction = playerInput.actions["TouchPosition"];
	}


	void Start()
    {
		animator = GetComponent<Animator>();
		cam = Camera.main;

		audioSource = GetComponent<AudioSource>();
	}

	public void setFish(o_PondFishData fish)
	{
		fishData = fish;
        var model = Resources.Load<Mesh>(fishData.modelPath);
        var material = Resources.Load<Material>(fishData.modelPath);
        if (model != null && material != null)
        {
            gameObject.GetComponentInChildren<MeshFilter>().mesh = model;
            gameObject.GetComponentInChildren<MeshRenderer>().material = material;
        }
    }

    void Update()
    {
        switch (fishState)
		{
			case o_FishState.SIDEWAYS_INTRO:
                if (newStateTransition)
				{
					StartCoroutine(LerpRotation(0, -90f, 4f, o_FishState.COMBAT));
					newStateTransition = false;
                    audioSource.clip = clipList[0];
                    audioSource.Play();
                }

				FaceCamera(lerpedRotationY);

				break;
			case o_FishState.COMBAT:
				if (newStateTransition)
				{
					o_GameManager.instance.setState(o_GameManager.o_GameState.FIGHT);
					fishSpawner.clashBar.gameObject.SetActive(true);
					fightStartSource.Play();
					boxingGloves.SetActive(true);
					StartCoroutine(LerpRotation(-90, 0f, 0.5f, o_FishState.NONE));
					newStateTransition = false;
					
					audioSource.clip = clipList[1];
					animator.SetBool("Combat", true);
				}

				CombatUpdate();
				FaceCamera(lerpedRotationY);

				break;
			case o_FishState.END:
				if (newStateTransition)
				{
					fishSpawner.clashBar.gameObject.SetActive(false);
					fishSpawner.fishDataUI.SetActive(true);
                    weight = UnityEngine.Random.Range(fishData.minWeight, fishData.maxWeight);
                    fishSpawner.SetFishDataUI(weight);

					boxingGloves.SetActive(false);
					StartCoroutine(LerpRotation(0, -90f, 2f, o_FishState.NONE));
					newStateTransition = false;
                    audioSource.clip = clipList[2];
					audioSource.Play();

					o_GameManager.instance.setState(o_GameManager.o_GameState.POND);
					animator.SetBool("Combat", false);

					endTouchTimer = 0f;
					canTouchInEndState = false;
				}

				
				EndUpdate();
				FaceCamera(lerpedRotationY);

				endTimer += Time.deltaTime;

				break;
			case o_FishState.LOSE:
				Destroy(gameObject);
				break;
		}
    }

    void FaceCamera(float modelRotation)
    {
        Vector3 camPos = cam.transform.position;
		Vector3 ourPos = gameObject.transform.position;

		// Apply Y Rotation
		modelParent.localRotation = Quaternion.Euler(0f, modelRotation, 0f);

		// Fish Face Camera
		Vector3 relativePos = camPos - gameObject.transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
		gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rotation, Time.deltaTime * rotationDamping);

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, camPos + cam.transform.forward * forwardDistance, Time.deltaTime * lerpPositionDamping);
	}

    IEnumerator LerpRotation(float start, float end, float lerpDuration, o_FishState endState)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;

            t = t * t * (3f - 2f * t);
			
			lerpedRotationY = Mathf.LerpAngle(start, end, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

		lerpedRotationY = end;

		if (endState != o_FishState.NONE)
		{
			fishState = endState;
			newStateTransition = true;
		}
	}

	void CombatUpdate()
	{
		if (fishSpawner.clashBar.value >= fishSpawner.clashBar.maxValue)
		{
			newStateTransition = true;
			fishState = o_FishState.END;
			return;
		}

		fishSpawner.clashBar.value -= fishData.defense * Time.deltaTime;

        if (touchPressAction.WasPerformedThisFrame())
		{
			fishSpawner.clashBar.value += fishData.attack;
			audioSource.Play();
			//print("wabam!");
		}
	}

	void EndUpdate()
	{
		if (touchPressAction.WasPerformedThisFrame() && endTimer >= victoryLength)
		{
            AquariumFishData data = new AquariumFishData();
            data.name = fishData.name;
            data.description = fishData.description;
            data.modelPath = fishData.modelPath;
            data.weight = weight;

            fishSpawner.EndOfFish(data);
			Destroy(gameObject);
		}
	}
}
