using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private float stepDelay = 0.5f;

    private float stepTimer = 0f;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleFootsteps();
    }

    void HandleFootsteps()
    {
        // Берём только горизонтальную скорость, без Y
        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
        bool isMoving = horizontalVelocity.magnitude > 0.1f && characterController.isGrounded;

        if (isMoving)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepDelay)
            {
                if (!footstepAudioSource.isPlaying)
                {
                    footstepAudioSource.Play(); // Использует Random Container
                }

                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = stepDelay;
        }
    }
}
