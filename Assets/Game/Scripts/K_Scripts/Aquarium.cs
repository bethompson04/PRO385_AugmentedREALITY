using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AquariumFishData
{
    public string name;
    public string description;
    public string modelPath;
    public float weight;
}

public class Aquarium : MonoBehaviour
{
    [SerializeField] DataList<AquariumFishData> aquariumDataList;
    [SerializeField] List<GameObject> fishList;

    [SerializeField][Range(0, 1)] float fishSpeedWeight;
    [SerializeField] GameObject fishPrefab;
    [SerializeField] Vector3 fishRange = Vector3.one;
    [SerializeField] Vector3 tankRotation = Vector3.zero;
	[SerializeField] float forwardDistance = 0.5f;
	[SerializeField] float lerpPositionDamping = 5f;
	[SerializeField] GameObject viewButton;
    [SerializeField] GameObject model;

    private bool moveToInFrontCamera = false;

    private void Start()
    {
        aquariumDataList = new DataList<AquariumFishData>();
    }

    private void FixedUpdate()
    {
        foreach(var fish in fishList)
        {
            AquariumFish f = fish.GetComponent<AquariumFish>();
            if (f.nextLocation == null || Vector3.Distance(fish.transform.localPosition, f.nextLocation) <= 0.25f)
            {
                f.nextLocation = new Vector3(
                    Random.Range(-fishRange.x, fishRange.x), 
                    Random.Range(-fishRange.y, fishRange.y), 
                    Random.Range(-fishRange.z, fishRange.z)
                );
                f.timer = 0;
            }
            f.timer += fishSpeedWeight * Time.deltaTime / f.data.weight;
            fish.transform.localPosition = Vector3.Slerp(fish.transform.localPosition, f.nextLocation, f.timer);
        }

        transform.rotation *= Quaternion.Euler(tankRotation.x, tankRotation.y, tankRotation.z);
    }

	private void Update()
	{
        if (moveToInFrontCamera)
        {
            HoverInFrontCamera();
        }
	}

	public void LoadAquarium()
    {
        aquariumDataList = SystemIO.LoadFile<AquariumFishData>("aquarium_data");
    }

    public void SaveAquarium()
    {
        SystemIO.SaveFile(aquariumDataList, "aquarium_data");
    }

    public void AddFish(AquariumFishData data)
    {
        Debug.Log(data.name);
        aquariumDataList.list.Add(data);
        Debug.Log("Fish added to aquarium");
    }

    public void SpawnFish()
    {
        fishList = new List<GameObject>();
        StartCoroutine("SpawnFishCoroutine");
    }

    public void DeleteFish()
    {
        StopCoroutine("SpawnFishCoroutine");
        for (int i = fishList.Count-1; i >= 0; i--)
        {
            Destroy(fishList[i]);
        }
        fishList = new List<GameObject>();
        //Destroy(gameObject);
    }

    IEnumerator SpawnFishCoroutine()
    {
        foreach (var data in aquariumDataList.list)
        {
            yield return new WaitForSeconds(2);
            GameObject fish = Instantiate(fishPrefab, gameObject.transform);
            fish.GetComponent<AquariumFish>().setFish(data);
            fish.GetComponent<AquariumFish>().nextLocation = new Vector3(
                    Random.Range(-fishRange.x, fishRange.x),
                    Random.Range(-fishRange.y, fishRange.y),
                    Random.Range(-fishRange.z, fishRange.z)
                );
            fishList.Add(fish);
        }

        yield return null;
    }

	private void HoverInFrontCamera()
	{
		Vector3 camPos = Camera.main.transform.position;
        //camPos = new Vector3(camPos.x, camPos.y + 0.5f, camPos.z);
		Vector3 ourPos = gameObject.transform.position;

		gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, camPos + Camera.main.transform.forward * forwardDistance, Time.deltaTime * lerpPositionDamping);
	}

    public void SetHover()
    {
        if (model.activeSelf == false)
        {
            model.SetActive(true);
            SpawnFish();
            GameManager.instance.setState(3);
            GameManager.instance.objectSpawner.SetActive(false);

            moveToInFrontCamera = true;
        } 
        else
        {
            model.SetActive(false);
            DeleteFish();
            GameManager.instance.setState(1);
            GameManager.instance.objectSpawner.SetActive(true);

            moveToInFrontCamera = false;
        }
        
    }
}
