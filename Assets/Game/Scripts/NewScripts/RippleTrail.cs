using UnityEngine;
using UnityEngine.InputSystem;

public class RippleTrail : MonoBehaviour
{
    [SerializeField] GameObject ripplePrefab;
    [SerializeField] float rippleSpawnRate;

    private float rippleSpawnTimer = 0.0f;

    void Start()
    {
        rippleSpawnTimer = rippleSpawnRate;
    }

    void Update()
    {
        if (rippleSpawnTimer <= 0.0f)
        {
            rippleSpawnTimer = rippleSpawnRate;
            Vector3 flatRotation = new Vector3(-90f, 0f, 0f);
            Instantiate(ripplePrefab, new Vector3(
                                        transform.position.x,
                                        transform.position.y + 0.11f,
                                        transform.position.z
                                        ),
                                        Quaternion.Euler(flatRotation)
                                        );
        }
        rippleSpawnTimer -= Time.deltaTime;
    }
}
