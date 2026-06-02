using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : MonoBehaviour
{
    public float moveSpeed = 5f;

    public LayerMask wallLayer;
    public LayerMask interactLayer;

    protected Vector3 currentDirection;
    protected Vector3 nextDirection;
    protected Vector3 targetPos;
    
    protected bool bIsMoving;
    protected bool bIsJumping = false;
    
    protected Rigidbody rb;
    protected Animator animator;
    
    public void OnMove(InputValue value)
    {
        if (bIsJumping) return;
        
        Vector2 input = value.Get<Vector2>();

        if (input.x != 0 && input.y != 0)
        {
            return;
        }

        if (input != Vector2.zero)
        {
            nextDirection = new Vector3(input.x, 0, input.y);
        }
    }

    public virtual void OnJump(InputValue value)
    {
        
    }

    public virtual void OnInteract(InputValue value)
    {
        
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        targetPos = transform.position;
    }

    void Update()
    {
        if (bIsJumping) return;

        if (!bIsMoving)
        {
            if (CanMove(nextDirection))
            {
                currentDirection = nextDirection;
            }

            if (CanMove(currentDirection))
            {
                targetPos = transform.position + currentDirection;
                bIsMoving = true;
            }
            else
            {
                rb.linearVelocity = Vector3.zero;
                //anim stuff
                if (animator != null)
                {
                    animator.SetBool("Running", false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (bIsMoving && !bIsJumping)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        //animation stuff
        if (animator != null)
        {
            animator.SetBool("Running", true);
            animator.SetBool("Falling", false);
        }

       Vector3 moveDir = (targetPos - transform.position).normalized;
       
       rb.linearVelocity = moveDir * moveSpeed;

       if (moveDir != Vector3.zero)
       {
           Quaternion targetRotation = Quaternion.LookRotation(moveDir);
           transform.rotation = targetRotation;
       }
       
       if (Vector3.Distance(transform.position, targetPos) < 0.05f)
       {
           transform.position = targetPos;
           rb.linearVelocity = Vector3.zero;
           bIsMoving = false;
       }
    }

    bool CanMove(Vector3 direction)
    {
        if (direction == Vector3.zero)
        {
            return false;
        }
        
        Vector3 rayStart = transform.position + Vector3.up * 0.5f;
        bool hit = Physics.Raycast(rayStart, direction, 1.0f, wallLayer);

        Debug.DrawRay(rayStart, direction * 1.0f, hit ? Color.red : Color.green);
        
        return !hit;
    }

    public void ForceStop(Vector3 position)
    {
        bIsMoving = false;
        currentDirection = Vector3.zero;
        rb.linearVelocity = Vector3.zero;

        // Snap cleanly to the specified grid tile
        transform.position = new Vector3(
            Mathf.Round(position.x),
            transform.position.y,
            Mathf.Round(position.z)
        );
        targetPos = transform.position;

        if (animator != null) 
        {
            animator.SetBool("Running", false);
        }
    }

    public void HandleItemDropDisplacemnt()
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(rayStart, transform.forward, 1.0f, interactLayer))
        {
            transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
        
            targetPos = transform.position;
            bIsMoving = false;
            currentDirection = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        
            if (animator != null) animator.SetBool("Running", false);
        }
    }
}
