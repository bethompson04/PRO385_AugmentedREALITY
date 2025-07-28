using System.Collections;
using UnityEngine;

public class Ripple : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  [SerializeField] float rippleSize = 0.05f;
  [SerializeField] float rippleSpeed = 1.0f;
  [SerializeField] Animator animator;

  private Vector3 rippleTempScale;
  private float rippleIntroRate;
  private Vector3 rippleDestination;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public void NewDestination() {
    rippleDestination = Random.insideUnitCircle * 0.05f;
  }

  // These are for the animations of them disappearing
  public void DestroyRipple() {
    //animator.Play("RippleDisappear");
  }

  public void DeleteRipple() {
    Destroy(gameObject);
  }
}

