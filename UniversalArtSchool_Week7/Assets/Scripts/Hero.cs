using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    [SerializeField] int jumpForce;

    /*
     This List was created for the exercise about "struct". It contains a list of Guns.
     In game the player can collect guns that will inflict different types of damages to his enemies 
    */
    [SerializeField] List<Guns> guns = new List<Guns>();

    /*
     This Dictionary was created for the exercise about "Dictionary". It contains Reliques. This will give the player special abilities
     For now they one will make the player jumping higer and the other will give the player a "whip"
    */
    [SerializeField] Dictionary<string,Reliques> invenotory = new Dictionary<string,Reliques>();

    private bool isFiring;
    private bool hasWhip = false;

    private Guns gunSelected;
    private int inventoryIndex = -1;

    private void Awake()
    {
        GetAnimator();
        GetBoxCollider();
    }
    void Start()
    {
        facingForward = true;

        /*
            maxLifePoints can be set and can be see in the inspector
            Initially the player lifePoints will be equal to that of the maxLifePoints
            The healthbar is then updated
        */
        lifePoints = maxLifePoints;
        healthBar.SetMaxHealth(maxLifePoints);
        healthBar.SetHealth(maxLifePoints);

        if(gameObject.GetComponent<Rigidbody2D>())
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
    }
    void Update()
    {
        CheckIfButtonDownPressed();
        EmittingRaysFromFeet();
        //EmittingRayFromHead();
    }
    private void FixedUpdate()
    {
        CheckIfButtonPressed();
    
        if (immune)
        {
            Blinking();
        }
    }
    private void CheckIfButtonPressed()
    {
        RaycastHit2D rayFromHead = Physics2D.Raycast(transform.position, Vector2.up, 0.4f, ladder);
        Debug.Log("IsClimbing " + isClimbing);
        Debug.Log("RayFromHead.collider" + rayFromHead.collider);

        if (Input.GetButton("Horizontal") && !isFiring)
        {
            direction = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(speed * direction, rb.velocity.y);
            SetHeroRotation();
            animator.SetFloat("speed", speed);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }

        if (Input.GetButton("Vertical"))
        {
            if (rayFromHead.collider != null)
            {
                if (Input.GetButton("Vertical"))
                {
                    isClimbing = true;
                }
            }
            else if(isOnGround)
            {
                isClimbing = false;
            }
        }
        if (isClimbing == true && rayFromHead.collider != null)
        {
            directionWhenClimbing = Input.GetAxisRaw("Vertical");
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, directionWhenClimbing * speed);
            //Debug.Log("I am Climbing");
            //Debug.Log("IsClimbing " + isClimbing);
            //Debug.Log("RayFromHead.collider" + rayFromHead.collider);
        }
        else 
        {
            rb.gravityScale = 5;
            if(isOnGround)
            {
                isClimbing = false;
            }
        }
    }
    private void CheckIfButtonDownPressed()
    {
        if (Input.GetButtonDown("Fire1") && HasWeapon())
        {
            animator.SetTrigger("fire");
            isFiring = true;

            SetBulletType(bullet);

            Shoot(bullet, firePoint);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            isFiring = false;
        }

        else if (Input.GetButtonDown("Jump") && isOnGround)
        {
            animator.SetTrigger("jump");
            Jump(jumpForce);
        }

        //With r the player can change the type of gun that he has collected and a stored in the guns list 
        else if (Input.GetKeyDown("r"))
        {
            if (HasWeapon())
            {
                if (inventoryIndex > guns.Count - 1)
                {
                    inventoryIndex = 0;
                }
                gunSelected = guns[inventoryIndex];
                inventoryIndex++;

                Debug.Log("The Gun that we have selected is " + gunSelected.gunName);
            }
            else
                Debug.Log("I do not have any weapon yet");
        }
        else if (Input.GetKeyDown("f"))
        {
            if (hasWhip)
            {
                animator.SetTrigger("whip");
            }
            else
                Debug.Log("I do not have the weap yet");
        }
    }
    private void Shoot(GameObject b, Transform spwaningBulletPosition)
    {
        GameObject bullet = Instantiate(b, spwaningBulletPosition.position, spwaningBulletPosition.rotation);
    }
    private bool HasWeapon()
    {
        //Method used to see if we have ever found a gun or not yet
        if (guns.Count != 0)
        {
            Debug.Log("We have a weapon");
            return true;
        }
        return false;
    }
    private void OnCollisionEnter2D(Collision2D c)
    {
        /*
          Once we collide with an object we check if it is a Guns_Manager game object or a Item_Manager game object.
          The first is stored into a list; but before we want to check if the player has a gun or not; if the 
          answer is NO then the player is going to be equiped with a Gun
        */

        if (c.gameObject.GetComponent<Guns_Manager>())
        {
            Guns g = c.gameObject.GetComponent<Guns_Manager>().getGun();

            //If this is the first gun that we collect we set it as the equiped weapon  
            if (inventoryIndex == -1)
            {
                guns.Add(g);
                inventoryIndex = 0;
                gunSelected = guns[inventoryIndex];
            }
            else
            {
                guns.Add(g);
            }
            ShowGuns();
            Destroy(c.gameObject);
        }

        else if (c.gameObject.GetComponent<Item_Manager>())
        {
            Reliques r = c.gameObject.GetComponent<Item_Manager>().getRelique();
            invenotory.Add(r.reliqueName, r);
            GetPowerUp(r);
            ShowInventoryItems();
            Destroy(c.gameObject);
        }
        else
        {
            Debug.Log("Useless staff");
        }
    }
    private void SetHeroRotation()
    {
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            if (facingForward)
            {
                transform.Rotate(0, 180f, 0f);
                facingForward = false;
            }
        }
        else if (Input.GetKey("d") || Input.GetKey("right"))
        {
            if (!facingForward)
            {
                transform.Rotate(0f, 180f, 0f);
                facingForward = true;
            }
        }
    }
    private void SetBulletType(GameObject b)
    {
        //Depending on the Gun that we have selected we change the attribute of the ArcheoBullet that is going to be spawn
        if (b.gameObject.GetComponent<ArcheoBullet>())
        {
            b.gameObject.GetComponent<ArcheoBullet>().SetBulletAttributs(gunSelected);
        }
    }
    private void GetPowerUp (Reliques r)
    {
        //This is a sort of power up manager. It receive the newly obtaine relique and check what type of power up will give to our player
        if(r.GetPowerUpType() == "JumpingPower")
        {
            jumpForce += r.powerAttribute;
        }
        else if(r.GetPowerUpType() == "Whip")
        {
            hasWhip = true;
        }
    }
    private void ShowGuns()
    {
        Debug.Log("This are thes gun that we have collected");
        for (int i = 0; i < guns.Count; i++)
        {
            Debug.Log(guns[i].gunName);
        }
    }
    private void ShowInventoryItems()
    {
        Debug.Log("The element stored in the item are: ");
        foreach (KeyValuePair<string, Reliques> kvp in invenotory)
        {
            Debug.Log("The name of the item is: " + kvp.Key + ". And it is of type : "+ kvp.Value);
        }
    }

    //Old Methods

    /*pivate void CheckIfButtomPressed()
    {
        Questo funziona 
        direction = Input.GetAxisRaw("Horizontal");
        Debug.Log("direction: " + direction);
        rb.velocity = new Vector2(speed * direction, rb.velocity.y);

        RaycastHit2D rayFromHead = Physics2D.Raycast(transform.position, Vector2.up, 0.4f, ladder);

        if (rayFromHead)
        {
            //Debug.Log("A ladder");
            //Debug.Log("The name is " + rayFromHead.collider.gameObject.name);

            if (rayFromHead.collider != null)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    //Debug.Log("I am climbing");
                    isClimbing = true;
                }
            }
            else
            {
                isClimbing = false;
            }
        }
        if (isClimbing == true && rayFromHead.collider != null)
        {
            Debug.Log("Let's Climb" + direction);
            vertical = Input.GetAxisRaw("Vertical");
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 5;
        }

        ///

        if ((Input.GetButton("Horizontal")  && (!isFiring)))
        {
            //speed = speedLocalReference;
            SetHeroRotation();
            direction = Input.GetAxisRaw("Horizontal");
            animator.SetFloat("speed", speed);
            Move();
        }
        else if ((Input.GetButton("Vertical")))
        {
            Debug.Log("Move Vertically");
            //speed = speedLocalReference;
            SetHeroRotation();
            vertical = Input.GetAxis("Vertical");
            Move();
            //Climb();
        }
        else if (Input.GetButton("Vertical"))
        {
            speed = speedLocalReference;
            direction = Input.GetAxis("Vertical");
            animator.SetFloat("speed", speed);
            Move();
        }

        else
        {
            animator.SetFloat("speed", 0);
            //StopMoving();
        }
    }*/
}
