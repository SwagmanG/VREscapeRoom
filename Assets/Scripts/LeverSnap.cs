using UnityEngine;

public class LeverSnap : MonoBehaviour
{
    [SerializeField] public Transform leverMesh; // Drag your mesh here
    [SerializeField] public float minPosition = -0.5f;
    [SerializeField] public float maxPosition = 0.5f;
    [SerializeField] public float snapSpeed = 5f;
    private float targetPosition;
    private bool isGrabbed = false;
    private Vector3 meshStartPos;
    private Vector3 handleStartPos;
    [Range(0f, 1f)] public float leverValue { get; private set; }

    void Start()
    {
        if (leverMesh != null)
            meshStartPos = leverMesh.localPosition;
        handleStartPos = transform.localPosition;
        targetPosition = meshStartPos.x;
    }

    void Update()
    {
        if (leverMesh == null) return;

        // Calculate how far the handle moved
        float handleMovement = transform.localPosition.x - handleStartPos.x;

        // Apply that movement to the mesh, clamped
        Vector3 meshPos = leverMesh.localPosition;
        meshPos.x = meshStartPos.x + Mathf.Clamp(handleMovement, minPosition - meshStartPos.x, maxPosition - meshStartPos.x);
        leverMesh.localPosition = meshPos;

        // Snap back when released
        if (!isGrabbed)
        {
            float mid = (minPosition + maxPosition) / 2f;
            targetPosition = meshPos.x > mid ? maxPosition : minPosition;
            meshPos.x = Mathf.Lerp(meshPos.x, targetPosition, snapSpeed * Time.deltaTime);
            leverMesh.localPosition = meshPos;
        }

        leverValue = Mathf.InverseLerp(minPosition, maxPosition, leverMesh.localPosition.x);
    }

    // Call this when the lever is grabbed
    public void OnGrab()
    {
        isGrabbed = true;
    }

    // Call this when the lever is released
    public void OnRelease()
    {
        isGrabbed = false;
    }
}