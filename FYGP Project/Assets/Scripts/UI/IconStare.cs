using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconStare : MonoBehaviour
{
    [SerializeField] private Vector3 localDownwardAngle;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z);
        transform.LookAt(targetPosition);
        transform.Rotate(localDownwardAngle, Space.Self);
    }
}
