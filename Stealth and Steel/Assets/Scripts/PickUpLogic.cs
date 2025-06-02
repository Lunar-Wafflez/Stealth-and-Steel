using Unity.VisualScripting;
using UnityEngine;

public class PickUpLogic : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private PickUpType _pickupType;
    [SerializeField]
    private AudioClip kunaiPickupSound;
    [SerializeField]
    private AudioClip smokeBombPickupSound;

    private AudioSource audioSource;
    enum PickUpType
    {
        Kunai,
        SmokeBomb
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();         
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); 
        }
        audioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(((1 << other.gameObject.layer) & _layerMask ) != 0)
        { 
            PlayerMovementScript playerMovement = other.GetComponent<PlayerMovementScript>();
            if (playerMovement != null)
            {
                Debug.Log("Player picked up: " + _pickupType);
                switch (_pickupType)
                {
                    case PickUpType.Kunai:
                        playerMovement.Kunais += 1;
                        break;
                    case PickUpType.SmokeBomb:
                        playerMovement.SmokeBombs += 1;
                        break;
                }
            }
            // Выбираем, какой звук проигрывать в зависимости от типа
            AudioClip clipToPlay = null;                    // *** Добавлено ***
            switch (_pickupType)                              // *** Добавлено ***
            {                                                // *** Добавлено ***
                case PickUpType.Kunai:                        // *** Добавлено ***
                    clipToPlay = kunaiPickupSound;           // *** Добавлено ***
                    break;                                   // *** Добавлено ***
                case PickUpType.SmokeBomb:                    // *** Добавлено ***
                    clipToPlay = smokeBombPickupSound;       // *** Добавлено ***
                    break;                                   // *** Добавлено ***
            }                                                // *** Добавлено ***

            // Проигрываем звук, если есть
            if (clipToPlay != null && audioSource != null) // *** Добавлено ***
            {
                audioSource.clip = clipToPlay;               // *** Добавлено ***
                audioSource.Play();                           // *** Добавлено ***
                Destroy(gameObject, clipToPlay.length);      // *** Добавлено: уничтожение с задержкой ***
            }
            else
            {
                Destroy(gameObject);                          // если звука нет — уничтожаем сразу
            }
        }
    }
}
