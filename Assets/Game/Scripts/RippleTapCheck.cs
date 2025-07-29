using UnityEngine;
using UnityEngine.XR.ARFoundation; // For AR specific components if needed

public class RippleTapCheck : MonoBehaviour {
    void Update() {
        // Check for touch input (for mobile devices)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            Touch touch = Input.GetTouch(0);
            HandleTap(touch.position);
        }
        // Also consider mouse input for editor testing (optional)
        //else if (Input.GetMouseButtonDown(0)) {
        //    HandleTap(Input.mousePosition);
        //}
    }

    void HandleTap(Vector2 screenPosition) {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            // A collider was hit
            Debug.Log("Tapped on: " + hit.collider.gameObject.name);

            // Example: Check if the tapped object has a specific tag
            if (hit.collider.CompareTag("Ripple")) {
                InteractManager.GetInstance().RippleClikced();
                // Add your custom logic here for when the object is tapped
            }
        }
    }
}