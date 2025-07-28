using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class AquariumFish : MonoBehaviour
{
    public Vector3 nextLocation;
    public float timer;

    public AquariumFishData data;

    public void setFish(AquariumFishData data)
    {
        this.data = data;
        gameObject.transform.localScale = new Vector3(this.data.weight, this.data.weight, this.data.weight);
    }
}
