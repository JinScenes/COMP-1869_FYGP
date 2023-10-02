using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Variables")]
    public Transform playerTransform;

    [Header("Camera Stats")]
    [Range(0, 10)] public float lerpSpeed;
    [Range(0, 1)] public float smoothZoomTime;
    [Range(0, 10)] public float targetOrthographicSize;

    private float currentVelocityZoom = 0f;
    public Vector3 cameraOffset;
    public Vector3 rotationAngles;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        if (playerTransform == null)
        {
            playerTransform = GameObject.Find("Player").transform;
        }
    }

    private void LateUpdate()
    {
        FollowPlayer();
        Zooming();
    }

    private void FollowPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 desiredPos = playerTransform.position + cameraOffset;
            desiredPos.z = transform.position.z;

            transform.position = Vector3.Lerp(transform.position, desiredPos, lerpSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogError("The camera requires a player gameobject to follow");
        }
    }

    private void Zooming()
    {
        float newOrthoSize = Mathf.SmoothDamp(Camera.main.orthographicSize, 
            targetOrthographicSize, ref currentVelocityZoom, smoothZoomTime);

        cam.orthographicSize = newOrthoSize;
    }
}
