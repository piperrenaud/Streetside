using System;
using UnityEngine;

public class PlayerStencilMask : MonoBehaviour
{
    [SerializeField] private Material maskMaterial;

    private Renderer[] characterRenderers;

    private void Awake()
    {
        characterRenderers = GetComponentsInChildren<Renderer>();
    }

    private void LateUpdate()
    {
        if (maskMaterial == null) return;

        for (int i = 0; i < characterRenderers.Length; i++)
        {
            Renderer rend = characterRenderers[i];

            if (rend != null && rend.enabled && rend.gameObject.activeInHierarchy)
            {
                Graphics.DrawMesh(
                    GetMeshFromRenderer(rend),
                    rend.transform.localToWorldMatrix,
                    maskMaterial,
                    rend.gameObject.layer
                    );
            }
        }
    }

    private Mesh GetMeshFromRenderer(Renderer rend)
    {
        if (rend is SkinnedMeshRenderer skinnedRend)
        {
            Mesh animatedMesh = new Mesh();
            skinnedRend.BakeMesh(animatedMesh);
            return animatedMesh;
        }

        return null;
    }
}
