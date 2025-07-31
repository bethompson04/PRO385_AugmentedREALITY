using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScreen : MonoBehaviour
{
    float rotationSpeed = 15f;
    float zoomDuration = 0.5f;

    new Camera camera;

    private bool pressed = false;
    
    void Start()
    {
        camera = Camera.main;
        GameManager.instance.touchPressAction = GameManager.instance.playerInput.actions["TouchPress"];
        StartCoroutine(Zoom(false));
    }

    private void FixedUpdate()
    {
        Vector3 relativePos = camera.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        transform.position = camera.transform.position + camera.transform.forward * 2;

        if (GameManager.instance.touchPressAction.WasPerformedThisFrame() && pressed == false)
        {
            pressed = true;
            StartCoroutine(Zoom(true));
        }
    }

    IEnumerator Zoom(bool zoomOut)
    {
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.zero;
        float time = 0f;

        if (zoomOut)
        {
            startScale = new Vector3(0.1f, 0.1f, 0.1f);
        } else
        {
            endScale = new Vector3(0.1f, 0.1f, 0.1f);
        }

        while (time < zoomDuration)
        {
            float t = time / zoomDuration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
        if (zoomOut)
        {
            GameManager.instance.setState(1);
            Destroy(gameObject);
        }
    }
}
