using UnityEngine;
using UnityEngine.AI;

public class EnemyFootsteps : MonoBehaviour
{
    private NavMeshAgent _agent;       
    public AudioSource FootstepsSFX;      
    public float speedThreshold = 0.1f;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); 
    }

    void Update()
    {     
        if (_agent.velocity.magnitude > speedThreshold)
        {
            
            if (!FootstepsSFX.isPlaying)
                FootstepsSFX.Play();
        }
        else
        {
            if (FootstepsSFX.isPlaying)
                FootstepsSFX.Stop();
        }
    }
}
