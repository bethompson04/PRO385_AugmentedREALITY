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

    private void FixedUpdate()
    {
        foreach(var fish in fishList)
        {
            AquariumFish f = fish.GetComponent<AquariumFish>();
            if (f.nextLocation == null || Vector3.Distance(fish.transform.position, f.nextLocation) <= 0.25f)
            {
                f.nextLocation = new Vector3(
                    Random.Range(transform.position.x - fishRange.x, transform.position.x + fishRange.x), 
                    Random.Range(transform.position.y - fishRange.y, transform.position.y + fishRange.y), 
                    Random.Range(transform.position.z - fishRange.z, transform.position.z + fishRange.z)
                );
                f.timer = 0;
            }
            f.timer += fishSpeedWeight * Time.deltaTime / f.data.weight;
            fish.transform.position = Vector3.Slerp(fish.transform.position, f.nextLocation, f.timer);
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
        aquariumDataList.list.Add(data);
    }

    public void SpawnFish()
    {
        fishList = new List<GameObject>();
        StartCoroutine("SpawnFishCoroutine");
    }

    public void DeleteFish()
    {
        for (int i = fishList.Count; i > 0; i--)
        {
            Destroy(fishList[i]);
        }
        fishList = new List<GameObject>();
    }

    IEnumerator SpawnFishCoroutine()
    {
        foreach (var data in aquariumDataList.list)
        {
            yield return new WaitForSeconds(2);
            GameObject fish = Instantiate(fishPrefab, gameObject.transform);
            fish.GetComponent<AquariumFish>().setFish(data);
            fish.GetComponent<AquariumFish>().nextLocation = new Vector3(
                    Random.Range(transform.position.x - fishRange.x, transform.position.x + fishRange.x),
                    Random.Range(transform.position.y - fishRange.y, transform.position.y + fishRange.y),
                    Random.Range(transform.position.z - fishRange.z, transform.position.z + fishRange.z)
                );
            fishList.Add(fish);
        }

        yield return null;
    }
}
