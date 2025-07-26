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
		NONE
    }
	
	[SerializeField] Transform modelParent;
	[SerializeField] Slider clashBar;

	[Header("Fish Animation")]
	[SerializeField] FishState fishState = FishState.SIDEWAYS_INTRO;
	[SerializeField] float lerpedRotationY;
	[SerializeField] float rotationDamping = 5f;
	[SerializeField] float lerpPositionDamping = 5f;
	[SerializeField] float forwardDistance = 1.3f;

	[Header("Fish Data")]
	[SerializeField] float attack = 1;
	[SerializeField] float defense = 3;
	[SerializeField] float minWeight = 0.4f;
	[SerializeField] float maxWeight = 1f;

	private bool newStateTransition = true;
	private Camera cam;
	private PlayerInput playerInput;
	private InputAction touchPositionAction;
	private InputAction touchPressAction;
	private bool isPressed;

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		touchPressAction = playerInput.actions["TouchPress"];
		touchPositionAction = playerInput.actions["TouchPosition"];
	}


	void Start()
    {
		cam = Camera.main;
		clashBar.value = 50;
	}

    void Update()
    {
        switch (fishState)
		{
			case FishState.SIDEWAYS_INTRO:
                if (newStateTransition)
				{
					StartCoroutine(LerpRotation(0, -90f, 2f, FishState.COMBAT));
					newStateTransition = false;
				}

				FaceCamera(lerpedRotationY);
				break;
			case FishState.COMBAT:
				if (newStateTransition)
				{
					StartCoroutine(LerpRotation(-90, 0f, 0.5f, FishState.NONE));
					newStateTransition = false;
				}

				CombatUpdate();
				FaceCamera(lerpedRotationY);

				break;
			case FishState.END:
				StartCoroutine(LerpRotation(0, -90f, 2f, FishState.NONE));
				FaceCamera(lerpedRotationY);
				break;
		}
    }

    void FaceCamera(float modelRotation)
    {
        Vector3 camPos = cam.transform.position;
		Vector3 ourPos = gameObject.transform.position;

		modelParent.localEulerAngles = new Vector3(0, modelRotation, 0);
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
        //modelParent.eulerAngles = new Vector3 (0, lerpedRotationY, 0);

		if (endState != FishState.NONE)
		{
			fishState = endState;
			print("true!");
			newStateTransition = true;
		}
	}

	void CombatUpdate()
	{
		if (clashBar.value >= clashBar.maxValue)
		{
			fishState = FishState.END;
			return;
		}

		clashBar.value -= defense * Time.deltaTime;

		if (touchPressAction.IsPressed())
		{
			clashBar.value += attack;
			print("bam");
		}
	}

	private void TouchPressed(InputAction.CallbackContext context)
	{
		Vector2 position = touchPositionAction.ReadValue<Vector2>();
	}
}
