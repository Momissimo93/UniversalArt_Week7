using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private void Awake()
    {
        GetAnimator();
        GetBoxCollider();
    }

    private void Start()
    {
        facingForward = true;
        initialPosition = transform.position;
        initialMovementType = movementType;
        initialSpeed = speed;

        timeBtwShots = startTimeBtwShots;
       
        lifePoints = maxLifePoints;
        healthBar.SetMaxHealth(maxLifePoints);
        healthBar.SetHealth(maxLifePoints);

        //           patrolling = true;
    }
    // Update is called once per frame
    void Update()
    {
        EmittingRaysFromFeet();
        EnemyBehaviour();

        if (movementType != MovementType.GoBackToInitialPosition)
            EmittingEyeSight();

        Move();
    }

    private void FixedUpdate()
    {
        if (immune)
        {
            Blinking();
        }
    }
}
