using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Fish : MonoBehaviour
{
	public enum FishState
    {
        SIDEWAYS_INTRO,
        COMBAT,
        END,
		LOSE,
		NONE
    }

	private FishSpawner fishSpawner = FishSpawner.instance;
	
	[SerializeField] Transform modelParent;
	[SerializeField] GameObject boxingGloves;
	

	[Header("Fish Animation")]
	[SerializeField] public FishState fishState = FishState.SIDEWAYS_INTRO;
	
	[SerializeField] float lerpedRotationY;
	[SerializeField] float rotationDamping = 5f;
	[SerializeField] float lerpPositionDamping = 5f;
	[SerializeField] float forwardDistance = 0.5f;


	[Header("Fish Data")]
	[SerializeField] public AquariumFishData fishData;
	private float attack = 5;
	private float defense = 10;
	
	private float lockout;
	private bool newStateTransition = true;
	private Camera cam;
	private PlayerInput playerInput;
	private InputAction touchPositionAction;
	private InputAction touchPressAction;
	//private bool isPressed;

	private void Awake()
	{
		boxingGloves.SetActive(false);
		playerInput = GetComponent<PlayerInput>();
		touchPressAction = playerInput.actions["TouchPress"];
		touchPositionAction = playerInput.actions["TouchPosition"];
	}


	void Start()
    {
		cam = Camera.main;
	}

    void Update()
    {
        switch (fishState)
		{
			case FishState.SIDEWAYS_INTRO:
                if (newStateTransition)
				{
					StartCoroutine(LerpRotation(0, -90f, 4f, FishState.COMBAT));
					newStateTransition = false;
				}

				FaceCamera(lerpedRotationY);

				break;
			case FishState.COMBAT:
				if (newStateTransition)
				{
					fishSpawner.clashBar.gameObject.SetActive(true);

					boxingGloves.SetActive(true);
					StartCoroutine(LerpRotation(-90, 0f, 0.5f, FishState.NONE));
					newStateTransition = false;
				}

				CombatUpdate();
				FaceCamera(lerpedRotationY);

				break;
			case FishState.END:
				if (newStateTransition)
				{
					fishSpawner.clashBar.gameObject.SetActive(false);
					fishSpawner.fishDataUI.SetActive(true);
					fishSpawner.SetFishDataUI();

					boxingGloves.SetActive(false);
					StartCoroutine(LerpRotation(0, -90f, 2f, FishState.NONE));
					newStateTransition = false;
				}

				
				EndUpdate();
				FaceCamera(lerpedRotationY);

				break;
			case FishState.LOSE:
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

    IEnumerator LerpRotation(float start, float end, float lerpDuration, FishState endState)
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

		if (endState != FishState.NONE)
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
			fishState = FishState.END;
			return;
		}

		fishSpawner.clashBar.value -= defense * Time.deltaTime;

		if (touchPressAction.WasPerformedThisFrame())
		{
			fishSpawner.clashBar.value += attack;
			print("wabam!");
		}
	}

	void EndUpdate()
	{
		if (touchPressAction.WasPerformedThisFrame())
		{
			fishSpawner.EndOfFish(fishData);
			Destroy(gameObject);
		}
	}
}
