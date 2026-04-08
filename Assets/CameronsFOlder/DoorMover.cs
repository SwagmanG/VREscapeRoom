using UnityEngine;

public class DoorMover : MonoBehaviour
{
    [Header("Door References")]
    [SerializeField] private Transform[] doors;

    [Header("Movement Settings")]
    [SerializeField] private Vector3 moveDirection = new Vector3(0, 0, -1); // Back
    [SerializeField] private float moveDistance = 5f; // How far to move
    [SerializeField] private float moveSpeed = 2f; // Speed of movement
    [SerializeField] private bool useWorldSpace = true; // Move in world or local space

    [Header("Additional Settings")]
    [SerializeField] private bool moveOnStart = false; // For testing

    private Vector3[] startPositions;
    private Vector3[] targetPositions;
    private bool isMoving = false;
    private float moveProgress = 0f;

    private void Start()
    {
        if (doors.Length == 0)
        {
            Debug.LogWarning("DoorMover: No doors assigned!");
            return;
        }

        // Store initial positions
        startPositions = new Vector3[doors.Length];
        targetPositions = new Vector3[doors.Length];

        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i] != null)
                startPositions[i] = doors[i].position;
        }

        if (moveOnStart)
            MoveDoors();
    }

    private void Update()
    {
        if (isMoving)
        {
            moveProgress += Time.deltaTime * moveSpeed;

            // Clamp progress to 0-1
            if (moveProgress >= 1f)
            {
                moveProgress = 1f;
                isMoving = false;
            }

            // Move all doors smoothly
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i] != null)
                {
                    doors[i].position = Vector3.Lerp(startPositions[i], targetPositions[i], moveProgress);
                }
            }
        }
    }

    /// <summary>
    /// Called when puzzle is solved - moves all doors
    /// </summary>
    public void MoveDoors()
    {
        if (doors.Length == 0)
        {
            Debug.LogWarning("DoorMover: No doors to move!");
            return;
        }

        // Calculate target positions for each door
        Vector3 movement = moveDirection.normalized * moveDistance;

        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i] != null)
            {
                if (useWorldSpace)
                {
                    targetPositions[i] = startPositions[i] + movement;
                }
                else
                {
                    // Local space movement
                    targetPositions[i] = startPositions[i] + doors[i].parent.TransformDirection(movement);
                }
            }
        }

        moveProgress = 0f;
        isMoving = true;
        Debug.Log($"Moving {doors.Length} door(s)...");
    }


    /// <summary>
    /// Set movement properties at runtime
    /// </summary>
    public void SetMovement(Vector3 direction, float distance, float speed)
    {
        moveDirection = direction;
        moveDistance = distance;
        moveSpeed = speed;
    }
}