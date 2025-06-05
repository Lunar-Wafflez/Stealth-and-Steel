using UnityEngine;

public class Throw : MonoBehaviour
{
    public AudioSource throww;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!throww.isPlaying)
            {
                throww.Play();
            }
        }
    }
}
