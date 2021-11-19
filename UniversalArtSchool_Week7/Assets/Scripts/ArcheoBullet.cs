using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcheoBullet : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    Rigidbody2D rb;
    int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Rigidbody2D>())
        {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Character character = hitInfo.GetComponent<Character>();
        if (character != null && character.name != "Hero")
        {
            character.TakeDamage(1);
            Destroy(this.gameObject);
        }
    }

}
