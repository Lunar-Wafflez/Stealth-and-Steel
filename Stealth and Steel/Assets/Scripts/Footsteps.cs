using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource footsteps, sprint, smokebomb;
    
    private PlayerMovementScript _playerMovementScript;
    private void Start()
    {
        _playerMovementScript = this.GetComponent<PlayerMovementScript>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
        { 
            footsteps.enabled = true;
            //if (Input.GetKey(KeyCode.Space))
            //{
            //    footsteps.enabled = false;
            //    sprint.enabled = true;
            //}
            //else
            //{
            //    footsteps.enabled = true;
            //    sprint.enabled = false;
            //}
            if (Input.GetKeyDown(KeyCode.Space) && _playerMovementScript.SmokeBombs > 0)
            {
                if (!smokebomb.isPlaying)
                {
                    footsteps.enabled = false;
                    sprint.enabled = true;
                    smokebomb.Play();
                    sprint.Play();
                }
                else
                {
                    footsteps.enabled = true;
                    sprint.enabled = false;
                }
            }
        }
        else
        {
            footsteps.enabled = false;
            sprint.enabled = false;
        }
    }
}
