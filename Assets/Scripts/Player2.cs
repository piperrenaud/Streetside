using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : PlayerBase
{
    public float jumpHeight = 3.5f;
    public float duration = 0.8f;
    
    private bool bNearHighJump = false;
    private Transform jumpNodeTransform;
    
    public override void OnJump(InputValue value)
    {
        base.OnJump(value);

        if (bNearHighJump && !bIsJumping && jumpNodeTransform != null)
        {
            StartCoroutine(AnimateHighJump());
        }
    }

    IEnumerator AnimateHighJump()
    {
        bIsJumping = true;
        rb.linearVelocity = Vector3.zero;
        
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.forward * 2f;

        float elapsed = 0f;

        if (animator != null)
        {
            animator.SetBool("Falling", true);
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            
            Vector3 currentXZ = Vector3.Lerp(startPos, endPos, percent);

            float currentY = Mathf.Sin(percent * Mathf.PI) * jumpHeight;
            
            transform.position = new Vector3(currentXZ.x, startPos.y + currentY, currentXZ.z);
            yield return null;
        }
        
        transform.position = new Vector3(Mathf.Round(endPos.x), endPos.y, Mathf.Round(endPos.z));
        targetPos = transform.position;

        if (animator != null)
        {
            animator.SetBool("Falling", false);
        }
        bIsJumping = false;
        bIsMoving = false;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HighJump"))
        {
            bNearHighJump = true;
            jumpNodeTransform = other.transform;
            
            ForceStop(other.transform.position);
        }
        
        if (other.CompareTag("LongJump"))
        {
            ForceStop(other.transform.position);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HighJump"))
        {
            bNearHighJump = false;
            jumpNodeTransform = null;
        }
    }
}
