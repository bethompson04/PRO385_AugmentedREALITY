using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Title : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float zoomDuration = 1;

    [SerializeField] PlayerInput playerInput;
    private InputAction touchPressAction;

    private bool pressed = false;

    private void Start()
    {
        touchPressAction = playerInput.actions["TouchPress"];
        StartCoroutine("ZoomIn");
    }

    private void FixedUpdate()
    {
        Vector3 relativePos = Camera.main.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2;

        if (touchPressAction.WasPerformedThisFrame() && pressed == false)
        {
            pressed = true;
            StartCoroutine("ZoomOut");
        }
    }

    IEnumerator ZoomIn()
    {
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = new Vector3(0.1f, 0.1f, 0.1f);
        float time = 0f;

        while (time < zoomDuration)
        {
            float t = time / zoomDuration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
    }

    IEnumerator ZoomOut()
    {
        Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f);
        Vector3 endScale = Vector3.zero;
        float time = 0;

        while (time < zoomDuration)
        {
            float t = time / zoomDuration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
        Destroy(gameObject);
    }
}
