
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    /// <summary>
    /// The damage to deal to entity
    /// </summary>
    protected float startingDistance;
    public float damageToDeal;
    [SerializeField]
    protected float targetRadius;
    [SerializeField]
    protected float fallSpeed;
    [SerializeField]
    private LayerMask[] layersToHurt;
    private bool hasLanded = false;

    // Start is called before the first frame update
    void Awake()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, startingDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLanded) //if the hazard has landed
        {
            foreach (LayerMask layer in layersToHurt)
            {
                if (ColInCircle(transform.position, targetRadius, layer, out RaycastHit2D[] hits))
                {
                    foreach (RaycastHit2D hit in hits)
                    {
                        //deal damage to every gameobject in hits
                    }
                }
            }
        }
        else //if the hazard is falling down
        {
            transform.position += Vector3.forward * (fallSpeed * Time.deltaTime);

            hasLanded = (transform.position.z >= 1);
        }
    }

    protected bool ColInCircle(Vector2 origin, float radius, int layer, out RaycastHit2D[] hits)
    {
        hits = Physics2D.CircleCastAll(origin, radius, Vector2.zero, 1, layer);
        return (hits.Length > 0);
    }
}
