using UnityEngine;

public class TextRotation : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_camera.transform);
        transform.Rotate(0,180,0);
    }
}
