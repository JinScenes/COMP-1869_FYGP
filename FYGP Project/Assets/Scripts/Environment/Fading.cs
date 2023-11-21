using System;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour, IEquatable<Fading>
{
    public List<Renderer> Renderers = new List<Renderer>();
    public List<Material> materials = new List<Material>();

    public Vector3 pos;
    [HideInInspector] public float initialAlpha;

    private void Awake()
    {
        pos = transform.position;

        if (Renderers.Count == 0)
        {
            Renderers.AddRange(GetComponentsInChildren<Renderer>());
        }

        foreach(Renderer renderer in Renderers)
        {
            materials.AddRange(renderer.materials);
        }

        initialAlpha = materials[0].color.a;
    }

    public bool Equals(Fading other)
    {
        if (other == null)
            return false;

        return pos == other.pos;
    }


    public override int GetHashCode()
    {
        return pos.GetHashCode();
    }

}
