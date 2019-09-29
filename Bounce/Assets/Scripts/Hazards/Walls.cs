using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    public ParticleSystem explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 hitPoint = collision.gameObject.transform.position;
            Destroy(collision.gameObject);
            Instantiate(explosion, hitPoint,Quaternion.identity);
        }
    }
}
