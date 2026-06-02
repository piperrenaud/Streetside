using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : PlayerBase
{
    public float jumpHeight = 3.5f;
    public float duration = 0.8f;
    
    public float slideDuration = 0.5f;
    public float slideDistance = 2f;
    
    private bool bNearHighJump = false;
    private Transform jumpNodeTransform;

    private bool bNearSlide = false;

    private CapsuleCollider playerCollider;

    private void Awake()
    {
        playerCollider = GetComponentInChildren<CapsuleCollider>();
    }

    public override void OnJump(InputValue value)
    {
        base.OnJump(value);

        if (bNearHighJump && !bIsJumping && jumpNodeTransform != null)
        {
            StartCoroutine(AnimateHighJump());
        }
    }

    public override void OnInteract(InputValue value)
    {
        base.OnInteract(value);

        if (bNearSlide && !bIsSliding)
        {
            StartCoroutine(AnimateSlide());
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

    IEnumerator AnimateSlide()
    {
        bIsSliding = true;
        rb.linearVelocity = Vector3.zero;
        
        float originalHeight = playerCollider.height;
        Vector3 originalCenter = playerCollider.center;

        playerCollider.height = originalHeight * 0.5f;
        playerCollider.center = new Vector3(originalCenter.x, originalCenter.y * 0.5f, originalCenter.z);
        
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.forward * slideDistance;
        
        float elapsed = 0f;
        
        // add start slide anim here

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / slideDuration;
            
            transform.position = Vector3.Lerp(startPos, endPos, percent);
            yield return null;
        }
        
        transform.position = new Vector3(Mathf.Round(endPos.x), endPos.y, Mathf.Round(endPos.z));
        targetPos = transform.position;
        
        playerCollider.height = originalHeight;
        playerCollider.center = originalCenter;
        
        // add stop slide anim here

        bIsSliding = false;
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
        
        if (other.CompareTag("Slide"))
        {
            bNearSlide = true;
            
            ForceStop(other.transform.position);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HighJump"))
        {
            bNearHighJump = false;
        }
        
        if (other.CompareTag("Slide"))
        {
            bNearSlide = false;
            jumpNodeTransform = null;
        }
    }
}
