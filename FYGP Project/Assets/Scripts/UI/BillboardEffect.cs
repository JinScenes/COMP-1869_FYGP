using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardEffect : MonoBehaviour
{
    private Transform cam;

    private void Start()
    {
<<<<<<< HEAD
        if (cam == null)
            cam = FindObjectOfType<Camera>().transform;
=======
        cam = Camera.main.transform;
>>>>>>> BranchMerger
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
