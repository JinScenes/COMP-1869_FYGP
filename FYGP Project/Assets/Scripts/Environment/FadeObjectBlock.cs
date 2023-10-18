using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjectBlock : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform target;
    [SerializeField] private Camera cam;
    [SerializeField] private float fadedAlpha = 0.33f;
    [SerializeField] private bool retainShadows = true;
    [SerializeField] private Vector3 targetPositionOffset = Vector3.up;
    [SerializeField] private float fadeSpeed = 1;

    [Header("Read Only Data")]
    [SerializeField]
    private List<Fading> objectsBlockingView = new List<Fading>();
    private Dictionary<Fading, Coroutine> runningCoroutines = new Dictionary<Fading, Coroutine>();

    private RaycastHit[] hits = new RaycastHit[10];

    private void Start()
    {
        StartCoroutine(CheckForObjects());
    }

    private IEnumerator CheckForObjects()
    {
        while (true)
        {
            int p_hits = Physics.RaycastNonAlloc(cam.transform.position, (target.transform.position + targetPositionOffset 
                - cam.transform.position).normalized, hits, Vector3.Distance(cam.transform.position, 
                target.transform.position + targetPositionOffset), layerMask);

            if (p_hits > 0)
            {
                for (int i = 0; i < p_hits; i++)
                {
                    Fading fadingObj = GetFadingObjectFromHit(hits[i]);

                    if (fadingObj != null && !objectsBlockingView.Contains(fadingObj))
                    {
                        if (runningCoroutines.ContainsKey(fadingObj))
                        {
                            if (runningCoroutines[fadingObj] != null)
                            {
                                StopCoroutine(runningCoroutines[fadingObj]);
                            }

                            runningCoroutines.Remove(fadingObj);
                        }

                        runningCoroutines.Add(fadingObj, StartCoroutine(FadeObjectOut(fadingObj)));
                        objectsBlockingView.Add(fadingObj);
                    }
                }
            }

            FadeObjectsNoLongerBeingHit();

            ClearHits();

            yield return null;
        }
    }

    private void ClearHits()
    {
        System.Array.Clear(hits, 0, hits.Length);
    }

    private void FadeObjectsNoLongerBeingHit()
    {
        List<Fading> objectsToRemove = new List<Fading>(objectsBlockingView.Count);

        foreach(Fading fadingObj in objectsBlockingView)
        {
            bool objectsIsBeingHit = false;
            for (int i = 0; i < hits.Length; i++)
            {
                Fading hitFadingObject = GetFadingObjectFromHit(hits[i]);
                if (hitFadingObject != null && fadingObj == hitFadingObject)
                {
                    objectsIsBeingHit = true;
                    break;
                }
            }

            if (!objectsIsBeingHit)
            {
                if (runningCoroutines.ContainsKey(fadingObj))
                {
                    if (runningCoroutines[fadingObj] != null)
                    {
                        StopCoroutine(runningCoroutines[fadingObj]);
                    }

                    runningCoroutines.Remove(fadingObj);
                }

                runningCoroutines.Add(fadingObj, StartCoroutine(FadeObjectIn(fadingObj)));
                objectsToRemove.Add(fadingObj);
            }
        }

        foreach(Fading removeObj in objectsToRemove)
        {
            objectsBlockingView.Remove(removeObj);
        }
    }

    private IEnumerator FadeObjectOut(Fading fadingObj)
    {
        foreach(Material material in fadingObj.materials)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.SetInt("_Surface", 1);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", retainShadows);

            material.SetOverrideTag("RenderType", "Transparent");

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        }

        float time = 0;

        while (fadingObj.materials[0].color.a > fadedAlpha)
        {
            foreach (Material material in fadingObj.materials)
            {
                if (material.HasProperty("_BaseColor"))
                {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        Mathf.Lerp(fadingObj.initialAlpha, fadedAlpha, time * fadeSpeed)
                    );
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        if (runningCoroutines.ContainsKey(fadingObj))
        {
            StopCoroutine(runningCoroutines[fadingObj]);
            runningCoroutines.Remove(fadingObj);
        }
    }

    private IEnumerator FadeObjectIn(Fading fadingObj)
    {
        float time = 0;

        while (fadingObj.materials[0].color.a < fadingObj.initialAlpha)
        {
            foreach (Material material in fadingObj.materials)
            {
                if (material.HasProperty("_BaseColor"))
                {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        Mathf.Lerp(fadedAlpha, fadingObj.initialAlpha, time * fadeSpeed)
                    );
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        foreach (Material material in fadingObj.materials)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.SetInt("_Surface", 0);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            material.SetShaderPassEnabled("DepthOnly", true);
            material.SetShaderPassEnabled("SHADOWCASTER", true);

            material.SetOverrideTag("RenderType", "Opaque");

            material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        }

        if (runningCoroutines.ContainsKey(fadingObj))
        {
            StopCoroutine(runningCoroutines[fadingObj]);
            runningCoroutines.Remove(fadingObj);
        }
    }

    private Fading GetFadingObjectFromHit(RaycastHit hit)
    {
        return hit.collider != null ? hit.collider.GetComponent<Fading>() : null;
    }
}
