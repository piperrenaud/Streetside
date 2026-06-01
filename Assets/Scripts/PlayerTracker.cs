using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private Transform playerOne;
    [SerializeField] private Transform playerTwo;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask wallLayer;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!mainCamera) return;

        TrackPlayer(playerOne, "_PlayerOneScreenPos", "_PlayerOneOccluded");
        TrackPlayer(playerTwo, "_PlayerTwoScreenPos", "_PlayerTwoOccluded");
    }

    private void TrackPlayer(Transform player, string shaderPosKey, string shaderOccludedKey)
    {
        if (!player)
        {
            Shader.SetGlobalFloat(shaderOccludedKey, 0f);
            return;
        }
        
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(player.position + Vector3.up);
        Shader.SetGlobalVector(shaderPosKey, new Vector4(screenPoint.x, screenPoint.y, 0, 0));
        
        Vector3 directionToPlayer = (player.position + Vector3.up) - mainCamera.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (Physics.Raycast(mainCamera.transform.position, directionToPlayer.normalized, out RaycastHit hit,
                distanceToPlayer, wallLayer))
        {
            Shader.SetGlobalFloat(shaderOccludedKey, 1f);
        }
        else
        {
            Shader.SetGlobalFloat(shaderOccludedKey, 0f);
        }
    }
}
