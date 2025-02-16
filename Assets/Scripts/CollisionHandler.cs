using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float delay = 2f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;
    AudioSource audioSource;

    bool isControllable = true;
    bool isCollidable = true;

    private void Start()
    {
        if (crashParticles == null)
        {
            Debug.LogError("Crash particles are not assigned!");
        }
        else
        {
            crashParticles.Stop();
        }

        if (successParticles == null)
        {
            Debug.LogError("Success particles are not assigned!");
        }
        else
        {
            successParticles.Stop();
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Audio source is not assigned!");
        }
    }
    private void Update()
    {
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            isCollidable = !isCollidable;
            StartCrashSequence();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!isControllable || !isCollidable) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Complete":
                Debug.Log("Level complete");
                StartSuccessSequence();
                break;
            case "fuel":
                Debug.Log("Fuel collected");
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        if (crashParticles != null)
        {
            crashParticles.Play();
        }
        isControllable = false;
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(crashSound);
        }
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(ReloadLevel), delay);
    }

    void StartSuccessSequence()
    {
        if (successParticles != null)
        {
            successParticles.Play();
        }
        isControllable = false;
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(successSound);
        }
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(LoadNextLevel), delay);
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        if (nextScene >= SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        SceneManager.LoadScene(nextScene);
    }
}
