using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private Transform playerOne;
    [SerializeField] private Transform playerTwo;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask wallLayer;
    
    private Vector3 p1TargetPos;
    private Vector3 p2TargetPos;
    private List<Vector3> p1HitPoints = new List<Vector3>();
    private List<Vector3> p2HitPoints = new List<Vector3>();

    private HashSet<Renderer> activeWallRenderers = new HashSet<Renderer>();
    private HashSet<Renderer> visibleThisFrame = new HashSet<Renderer>();
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!mainCamera) return;

        visibleThisFrame.Clear();
        p1HitPoints.Clear();
        p2HitPoints.Clear();

        ProcessPlayerCutout(playerOne, "_CutoutTargetWS1");
        ProcessPlayerCutout(playerTwo, "_CutoutTargetWS2");

        foreach (Renderer wall in activeWallRenderers)
        {
            if (!visibleThisFrame.Contains(wall) && wall != null)
            {
                wall.material.SetVector("_CutoutTargetWS1", Vector4.zero);
                wall.material.SetVector("_CutoutTargetWS2", Vector4.zero);
            }
        }

        activeWallRenderers.RemoveWhere(w => !visibleThisFrame.Contains(w));
        foreach (Renderer w in visibleThisFrame) activeWallRenderers.Add(w);
    }

    private void ProcessPlayerCutout(Transform player, string shaderTargetKey)
    {
        if (!player) return;

        Vector3 targetPos = player.position + Vector3.up;
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(targetPos);
        Ray orthoRay = mainCamera.ViewportPointToRay(viewportPoint);
        float distance = Vector3.Distance(orthoRay.origin, targetPos);

        RaycastHit[] hits = Physics.RaycastAll(orthoRay.origin, orthoRay.direction, distance, wallLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            Renderer wallRenderer = hit.collider.GetComponent<Renderer>();

            if (wallRenderer)
            {
                visibleThisFrame.Add(wallRenderer);

                Vector4 shaderPassVector = new Vector4(hit.point.x, hit.point.y, hit.point.z, 1.0f);
                wallRenderer.material.SetVector(shaderTargetKey, shaderPassVector);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (mainCamera == null) return;

        // --- DRAW PLAYER 1 PARALLEL PATH ---
        if (playerOne != null)
        {
            Vector3 p1Target = playerOne.position + Vector3.up;
            Vector3 vPoint = mainCamera.WorldToViewportPoint(p1Target);
            Ray ray = mainCamera.ViewportPointToRay(vPoint);
        
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(ray.origin, p1Target); // Draws the line straight from the orthographic plane

            Gizmos.color = Color.blue;
            foreach (Vector3 hitPoint in p1HitPoints)
            {
                Gizmos.DrawSphere(hitPoint, 0.25f);
            }
        }

        // --- DRAW PLAYER 2 PARALLEL PATH ---
        if (playerTwo != null)
        {
            Vector3 p2Target = playerTwo.position + Vector3.up;
            Vector3 vPoint = mainCamera.WorldToViewportPoint(p2Target);
            Ray ray = mainCamera.ViewportPointToRay(vPoint);
        
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(ray.origin, p2Target);

            Gizmos.color = Color.red;
            foreach (Vector3 hitPoint in p2HitPoints)
            {
                Gizmos.DrawSphere(hitPoint, 0.25f);
            }
        }
    }
}
