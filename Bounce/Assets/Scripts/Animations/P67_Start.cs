using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P67_Start : MonoBehaviour
{
    Rigidbody2D p67Body;
    public float startSpeed,bounceBack,boostDelay,boostScale,finalDrag;
    public Sprite[] sprites;
    void Start()
    {
        p67Body = GetComponent<Rigidbody2D>();
    }

    public void MoveRight()
    {
        p67Body.AddRelativeForce(startSpeed * new Vector2(1, 0), ForceMode2D.Impulse);
        GetComponent<Image>().sprite = sprites[0];
        transform.localScale = new Vector3(boostScale, boostScale, boostScale);
        Invoke("BigBoost", boostDelay);
    }

    public void BigBoost()
    {
        GetComponent<Image>().sprite = sprites[1];
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        p67Body.AddRelativeForce(bounceBack * new Vector2(-1, 0), ForceMode2D.Impulse);
        GetComponent<Image>().sprite = sprites[2];
        transform.localScale = new Vector3(1,1,1);
        p67Body.drag = finalDrag;
    }
}
