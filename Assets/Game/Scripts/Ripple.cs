using System.Collections;
using UnityEngine;

public class Ripple : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  [SerializeField] float rippleSize = 0.05f;
  [SerializeField] float rippleSpeed = 1.0f;

  private Vector3 rippleTempScale;
  private float rippleIntroRate;
    void Start()
    {
    rippleIntroRate = rippleSize / 10;
    gameObject.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  //  IEnumerator SlowAppear() {
  //  rippleTempScale = gameObject.transform.localScale;
  //  gameObject.transform.localScale = new Vector3(rippleTempScale.x + rippleIntroRate, rippleTempScale.y + rippleIntroRate, rippleTempScale.z + rippleIntroRate);

  //  if (gameObject.transform.localScale.x >= rippleSize) {
  //    yield break;
  //  }else {
  //    yield return new WaitForSeconds(rippleIntroRate);
  //  }
  //}
}

