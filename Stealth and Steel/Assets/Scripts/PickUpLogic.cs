using Unity.VisualScripting;
using UnityEngine;

public class PickUpLogic : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private PickUpType _pickupType;
    [SerializeField] 
    enum PickUpType
    {
        Kunai,
        SmokeBomb
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
                switch(_pickupType)
                {
                    case PickUpType.Kunai:
                        playerMovement.Kunais += 1;
                        break;
                    case PickUpType.SmokeBomb:
                        playerMovement.SmokeBombs += 1;
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
