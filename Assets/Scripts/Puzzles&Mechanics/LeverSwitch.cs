using UnityEngine;

public class LeverSwitch : MonoBehaviour
{
    [Header("Lever Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private bool startsOn = false;

    [Tooltip("Prevents rapid double flipping")]
    [SerializeField] private float coolDown = 0.5f;

    public bool IsOn {  get; private set; }

    private LeverPuzzle puzzle;
    private float lastFlipTime = -999f;

    private void Start()
    {
        IsOn = startsOn;

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    public void SetPuzzle(LeverPuzzle newPuzzle)
    {
        puzzle = newPuzzle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time < lastFlipTime + coolDown) return;

        Flip();
    }

    private void Flip()
    {
        lastFlipTime = Time.time;

        IsOn = !IsOn;

        if (IsOn) animator.SetTrigger("Right");
        else animator.SetTrigger("Left");

        if (puzzle != null) puzzle.OnLeverFlipped();
    }
}
