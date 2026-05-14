using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : PlayerBase
{
    private float jumpInteractDistance = 5f;

    public override void OnJump(InputValue value)
    {
        base.OnJump(value);
        
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.5f;
        Physics.Raycast(rayStart, transform.forward, out hit, jumpInteractDistance, wallLayer);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("HighJump"))
            {
                isGrounded = false;
                Vector3 jumpDirection = (Vector3.up * jumpForce) + (transform.forward * forwardForce);
            
                rb.AddForce(jumpDirection, ForceMode.VelocityChange);
            }
        }
    }
}
