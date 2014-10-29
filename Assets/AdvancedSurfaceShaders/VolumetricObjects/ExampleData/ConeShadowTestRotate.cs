using UnityEngine;

public class ConeShadowTestRotate : MonoBehaviour
{
    public float rotationSpeed = 1.0F;

    private void Update()
    {
        transform.Rotate(0f, Time.deltaTime * rotationSpeed, 0f);
    }
}
