using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1 : PlayerBase
{
    private float jumpInteractDistance = 5f;
    private bool bCanPickUp = false;
    private GameObject pickupItem;
    private bool bIsPickedUp = false;
    
    public override void OnJump(InputValue value)
    {
        base.OnJump(value);

        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.5f;
        Physics.Raycast(rayStart, transform.forward, out hit, jumpInteractDistance, wallLayer);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("LongJump"))
            {
                isGrounded = false;

                //animation stuff
                if (animator != null)
                {
                    animator.SetBool("Running", false);
                    animator.SetBool("Falling", true);
                }

                Vector3 jumpDirection = (Vector3.up * jumpForce) + (transform.forward * forwardForce);
            
                rb.AddForce(jumpDirection, ForceMode.VelocityChange);
            }
        }
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
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            bCanPickUp = true;
            pickupItem = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            bCanPickUp = false;
            pickupItem = null;
        }
    }
}
