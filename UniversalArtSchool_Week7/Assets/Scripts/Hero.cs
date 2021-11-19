using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    [SerializeField] int jumpForce;
    [SerializeField] List<Guns> guns = new List<Guns>();
    //[SerializeField] GameObject bullet;
    //[SerializeField] Transform firePoint;

    private bool isFiring;
    private float speedLocalReference;

    private void Awake()
    {
        GetAnimator();
        GetBoxCollider();
    }
    void Start()
    {
        facingForward = true;
        speedLocalReference = speed;

        //maxLifePoints can be set and can be see in the inspector
        //Initially the player lifePoints will be equal to that of the maxLifePoints
        //The healthbar is then updated

        lifePoints = maxLifePoints;
        healthBar.SetMaxHealth(maxLifePoints);
        healthBar.SetHealth(maxLifePoints);
    }

    void Update()
    {
        checkIfButtonDownPressed();
        EmittingRaysFromFeet();
    }

    private void FixedUpdate()
    {
        checkIfButtonPressed();
        if (immune)
        {
            Blinking();
        }
    }

    void checkIfButtonPressed()
    {
        if (Input.GetButton("Horizontal") && (!isFiring))
        {
            speed = speedLocalReference;
            SetHeroRotation();
            direction = Input.GetAxis("Horizontal");
            animator.SetFloat("speed", speed);

            Move();
        }
        else
        {
            animator.SetFloat("speed", 0);
            StopMoving();
        }
    }

    void checkIfButtonDownPressed()
    {
        if (Input.GetButtonDown("Fire1") && HasWeapon())
        {
            animator.SetTrigger("fire");
            isFiring = true;
            Shoot(bullet, firePoint);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            isFiring = false;
        }

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            animator.SetTrigger("jump");
            Jump(jumpForce);
        }
    }
    void SetHeroRotation()
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
    private void Shoot(GameObject b, Transform spwaningBulletPosition)
    {
        GameObject bullet = Instantiate(b, spwaningBulletPosition.position, spwaningBulletPosition.rotation);
       
    }

    private bool HasWeapon()
    {
        if (guns.Count != 0)
        {
            Debug.Log("We have a weapon");
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Weapon"))
        {
            Guns g = c.gameObject.GetComponent<Revolver>().getGun();
            guns.Add(g);
            ShowGuns();
            Destroy(c.gameObject);
        }
    }

    private void ShowGuns()
    {
        Debug.Log("This are the gun that we have collected");
        for (int i = 0; i < guns.Count; i++)
        {
            Debug.Log(guns[i].gunType);
        }
    }
}
