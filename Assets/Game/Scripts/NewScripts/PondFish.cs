using System.Collections;
using UnityEngine;
using static o_Fish;

[System.Serializable]
public struct PondFishData
{
    public string name;
    public string description;
    public string modelPath;
    public int attack;
    public int defense;
    public float minWeight;
    public float maxWeight;
}

public class PondFish : MonoBehaviour
{
    public enum FishState
    {
        INTRO,
        COMBAT,
        END,
        NONE
    }

    [SerializeField] Transform modelParent;
    [SerializeField] GameObject boxingGloves;
    [SerializeField] AudioClip[] clipList;
    [SerializeField] AudioSource fightStartSource;

    [SerializeField] AudioSource audioSource;

    [SerializeField] float victoryLength = 2f;
    private float endTimer = 0;

    [Header("Fish Animation")]
    [SerializeField] public FishState fishState = FishState.INTRO;

    [SerializeField] float lerpedRotationY;
    [SerializeField] float rotationDamping = 5f;
    [SerializeField] float lerpPositionDamping = 5f;
    [SerializeField] float forwardDistance = 0.5f;


    [Header("Fish Data")]
    [SerializeField] public PondFishData fishData;
    private float weight = 0;

    private bool newStateTransition = true;
    private Camera cam;

    private Animator animator;

    private void Awake()
    {
        boxingGloves.SetActive(false);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    public void setFish(PondFishData data)
    {
        fishData = data;
        var model = Resources.Load<Mesh>(fishData.modelPath);
        var material = Resources.Load<Material>(fishData.modelPath);
        if (model != null && material != null)
        {
            gameObject.GetComponentInChildren<MeshFilter>().mesh = model;
            gameObject.GetComponentInChildren<MeshRenderer>().material = material;
        }
    }

    private void Update()
    {
        switch (fishState)
        {
            case FishState.INTRO:
                if (newStateTransition)
                {
                    newStateTransition = false;
                    StartCoroutine(LerpRotation(0, -90f, 4f, FishState.COMBAT));
                    audioSource.clip = clipList[0];
                    audioSource.Play();
                }

                FaceCamera(lerpedRotationY);
                break;
            case FishState.COMBAT:
                if (newStateTransition)
                {
                    newStateTransition = false;
                    GameManager.instance.clashBar.gameObject.SetActive(true);
                    fightStartSource.Play();
                    boxingGloves.SetActive(true);
                    StartCoroutine(LerpRotation(-90, 0f, 0.5f, FishState.NONE));

                    audioSource.clip = clipList[1];
                    animator.SetBool("Combat", true);
                }

                CombatUpdate();
                FaceCamera(lerpedRotationY);
                break;
            case FishState.END:
                if (newStateTransition)
                {
                    newStateTransition = false;
                    GameManager.instance.clashBar.gameObject.SetActive(false);
                    GameManager.instance.fishDataUI.SetActive(true);
                    weight = UnityEngine.Random.Range(fishData.minWeight, fishData.maxWeight);
                    GameManager.instance.SetFishDataUI(weight);

                    boxingGloves.SetActive(false);
                    StartCoroutine(LerpRotation(0, -90f, 2f, FishState.NONE));
                    audioSource.clip = clipList[2];
                    audioSource.Play();

                    GameManager.instance.setState(1);
                    animator.SetBool("Combat", false);
                }

                EndUpdate();
                FaceCamera(lerpedRotationY);

                endTimer += Time.deltaTime;
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
        if (GameManager.instance.clashBar.value >= GameManager.instance.clashBar.maxValue)
        {
            newStateTransition = true;
            fishState = FishState.END;
            return;
        }

        GameManager.instance.clashBar.value -= fishData.defense * Time.deltaTime;

        if (GameManager.instance.touchPressAction.WasPerformedThisFrame())
        {
            GameManager.instance.clashBar.value += fishData.attack;
            audioSource.Play();
            //print("wabam!");
        }
    }

    void EndUpdate()
    {
        if (GameManager.instance.touchPressAction.WasPerformedThisFrame() && endTimer >= victoryLength)
        {
            AquariumFishData data = new AquariumFishData();
            data.name = fishData.name;
            data.description = fishData.description;
            data.modelPath = fishData.modelPath;
            data.weight = weight;

            GameManager.instance.EndFight(data);
        }
    }
}
