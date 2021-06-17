using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private float depth = 10;

    private Transform cameraTransform;
    private Vector3 origin;
    private Vector3 cameraOrigin;

    private void Awake()
    {
        origin = transform.position;
        cameraTransform = Camera.main.transform;
        cameraOrigin = cameraTransform.position;
    }

    private void Update()
    {
        var target = origin + (cameraTransform.position - origin) * (1 / depth);
        target.z = origin.z;
        transform.position = target;
    }
}
