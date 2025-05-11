using UnityEngine;

public class KunaiLogic : MonoBehaviour
{
    [SerializeField]
    private LayerMask _enemyLayerMask;
    [SerializeField]
    private float _duration = 5.0f;
    private float _timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if ( _timer >= _duration )
            Destroy( gameObject );
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (((1 << collision.gameObject.layer) & _enemyLayerMask) != 0)
        {
            Debug.Log("Kunai hit an enemy!");
            Destroy(collision.gameObject);  
            Destroy(gameObject); 
        }
    }
}
