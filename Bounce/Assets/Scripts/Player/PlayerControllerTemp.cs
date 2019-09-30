using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTemp : MonoBehaviour
{
    public float moveSpeed = 5f;
    Rigidbody2D rbody;
    Vector2 moveVec;
	Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveVec.x = Input.GetAxisRaw("Horizontal");
        moveVec.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
		velocity = moveVec * moveSpeed * Time.deltaTime;
		rbody.MovePosition(rbody.position + velocity);
    }

	public Vector2 Velocity
	{
		get
		{
			return new Vector2(velocity.x, velocity.y);
		}
	}

}
