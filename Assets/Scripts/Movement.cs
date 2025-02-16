using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustStrength = 1000f;
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float rotationStrength = 50f;
    [SerializeField] AudioClip thrustSound;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftEngineParticles;
    [SerializeField] ParticleSystem rightEngineParticles;
    //caching variables
    Rigidbody rb;
    AudioSource audioSource;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else if (!thrust.IsPressed())
        {
            StopThrusting();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustSound);
        }
        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput < 0)
        {
            ApplyRotation(rotationStrength);
            RightEngineThrust();
        }
        else if (rotationInput > 0)
        {
            ApplyRotation(-rotationStrength);
            LeftEngineThrust();
        }
        else
        {
            StopSideEnginesThrust();
        }
    }

    private void StopSideEnginesThrust()
    {
        rightEngineParticles.Stop();
        leftEngineParticles.Stop();
    }

    private void LeftEngineThrust()
    {
        if (!leftEngineParticles.isPlaying)
        {
            rightEngineParticles.Stop();
            leftEngineParticles.Play();
        }
    }

    private void RightEngineThrust()
    {
        if (!rightEngineParticles.isPlaying)
        {
            leftEngineParticles.Stop();
            rightEngineParticles.Play();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }
}
