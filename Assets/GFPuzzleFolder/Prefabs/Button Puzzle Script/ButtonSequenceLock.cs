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
    public AudioClip correctButtonSound;  // plays each time a correct button is pressed
    public AudioClip successSound;        // plays when ALL buttons are correct
    public AudioClip wrongSound;          // plays on any wrong press

    [Header("Correct Sequence")]
    public PokeInteractable button0;
    public PokeInteractable button1;
    public PokeInteractable button2;
    public PokeInteractable button3;

    // --- Private State ---
    private List<int> correctOrder;
    private List<int> playerInput = new List<int>();
    private bool sequenceSolved = false;
    private bool isResetting = false;

    private float lastPressTime = -1f;
    private float pressCooldown = 0.4f;

    private void Start()
    {
        correctOrder = new List<int> { 0, 1, 2, 3 };
    }

    public void OnButton0Pressed() { RegisterPress(0); }
    public void OnButton1Pressed() { RegisterPress(1); }
    public void OnButton2Pressed() { RegisterPress(2); }
    public void OnButton3Pressed() { RegisterPress(3); }

    private void RegisterPress(int buttonIndex)
    {
        if (sequenceSolved) return;
        if (isResetting) return;

        if (Time.time - lastPressTime < pressCooldown) return;
        lastPressTime = Time.time;

        Debug.Log($"Button {buttonIndex} pressed. Step {playerInput.Count + 1} of {correctOrder.Count}");

        playerInput.Add(buttonIndex);

        int step = playerInput.Count - 1;

        // Wrong button
        if (playerInput[step] != correctOrder[step])
        {
            Debug.Log("Wrong button! Resetting immediately.");
            PlaySound(wrongSound);
            StartCoroutine(ResetAfterWrong());
            return;
        }

        // Correct button pressed — check if it's the last one
        if (playerInput.Count == correctOrder.Count)
        {
            // All buttons correct — play success sound
            sequenceSolved = true;
            Debug.Log("Sequence complete!");
            PlaySound(successSound);
            TriggerSuccess();
        }
        else
        {
            // Correct button but not finished yet — play the per-button correct sound
            Debug.Log($"Correct! {playerInput.Count} of {correctOrder.Count} done.");
            PlaySound(correctButtonSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    private System.Collections.IEnumerator ResetAfterWrong()
    {
        isResetting = true;
        playerInput.Clear();
        yield return new WaitForSeconds(0.1f);
        isResetting = false;
        Debug.Log("Ready for new input.");
    }

    private void TriggerSuccess()
    {
        if (objectToDisable != null)
            objectToDisable.SetActive(false);

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
        isResetting = false;
    }
}