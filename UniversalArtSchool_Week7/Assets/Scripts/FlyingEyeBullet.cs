using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeBullet : MonoBehaviour
{
    /*
    public float speed;
    private Transform player;
    private Vector2 target;
    */

    public float speed;
    Rigidbody2D rb;
    Hero target;
    Vector2 moveDirection;
  

    private void Start()
    {
        //player  = GameObject.FindGameObjectWithTag("Hero").transform;
        //target = new Vector2(player.position.x, player.position.y);

        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindObjectOfType<Hero>();
        moveDirection = (target.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        /*
        transform.position = Vector2.MoveTowards(transform.position,target,speed * Time.deltaTime);
         if(transform.position.x ==  target.x && transform.position.y == target.y)
         {
             DestroyProjectile();
         }*/
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        /*
        Character character = hitInfo.GetComponent<Character>();
        if (character != null && character.name == "Hero")
        {
            character.TakeDamage(1);
            Destroy(gameObject);
        }*/
        Character character = hitInfo.GetComponent<Character>();
        if (character != null && character.name == "Hero")
        {
            character.TakeDamage(1);
            Destroy(gameObject);
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
