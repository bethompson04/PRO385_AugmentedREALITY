using UnityEngine;

public class PondManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    InteractManager.GetInstance().PondSpawned();
      // Raise Event for going into Pond state
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  private void OnDestroy() {
    // Raise Event for leaving Pond state, going back to Creation state
    InteractManager.GetInstance().PondDestroyed();
  }
}
