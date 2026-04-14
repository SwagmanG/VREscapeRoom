using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class ButtonSequenceLock : MonoBehaviour
{
    [Header("Cannon Setup")]
    public GameObject cannon;
    public Transform cannonSpawnPoint;

    [Header("Object to Disable")]
    public GameObject objectToDisable;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip successSound;

    // The correct order, 0 = first button that should be pressed
    // Assign these in inspector by dragging the PokeInteractables
    [Header("Correct Sequence")]
    public PokeInteractable button0;
    public PokeInteractable button1;
    public PokeInteractable button2;
    public PokeInteractable button3;

    // --- Private State ---
    private List<int> correctOrder;
    private List<int> playerInput = new List<int>();
    private bool sequenceSolved = false;

    private float lastPressTime = -1f;
    private float pressCooldown = 0.4f;

    private void Start()
    {
        // Build the correct order list from the assigned buttons
        // The ORDER you assign them (0,1,2,3) IS the correct sequence
        correctOrder = new List<int> { 0, 1, 2, 3 };
    }

    // --- These get called by InteractableUnityEventWrapper on each button ---

    public void OnButton0Pressed()
    {
        RegisterPress(0);
    }

    public void OnButton1Pressed()
    {
        RegisterPress(1);
    }

    public void OnButton2Pressed()
    {
        RegisterPress(2);
    }

    public void OnButton3Pressed()
    {
        RegisterPress(3);
    }

    // --------------------------------------------------------

    private void RegisterPress(int buttonIndex)
    {
        if (sequenceSolved) return;

        // Cooldown to prevent double-firing
        if (Time.time - lastPressTime < pressCooldown) return;
        lastPressTime = Time.time;

        Debug.Log($"Button {buttonIndex} pressed. Step {playerInput.Count + 1} of {correctOrder.Count}");

        playerInput.Add(buttonIndex);

        int step = playerInput.Count - 1;

        if (playerInput[step] != correctOrder[step])
        {
            Debug.Log("Wrong order! Resetting.");
            playerInput.Clear();
            return;
        }

        if (playerInput.Count == correctOrder.Count)
        {
            sequenceSolved = true;
            Debug.Log("Sequence complete!");
            TriggerSuccess();
        }
    }

    private void TriggerSuccess()
    {
        if (objectToDisable != null)
            objectToDisable.SetActive(false);

        if (audioSource != null && successSound != null)
            audioSource.PlayOneShot(successSound);

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