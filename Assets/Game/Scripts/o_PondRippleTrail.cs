using Unity.Hierarchy;
using UnityEngine;

public class o_PondRippleTrail : MonoBehaviour
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
            Vector3 flatRotation = new Vector3(-90f, 0f, 0f);
            Instantiate(ripplePrefab, new Vector3(transform.position.x, transform.position.y + 0.11f, transform.position.z), Quaternion.Euler(flatRotation));
        }
        rippleTimer -= Time.deltaTime;
    }
}
