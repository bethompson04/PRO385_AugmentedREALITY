using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class Ripple : MonoBehaviour {
    [SerializeField] AnimationCurve easeCurve;

	private float radius = 0.15f;       // Radius of the circle
    private float lerpDuration = 2f;
    private float pauseTime = 1f;    // Time to pause at each point

    private Vector3 center;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float lerpTime;
    private bool isLerping = false;

    void Start() {
        center = transform.position;
        PickNewTarget();
    }

    void Update() {
        if (isLerping) {
            lerpTime += Time.deltaTime;
            float t = Mathf.Clamp01(lerpTime / lerpDuration);
            float easedT = easeCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPosition, targetPosition, easedT);

            if (t >= 1f) {
                isLerping = false;
                transform.position += transform.forward * 0.01f * Time.deltaTime;
                Invoke(nameof(PickNewTarget), pauseTime);
            }
        } else
        {

            transform.position += transform.forward * 0.01f * Time.deltaTime;
        }
    }

    void PickNewTarget() {
        Vector2 randomPoint2D = Random.insideUnitCircle * radius;
        startPosition = transform.position;
        targetPosition = new Vector3(center.x + randomPoint2D.x, startPosition.y, center.z + randomPoint2D.y);
        transform.LookAt(targetPosition);

        lerpTime = 0f;
        isLerping = true;
    }

    // These are for the animations of them disappearing
    public void DestroyRipple() {
    //animator.Play("RippleDisappear");
    }

    public void DeleteRipple() {
        Destroy(gameObject);
    }
}

