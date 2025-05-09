using UnityEngine;

public class CameraTestScript : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.position;
        transform.rotation = _target.rotation;

    }
}
