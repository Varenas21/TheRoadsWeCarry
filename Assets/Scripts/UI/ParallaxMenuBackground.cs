using UnityEngine;

public class ParallaxMenuBackground : MonoBehaviour
{
    public Transform[] layers;
    public float[] speeds;

    private void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].Rotate(Vector3.up, speeds[i] * Time.deltaTime, Space.World);
        }
    }
}
