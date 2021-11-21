using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private float inputHorizontal;
    private float inputVertical;
    Rigidbody2D rb;
    public LayerMask ladder;
    bool isClimbing = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputHorizontal * speed, rb.velocity.y);

        RaycastHit2D rd = Physics2D.Raycast(transform.position, Vector2.up, 0.5f, ladder);
        if(rd)
        {
            Debug.Log("A ladder");

            if(rd.collider != null)
            {
                if(Input.GetKeyDown(KeyCode.UpArrow))
                {
                    isClimbing = true;
                }
            }
 
        }
        if (isClimbing == true && rd.collider != null)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2 (rb.velocity.x, inputVertical * speed);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 5;
        }
    }
}
