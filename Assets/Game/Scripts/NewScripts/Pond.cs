using System.Collections;
using UnityEngine;

public class Pond : MonoBehaviour
{
    [SerializeField] GameObject pondFishPrefab;
    [SerializeField] Animator animator;

    private GameObject underwaterFish;
    
    void Start()
    {
        GameManager.instance.setPondSpawned();
        StartCoroutine("SpawnUnderwaterFish");
    }

    IEnumerator SpawnUnderwaterFish()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Spawned new ripple!");

        if (underwaterFish == null)
        {
            underwaterFish = Instantiate(pondFishPrefab, gameObject.transform);
            underwaterFish.transform.position = new Vector3 (
                                                underwaterFish.transform.position.x,
                                                underwaterFish.transform.position.y - 0.1f,
                                                underwaterFish.transform.position.z
                                                );
        }
        
        yield break;
    }
}
