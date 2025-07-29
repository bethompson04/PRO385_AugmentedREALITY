using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class Ripple : MonoBehaviour {
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

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            if (t >= 1f) {
                isLerping = false;
                Invoke(nameof(PickNewTarget), pauseTime);
            }
        }
    }

    void PickNewTarget() {
        Vector2 randomPoint2D = Random.insideUnitCircle * radius;
        startPosition = transform.position;
        targetPosition = new Vector3(center.x + randomPoint2D.x, startPosition.y, center.z + randomPoint2D.y);

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

