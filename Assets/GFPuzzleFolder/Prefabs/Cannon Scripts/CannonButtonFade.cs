using UnityEngine;
using UnityEngine.SceneManagement;
using Oculus.Interaction;

public class CannonButtonSceneLoad : MonoBehaviour
{
    [Header("Button")]
    public PokeInteractable pokeButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip cannonSound;

    [Header("Scene")]
    [Tooltip("The exact name of the scene you want to load")]
    public string sceneToLoad;

    [Tooltip("How long to wait after the cannon sound before loading the scene")]
    public float delayBeforeLoad = 1.5f;

    private bool hasTriggered = false;

    private void Start()
    {
        if (pokeButton != null)
            pokeButton.WhenSelectingInteractorViewAdded += (_) => OnButtonPoked();
    }

    private void OnDestroy()
    {
        if (pokeButton != null)
            pokeButton.WhenSelectingInteractorViewAdded -= (_) => OnButtonPoked();
    }

    private void OnButtonPoked()
    {
        if (hasTriggered) return;
        hasTriggered = true;

        PlayCannonSound();
        Invoke(nameof(LoadScene), delayBeforeLoad);
    }

    private void PlayCannonSound()
    {
        if (audioSource != null && cannonSound != null)
            audioSource.PlayOneShot(cannonSound);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ResetButton()
    {
        hasTriggered = false;
    }
}