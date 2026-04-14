using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class ButtonSequenceLock : MonoBehaviour
{
    [Header("Sequence Setup")]
    [Tooltip("Drag your 4 PokeInteractable buttons here IN THE CORRECT ORDER they must be pressed")]
    public List<PokeInteractable> correctSequence = new List<PokeInteractable>();

    [Header("Cannon Setup")]
    [Tooltip("The cannon GameObject to move")]
    public GameObject cannon;
    [Tooltip("The spawn point Transform in the NEW room")]
    public Transform cannonSpawnPoint;

    [Header("Object to Disable")]
    [Tooltip("The GameObject that gets disabled when sequence is complete")]
    public GameObject objectToDisable;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip successSound;

    // --- Private State ---
    private List<PokeInteractable> playerInput = new List<PokeInteractable>();
    private bool sequenceSolved = false;

    private void Start()
    {
        foreach (PokeInteractable button in correctSequence)
        {
            if (button != null)
            {
                PokeInteractable localRef = button;
                button.WhenSelectingInteractorViewAdded += (_) => OnButtonPressed(localRef);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (PokeInteractable button in correctSequence)
        {
            if (button != null)
            {
                PokeInteractable localRef = button;
                button.WhenSelectingInteractorViewAdded -= (_) => OnButtonPressed(localRef);
            }
        }
    }

    private void OnButtonPressed(PokeInteractable pressedButton)
    {
        if (sequenceSolved) return;

        playerInput.Add(pressedButton);

        int step = playerInput.Count - 1;

        // Wrong button — reset
        if (playerInput[step] != correctSequence[step])
        {
            Debug.Log("Wrong order! Resetting sequence.");
            playerInput.Clear();
            return;
        }

        // Correct so far — check if complete
        if (playerInput.Count == correctSequence.Count)
        {
            sequenceSolved = true;
            Debug.Log("Correct sequence! Triggering effects.");
            TriggerSuccess();
        }
    }

    private void TriggerSuccess()
    {
        // 1. Disable target object
        if (objectToDisable != null)
            objectToDisable.SetActive(false);

        // 2. Play success sound
        if (audioSource != null && successSound != null)
            audioSource.PlayOneShot(successSound);

        // 3. Teleport cannon to new room
        if (cannon != null && cannonSpawnPoint != null)
        {
            cannon.transform.position = cannonSpawnPoint.position;
            cannon.transform.rotation = cannonSpawnPoint.rotation;
            cannon.SetActive(true);
        }
    }

    public void ResetPuzzle()
    {
        playerInput.Clear();
        sequenceSolved = false;
    }
}