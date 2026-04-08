using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    [Header("Cylinder References")]
    [SerializeField] private CylinderRotator[] cylinders = new CylinderRotator[3];

    [Header("Target Values")]
    [SerializeField] private int[] targetValues = new int[3];

    [Header("Check Settings")]
    [SerializeField] private float checkInterval = 0.25f;
    private float lastCheckTime = 0f;

    [Header("Events")]
    [SerializeField] private UnityEvent onPuzzleSolved;

    private bool puzzleSolved = false;

    private void Start()
    {
        SetTargetValues(targetValues[0], targetValues[1], targetValues[2]);
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkInterval && !puzzleSolved)
        {
            CheckPuzzle();
            lastCheckTime = Time.time;
        }
    }

    public void SetTargetValues(int cyl1, int cyl2, int cyl3)
    {
        targetValues[0] = cyl1;
        targetValues[1] = cyl2;
        targetValues[2] = cyl3;

        for (int i = 0; i < cylinders.Length; i++)
        {
            if (cylinders[i] != null)
                cylinders[i].SetTargetValue(targetValues[i]);
        }

        puzzleSolved = false;

        Debug.Log($"Puzzle Target: {cyl1} {cyl2} {cyl3}");
    }

    private void CheckPuzzle()
    {
        for (int i = 0; i < cylinders.Length; i++)
        {
            if (!cylinders[i].IsCorrect())
                return;
        }

        SolvePuzzle();
    }

    private void SolvePuzzle()
    {
        puzzleSolved = true;

        Debug.Log("PUZZLE SOLVED");

        onPuzzleSolved?.Invoke();
    }

    public void ResetPuzzle()
    {
        foreach (var cylinder in cylinders)
        {
            cylinder.Reset();
        }

        puzzleSolved = false;
    }

    public bool IsSolved()
    {
        return puzzleSolved;
    }
}