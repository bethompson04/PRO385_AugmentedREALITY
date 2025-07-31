using System.Collections;
using UnityEngine;

public class o_PondManager : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject ripplePrefab;
    [SerializeField] float rippleTimer = 60.0f;
    [SerializeField] Animator animator;

    private GameObject currentRipple;
    private float rippleSpawnTimer;
    //public Rarity rippleRarity

    void Start() {
        o_InteractManager.GetInstance().PondSpawned();
        rippleSpawnTimer = rippleTimer;
        StartCoroutine(SpawnNewRipple(5.0f));   
    }

    // Update is called once per frame
    void Update() {
        if (rippleSpawnTimer <= 0) {
            Debug.Log("Kill Ripple");
            Destroy(currentRipple);
            StartCoroutine(SpawnNewRipple(1.0f));
        } else {
            rippleSpawnTimer -= Time.deltaTime;
        }
    }

    public void DestroyPond() {
        // Raise Event for leaving Pond state, going back to Creation state
        animator.Play("PondDisappear");
    }

    public void DeletePond() {
        Destroy(gameObject);
    }
    IEnumerator SpawnNewRipple(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Spawned New Ripple!");
        
        if (currentRipple == null)
        {
			currentRipple = Instantiate(ripplePrefab, gameObject.transform);
            currentRipple.transform.position = new Vector3(currentRipple.transform.position.x, currentRipple.transform.position.y - 0.1f, currentRipple.transform.position.z);
		}

        rippleSpawnTimer = rippleTimer;
        yield break;
    }
}
