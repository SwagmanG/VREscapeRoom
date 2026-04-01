using UnityEngine;

public class TorchActivator : MonoBehaviour
{
    [SerializeField] private string requiredObjectTag = "Torch";
    [SerializeField] private GameObject objectToEnable;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(requiredObjectTag))
        {
            Debug.Log("Torch detected!");
            ActivateObject(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag(requiredObjectTag))
        {
            Debug.Log("Torch left!");
            ActivateObject(false);
        }
    }

    void ActivateObject(bool visible)
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(visible);
            Debug.Log("Object set active: " + visible + " — " + objectToEnable.name);
        }
        else
        {
            Debug.LogWarning("No object assigned to enable!");
        }
    }
}
