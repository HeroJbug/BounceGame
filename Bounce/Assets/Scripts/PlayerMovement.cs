using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(speed*Input.GetAxis("Horizontal"), speed*Input.GetAxis("Vertical"), 0));
    }
}
