using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask wallLayer;

    private Vector3 currentDirection;
    private Vector3 nextDirection;
    private Vector3 targetPos;
    private bool isMoving;

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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
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
        else
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.001f)
        {
            transform.position = targetPos;
            isMoving = false;
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
