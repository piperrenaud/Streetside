using UnityEngine;

public class LiftingDoor : MonoBehaviour
{
    [Header("Door Movement")]
    public float doorOpenHeight = 5f;
    public float moveSpeed = 3f;
    public Transform door;

    private Vector3 targetPosition;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        if (door == null) door = transform.GetChild(0);

        closedPosition = door.position;
        openPosition = closedPosition + Vector3.up * doorOpenHeight;

        targetPosition = closedPosition;
    }

    private void Update()
    {
        door.position = Vector3.MoveTowards(
            door.position,
            targetPosition,
            moveSpeed * Time.deltaTime );
    }

    public void OpenDoor()
    {
        targetPosition = openPosition;
    }

    public void CloseDoor()
    {
        targetPosition = closedPosition;
    }
}
