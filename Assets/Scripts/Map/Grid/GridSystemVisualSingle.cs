using System;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void Show(Material givenMaterial)
    {
        meshRenderer.material = givenMaterial;
        meshRenderer.enabled = true;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }
}
