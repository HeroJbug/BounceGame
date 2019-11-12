using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P67_Start : MonoBehaviour
{
    Rigidbody2D p67Body;
    public float startSpeed,bounceBack,finalDrag;
    public Sprite finalSprite;
    void Start()
    {
        p67Body = GetComponent<Rigidbody2D>();
    }

    public void MoveRight()
    {
        p67Body.AddRelativeForce(startSpeed * new Vector2(1, 0), ForceMode2D.Impulse);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        p67Body.AddRelativeForce(bounceBack * new Vector2(-1, 0), ForceMode2D.Impulse);
        GetComponent<Image>().sprite = finalSprite;
        p67Body.drag = finalDrag;
    }
}
