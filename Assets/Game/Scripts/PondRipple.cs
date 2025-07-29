using UnityEngine;

public class PondRipple : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Renderer renderer;
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update 60 fps
    void FixedUpdate()
    {
        if (renderer.material.color.a > 0) {
            Color newColor = renderer.material.color;
            newColor.a -= 0.01f;
            renderer.material.color = newColor;
        }
    }

    void DestroyPondRipple() {
        Destroy(gameObject);
    }
}
