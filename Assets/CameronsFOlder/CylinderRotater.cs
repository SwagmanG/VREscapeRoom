using UnityEngine;

public class CylinderRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float rotationIncrement = 36f; // 360 / 10

    [Header("Values")]
    [SerializeField] private int startingValue = 0; // what cylinder starts at
    [SerializeField] private int targetValue = 0;   // correct solution

    [SerializeField] private int currentValue = 0;

    private float targetRotation = 0f;
    private float currentRotation = 0f;
    private bool isRotating = false;

    private Quaternion baseRotation;

    private float lastRotationTime = 0f;
    [SerializeField] private float rotationCooldown = 0.25f;

    private void Start()
    {
        baseRotation = transform.rotation;

        currentValue = startingValue;

        targetRotation = currentValue * rotationIncrement;
        currentRotation = targetRotation;

        transform.rotation = baseRotation * Quaternion.Euler(currentRotation, 0, 0);
    }

    private void Update()
    {
        if (isRotating)
        {
            currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);

            transform.rotation = baseRotation * Quaternion.Euler(currentRotation, 0, 0);

            if (Mathf.Abs(targetRotation - currentRotation) < 0.1f)
            {
                currentRotation = targetRotation;
                transform.rotation = baseRotation * Quaternion.Euler(currentRotation, 0, 0);
                isRotating = false;
            }
        }
    }

    public void RotateUp()
    {
        if (Time.time - lastRotationTime < rotationCooldown)
            return;

        currentValue = (currentValue + 1) % 10;

        targetRotation = currentValue * rotationIncrement;
        isRotating = true;

        lastRotationTime = Time.time;
    }

    public void RotateDown()
    {
        if (Time.time - lastRotationTime < rotationCooldown)
            return;

        currentValue--;

        if (currentValue < 0)
            currentValue = 9;

        targetRotation = currentValue * rotationIncrement;
        isRotating = true;

        lastRotationTime = Time.time;
    }

    public void SetTargetValue(int value)
    {
        targetValue = Mathf.Clamp(value, 0, 9);
    }

    public int GetCurrentValue()
    {
        return currentValue;
    }

    public bool IsCorrect()
    {
        return currentValue == targetValue;
    }

    public void Reset()
    {
        currentValue = startingValue;

        targetRotation = currentValue * rotationIncrement;
        currentRotation = targetRotation;

        transform.rotation = baseRotation * Quaternion.Euler(currentRotation, 0, 0);
        isRotating = false;
    }
}