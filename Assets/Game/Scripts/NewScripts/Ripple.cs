using UnityEngine;

public class Ripple : MonoBehaviour
{
    new private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        if (renderer.material.color.a > 0)
        {
            Color newColor = renderer.material.color;
            newColor.a -= 0.01f;
            renderer.material.color = newColor;
        } else
        {
            Destroy(gameObject);
        }
    }
}
