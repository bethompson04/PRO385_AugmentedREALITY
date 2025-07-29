using UnityEngine;

public class Ripple1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject ripplePrefab;
    [SerializeField] float rippleRate;

    private float rippleTimer = 0.0f;
    void Start()
    {
        rippleTimer = rippleRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(rippleTimer <= 0.0f) {
            rippleTimer = rippleRate;
            Instantiate(ripplePrefab, new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z), transform.rotation);
        }
        rippleTimer -= Time.deltaTime;
    }
}
