using UnityEngine;

public class AquariumFish : MonoBehaviour
{
    public Vector3 nextLocation;
    public float timer;

    public AquariumFishData data;

    public void setFish(AquariumFishData data)
    {
        this.data = data;
        gameObject.transform.localScale = new Vector3(this.data.weight * 0.003f, this.data.weight * 0.003f, this.data.weight * 0.003f);
        var model = Resources.Load<Mesh>(data.modelPath);
        var material = Resources.Load<Material>(data.modelPath);
        if (model != null && material != null)
        {
            gameObject.GetComponent<MeshFilter>().mesh = model;
            gameObject.GetComponent<MeshRenderer>().material = material;
        }
    }

    private void FixedUpdate()
    {
        Vector3 newDir = Vector3.RotateTowards(transform.forward, nextLocation, 5 * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
