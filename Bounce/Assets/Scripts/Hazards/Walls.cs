using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
			//Vector3 hitPoint = collision.gameObject.transform.position;
			collision.gameObject.GetComponent<Enemy>().OnCollisionDeath();
        }
    }
}
