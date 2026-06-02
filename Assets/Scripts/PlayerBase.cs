using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    public float forwardForce = 15f;
    public LayerMask wallLayer;
    public LayerMask groundLayer;


    private Vector3 currentDirection;
    private Vector3 nextDirection;
    protected Vector3 targetPos;
    protected bool isMoving;
    protected bool isGrounded;
    
    protected Rigidbody rb;
    protected Animator animator;
    
    public void OnMove(InputValue value)
    {
        Debug.Log($"{gameObject.name} received input: {value.Get<Vector2>()}");
        
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
        isGrounded = true;
        targetPos = transform.position;
    }

    void Update()
    {
        if (!isMoving)
        {
            if (CanMove(nextDirection))
            {
                currentDirection = nextDirection;
            }

            if (CanMove(currentDirection))
            {
                targetPos = transform.position + currentDirection;

                isMoving = true;

                if (currentDirection != Vector3.zero)
                {
                    transform.forward = currentDirection;
                }
            }
        }
        else if (!isGrounded)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1f, groundLayer))
            {
                targetPos = transform.position;
                isGrounded = true;
                isMoving = false;

                //anim stuff
                if (animator != null)
                {
                    animator.SetBool("Falling", false);
                }
            }
        }
        else
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

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            transform.position = targetPos;
            isMoving = false;

            //anim stuff
            if (animator != null)
            {
                animator.SetBool("Running", false);
            }
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
}
