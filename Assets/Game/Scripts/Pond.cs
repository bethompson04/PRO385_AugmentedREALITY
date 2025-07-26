using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PondManager : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  [SerializeField] GameObject ripplePrefab;
  [SerializeField] float rippleTimer = 10.0f;
  [SerializeField] Animator animator;

  private GameObject currentRipple;
  private float rippleSpawnTimer;
  //public Rarity rippleRarity

  void Start()
    {
    InteractManager.GetInstance().PondSpawned();
    rippleSpawnTimer = rippleTimer;
    //animator.Play("PondAppear");
    //StartCoroutine(SpawnNewRipple(5.0f));
    }

    // Update is called once per frame
    void Update()
    {
      if (rippleSpawnTimer <= 0)
      {
      //StartCoroutine(SpawnNewRipple(1.0f));
      }
      rippleSpawnTimer -= Time.deltaTime;
    }
    




  public void DestroyPond(){
    // Raise Event for leaving Pond state, going back to Creation state
    animator.Play("PondDisappear");
  }

  public void DeletePond() {
    Destroy(gameObject);
  }
    IEnumerator SpawnNewRipple(float waitTime) 
     {
      Debug.Log("Spawning new ripple...");
      yield return new WaitForSeconds(waitTime);
      Debug.Log("Spawned New Ripple!");
      Destroy(currentRipple);
      currentRipple = Instantiate(ripplePrefab);
      rippleSpawnTimer = rippleTimer;
      yield break;
     }
}
