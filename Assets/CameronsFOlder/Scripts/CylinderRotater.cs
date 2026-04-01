using UnityEngine;

public class CylinderRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float rotationIncrement = 36f; // 360 / 10 sides = 36 degrees per click

    private float targetRotation = 0f;
    private float currentRotation = 0f;
    private bool isRotating = false;

    // Cooldown to prevent multiple triggers
    private float lastRotationTime = 0f;
    [SerializeField] private float rotationCooldown = 0.5f;

    private void Update()
    {
        if (isRotating)
        {
            // Smoothly rotate towards target
            currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Apply rotation
            transform.rotation = Quaternion.Euler(currentRotation, 0, 0);

            // Stop when close enough
            if (Mathf.Abs(targetRotation - currentRotation) < 0.1f)
            {
                currentRotation = targetRotation;
                transform.rotation = Quaternion.Euler(currentRotation, 0, 0);
                isRotating = false;
            }
        }
    }

    // Call this when a hand touches the UP button
    public void RotateUp()
    {
        if (Time.time - lastRotationTime > rotationCooldown)
        {
            targetRotation += rotationIncrement;
            isRotating = true;
            lastRotationTime = Time.time;
        }
    }

    // Call this when a hand touches the DOWN button
    public void RotateDown()
    {
        if (Time.time - lastRotationTime > rotationCooldown)
        {
            targetRotation -= rotationIncrement;
            isRotating = true;
            lastRotationTime = Time.time;
        }
    }
}