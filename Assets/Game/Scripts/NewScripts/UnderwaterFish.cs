using UnityEngine;
using UnityEngine.InputSystem;

public class UnderwaterFish : MonoBehaviour
{
    [SerializeField] AnimationCurve easeCurve;

    private float radius = 0.15f;   // Radius of the circle
    private float lerpDuration = 2f;
    private float pauseTime = 1f;   // Time to pause at each point

    private Vector3 center;
    private Vector3 startPosition;
    private Vector3 endPosition;

    private float lerpTime;
    private bool isLerping = false;

    private void Start()
    {

        center = transform.position;
        PickNewTarget();
    }

    private void Update()
    {
        if (GameManager.instance.touchPressAction.WasPerformedThisFrame() && GameManager.instance.gameState == GameManager.GameState.POND)
        {
            HandleTap(GameManager.instance.touchPositionAction.ReadValue<Vector2>());
        }
    }

    private void FixedUpdate()
    {
        if (isLerping)
        {
            lerpTime += Time.deltaTime;
            float t = Mathf.Clamp01(lerpTime / lerpDuration);
            float easedT = easeCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPosition, endPosition, easedT);

            if (t >= 1f)
            {
                isLerping = false;
                transform.position += transform.forward * 0.01f * Time.deltaTime;
                Invoke(nameof(PickNewTarget), pauseTime);
            }
        }
        else
        {
            transform.position += transform.forward * 0.01f * Time.deltaTime;
        }
    }

    void PickNewTarget()
    {
        Vector2 randomPoint2D = Random.insideUnitCircle * radius;
        startPosition = transform.position;
        endPosition = new Vector3(center.x + randomPoint2D.x, startPosition.y, center.z + randomPoint2D.y);
        transform.LookAt(endPosition);

        lerpTime = 0f;
        isLerping = true;
    }

    void HandleTap(Vector2 screenPosition)
    {
        int layerMask = ~LayerMask.GetMask("Pond");

        Debug.Log("tap");
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            Debug.Log("Tapped on: " + hit.collider.gameObject.name);

            if (hit.collider.CompareTag("Ripple"))
            {
                GameManager.instance.StartFight();
            }
        }
    }
}
