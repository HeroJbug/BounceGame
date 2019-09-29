using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created in Advance to handle enemy collision with various hazards
public class EnemyCollision : MonoBehaviour
{
    Rigidbody rBody;

    private void Start()
    {
        rBody = this.GetComponent<Rigidbody>();
    }

}
