using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        aquariumDataList.list.Add(data);
    }

    public void SpawnFish()
    {
        fishList = new List<GameObject>();
        StartCoroutine("SpawnFishCoroutine");
    }

    public void DeleteAquarium()
    {
        StopCoroutine("SpawnFishCoroutine");
        for (int i = fishList.Count-1; i >= 0; i--)
        {
            Destroy(fishList[i]);
        }
        Destroy(gameObject);
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
}
