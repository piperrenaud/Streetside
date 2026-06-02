using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1 : PlayerBase
{
    public float jumpHeight = 1.2f;
    public float duration = 0.6f;
    public float jumpDistance = 5f;

    
    private bool bNearLongJump = false;
    private Transform jumpNodeTransform;
    
    private bool bCanPickUp = false;
    private GameObject pickupItem;
    private bool bIsPickedUp = false;
    
    public override void OnJump(InputValue value)
    {
        base.OnJump(value);

        if (bNearLongJump && !bIsJumping && jumpNodeTransform != null)
        {
            StartCoroutine(AnimateLongJump());
        }
    }

    IEnumerator AnimateLongJump()
    {
        bIsJumping = true;
        rb.linearVelocity = Vector3.zero;
        
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.forward * jumpDistance;

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

    public override void OnInteract(InputValue value)
    {
        base.OnInteract(value);

        if (!bIsPickedUp)
        {
            if (bCanPickUp && pickupItem)
            {
                bIsPickedUp = true;
                pickupItem.transform.SetParent(this.transform);
            }
        }
        else
        {
            bIsPickedUp = false;
            pickupItem.transform.SetParent(null);

            Vector3 dropPos = transform.position + transform.forward * 1f;
            
            pickupItem.transform.position = new Vector3(Mathf.Round(dropPos.x), transform.position.y, Mathf.Round(dropPos.z));

            HandleItemDropDisplacemnt();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LongJump"))
        {
            bNearLongJump = true;
            jumpNodeTransform = other.transform;
            
            ForceStop(other.transform.position);
        }
        
        if (other.CompareTag("HighJump"))
        {
            ForceStop(other.transform.position);
        }
        
        if (other.CompareTag("Interactable"))
        {
            bCanPickUp = true;
            pickupItem = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LongJump"))
        {
            bNearLongJump = false;
            jumpNodeTransform = null;
        }
        
        if (other.CompareTag("Interactable"))
        {
            bCanPickUp = false;
            pickupItem = null;
        }
    }
}
