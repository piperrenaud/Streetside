using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -6);
    [SerializeField] private float smoothSpeed = 8f;

    private Quaternion fixedRotation;

    private void Start()
    {
        fixedRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime);

        transform.rotation = fixedRotation;
    }
}
