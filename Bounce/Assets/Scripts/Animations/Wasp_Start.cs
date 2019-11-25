using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wasp_Start : MonoBehaviour
{
    Rigidbody2D waspBody;
    public float knockback;
    // Start is called before the first frame update
    void Start()
    {
        waspBody = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
		SoundSystem.system.PlaySFX("EnemyBoom1", 1);
		waspBody.AddRelativeForce(knockback * new Vector2(1, 0), ForceMode2D.Impulse);
        Invoke("Finish", 1f);
    }

    private void Finish()
    {
        Destroy(this.gameObject);
    }
}
