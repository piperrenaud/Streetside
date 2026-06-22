using UnityEngine;
using UnityEngine.Events;

public class LeverPuzzle : MonoBehaviour
{
    public enum PuzzleType
    {
        AnyLeverFlipped, //lever flipped to on = event happens
        RequiredNumberOn, //needs exact # of levers flipped to on = event happens
        ExactPattern //needs levers set in exact pattern (on/off/on/off) = event happens
    }

    [Header("Puzzle Setting")]
    [Tooltip("AnyLeverFlipped: lever flipped to on = event happens" +
        "\n\nRequireNumberOn: needs exact # of levers flipped to on = event happens" +
        "\n\nExactPattern: needs levers set in exact pattern (on/off/on/off) = event happens")]
    public PuzzleType puzzleType;

    [Tooltip("All levers used in this puzzle, in pattern order")]
    public LeverSwitch[] levers;

    [Header("If type = RequiredNumberOn")]
    [Tooltip("Used for RequiredNumberOn puzzles")]
    public int requiredOnCount = 1;

    [Header("If type = ExactPattern")]
    [Tooltip("Used for ExactPattern puzzles. true = on/right, false = off/left")]
    public bool[] requiredPattern;

    [Header("Events")]
    public UnityEvent onPuzzleSolved;
    public UnityEvent onPuzzleUnsolved;

    private bool isSolved;

    private void Start()
    {
        foreach (LeverSwitch lever in levers)
        {
            lever.SetPuzzle(this);
        }

        CheckPuzzle();
    }

    public void OnLeverFlipped()
    {
        CheckPuzzle();
    }

    private void CheckPuzzle()
    {
        bool solved = false;

        switch (puzzleType)
        {
            case PuzzleType.AnyLeverFlipped:
                solved = true;
                break;

            case PuzzleType.RequiredNumberOn:
                int onCount = 0;

                foreach (LeverSwitch lever in levers)
                {
                    if (lever.IsOn) onCount++;
                }

                solved = onCount >= requiredOnCount;
                break;

            case PuzzleType.ExactPattern:
                if (requiredPattern.Length != levers.Length)
                {
                    Debug.LogWarning("Required pattern length must match lever count.");
                    return;
                }

                solved = true;

                for (int i = 0; i < levers.Length; i++)
                {
                    if (levers[i].IsOn != requiredPattern[i])
                    {
                        solved = false;
                        break;
                    }
                }

                break;
        }

        if (solved && !isSolved)
        {
            isSolved = true;
            onPuzzleSolved.Invoke();
        }
        else if (!solved && isSolved)
        {
            isSolved = false;
            onPuzzleUnsolved.Invoke();
        }
    }

    public void TestingPuzzleDebugs(string textToSay)
    {
        Debug.Log(textToSay);
    }
}
