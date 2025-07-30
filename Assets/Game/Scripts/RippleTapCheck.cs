using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation; // For AR specific components if needed

public class RippleTapCheck : MonoBehaviour {

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    private void Awake() {
		playerInput = GetComponent<PlayerInput>();
		touchPressAction = playerInput.actions["TouchPress"];
		touchPositionAction = playerInput.actions["TouchPosition"];
	}

	void Update() {
        // Check for touch input (for mobile devices)
        if (touchPressAction.WasPerformedThisFrame() && GameManager.instance.state != GameManager.GameState.FIGHT) {
            Vector2 screenPosition = touchPositionAction.ReadValue<Vector2>();
            HandleTap(screenPosition);
        }
        // Also consider mouse input for editor testing (optional)
        //else if (Input.GetMouseButtonDown(0)) {
        //    HandleTap(Input.mousePosition);
        //}
    }

    void HandleTap(Vector2 screenPosition) {
		int layerMask = ~LayerMask.GetMask("Pond"); //invert mask to exclude Pond layer

		Debug.Log("tap");
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, layerMask)) {
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