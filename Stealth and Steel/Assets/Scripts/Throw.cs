using UnityEngine;

public class Throw : MonoBehaviour
{
    public AudioSource throww;
    [SerializeField]
    private PlayerMovementScript _playerMovementScript;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _playerMovementScript.Kunais > 0 && !_playerMovementScript._isInDuel)
        {
            if (!throww.isPlaying)
            {
                throww.Play();
            }
        }
    }
}
