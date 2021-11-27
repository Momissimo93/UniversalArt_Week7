using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //----------------------------------------------VARIABLES 

    //----------Protected variables 

    [SerializeField] protected float speed;
    [SerializeField] protected float direction;
    [SerializeField] protected float directionWhenClimbing;
    [SerializeField] protected int maxLifePoints;
    [SerializeField] protected float rayLenghtFromFeet;
    [SerializeField] protected float radius;
    [SerializeField] protected GameObject eyeSightCenterPoint;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected HealthBarBehaviour healthBar;
    protected MovementType initialMovementType;

    protected Animator animator;

    protected BoxCollider2D boxCollider2D;
    protected Rigidbody2D rb;

    protected Vector2 leftFoot;
    protected Vector2 rightFoot;
    protected Vector3 initialPosition;

    //For platfrom movement specifically
    protected Vector2 originalPos;
    protected Vector2 finalPosition;
    protected Vector2 numOfUnits;

    protected float initialSpeed;
    protected float distance;

    protected bool isOnGround;
    protected bool facingForward;
    protected bool goingtBackToInitialPos;
    protected bool directionChange = false;
    protected bool immune = false;
    protected bool isClimbing = false;

    //----------Public variables 

    public float startTimeBtwShots;
    public Transform target;

    public enum MovementType {Patrolling, MoveTowardsTarget, GoBackToInitialPosition, StopMoving, Diagonal, Vertical };
    [SerializeField] protected MovementType movementType;

    //----------Private variables 

    //Variables used to set the fire rate of the enemy
    private protected float timeBtwShots;

    private float rangeRadius;
    private float counter;

    private Vector3 rangeOrigin;

    private int ground = 1 << 6;
    private int enemyLayer = 1 << 7;
    private int bulletLayer = 1 << 8;
    private int heroLayer = 1 << 9;
    //I need to use it in the Hero class
    protected int ladder = 1 << 10;
    //The current life points 
    private protected int lifePoints;

    //----------------------------------------------METHODS
   
    //--Movements
    public void Move()
    {
        switch (movementType)
        {
            case MovementType.Patrolling:
                Patrolling();
                break;

            case MovementType.MoveTowardsTarget:
                MoveTowardsTarget();
                break;

            case MovementType.GoBackToInitialPosition:
                GoBackToInitialPosition();
                break;

            case MovementType.StopMoving:
                StopMoving();
                break;

            case MovementType.Vertical:
                VerticalMovement();
                break;

            case MovementType.Diagonal:
                DiagonalMovement();
                break;

            /*case MovementType.HeroMovement:
               HeroMovement();
               break;*/

            default: break;
        }
    }
    public void Patrolling()
    {
        transform.position = new Vector2(transform.position.x + (speed * direction * Time.deltaTime), transform.position.y);
    }
    private void MoveTowardsTarget()
    {
        speed = initialSpeed;
        if (target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
    private void GoBackToInitialPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
        if (transform.position == initialPosition)
        {
            Debug.Log("I back at initial position");
            movementType = initialMovementType;
        }
    }
    private protected void StopMoving()
    {
        speed = 0;
        Rigidbody2D rb;

        if (GetComponent<Rigidbody2D>())
        {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x * speed, rb.velocity.y);
        }
    }
    private void VerticalMovement()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + (speed * direction * Time.deltaTime));
        if (transform.position.y >= originalPos.y + distance || transform.position.y <= originalPos.y)
        {
            direction *= -1;
        }
    }
    private void DiagonalMovement()
    {
        Vector3 versor = (finalPosition - originalPos).normalized;
        transform.Translate(versor * speed * Time.deltaTime * direction);

        if (transform.position.x >= finalPosition.x || transform.position.x <= originalPos.x)
        {
            direction *= -1;
        }
    }
    //--Emitters
    protected void EmittingEyeSight()
    {
        rangeOrigin = SetOrigin();
        rangeRadius = SetRadius();
        //LookingForHeros();
    }
    protected void EmittingRaysFromFeet()
    {
        leftFoot = new Vector2(boxCollider2D.bounds.min.x, boxCollider2D.bounds.min.y);
        rightFoot = new Vector2(boxCollider2D.bounds.max.x, boxCollider2D.bounds.min.y);

        RaycastHit2D leftFootRays = Physics2D.Raycast(leftFoot, Vector2.down, rayLenghtFromFeet, ground);
        RaycastHit2D rightFootRays = Physics2D.Raycast(rightFoot, Vector2.down, rayLenghtFromFeet, ground);

        CheckIfGround(leftFootRays, rightFootRays);
        DrawRaysFromFeet();
    }

    protected void EnemyBehaviour()
    {
        if (LookForTarget() && !IsInRange(target))
        {
            if (speed != initialSpeed)
            {
                speed = initialSpeed;
                animator.SetFloat("flyingEye_speed", speed);
            }
            movementType = MovementType.MoveTowardsTarget;
        }
        else if (LookForTarget() && IsInRange(target))
        {
            movementType = MovementType.StopMoving;
            Attack();
        }
        else if (movementType != MovementType.Patrolling)
        {
            if (speed != initialSpeed)
            {
                speed = initialSpeed;
                animator.SetFloat("flyingEye_speed", speed);
            }

            movementType = MovementType.GoBackToInitialPosition;
        }
        else
            movementType = MovementType.Patrolling;

    }

    //--Checks
    private void CheckIfGround(RaycastHit2D lFRays, RaycastHit2D rFRays)
    {
        if ((!lFRays) || (!rFRays))
        {
            isOnGround = false;

            if ((gameObject.name == "Flying_Eye") && (movementType == MovementType.Patrolling))
            {
                //Chiedere al prof come nantenere l'angolo dell z fermo in rotazione

                transform.Rotate(0f, 180f, 0f);

                direction = direction * -1;
            }
            //direction *= direction - 1;
        }
        else
            isOnGround = true;
    }

    //--Get
    protected void GetAnimator()
    {
        if (GetComponent<Animator>())
        {
            animator = gameObject.GetComponent<Animator>();
        }
    }
    protected void GetBoxCollider()
    {
        if (GetComponent<BoxCollider2D>())
        {
            boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        }
    }

    //--Set
    private Vector2 SetOrigin()
    {
        Vector3 center;

        if (eyeSightCenterPoint)
        {
            center = eyeSightCenterPoint.transform.position;
            return center;
        }
        else
            //Is a correct way?
            return Vector2.zero;
    }
    private float SetRadius()
    {
        float r;

        if (eyeSightCenterPoint)
        {
            r = radius;
            return r;
        }
        else
        {
            return 0;
        }
    }

    //--Actions
    private protected void Jump(int jF)
    {
        int jumpForce = jF;
        Rigidbody2D rb;
        if (GetComponent<Rigidbody2D>())
        {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    public void TakeDamage(int damage)
    {
        if(!immune)
        {
            Debug.Log("lifePoints =" + lifePoints);
            Debug.Log("Damage =" + damage);
            //The lifePoints dimisnishes according to the damage taken and the healthbar is updated
            lifePoints -= damage;
            healthBar.SetHealth(lifePoints);

            //We check if we still have life
            if (lifePoints <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine("Immunity", 2f);
            }
        }
    }
    IEnumerator Immunity(float seconds)
    {
        immune = true;

        for (int i = 0; i < seconds; i++)
        {
            //it runs 3 times and at each iteration it stops for a second --> so in total the characters will blink for 3 seconds
            yield return new WaitForSeconds(1f);
        }
        RestoreRightAlpha();
        immune = false;
    }    
    protected void Blinking()
    { 
        if (gameObject.GetComponent<SpriteRenderer>())
        {
            SpriteRenderer spriteRenderer;
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Color tempColor = spriteRenderer.color;

            if (spriteRenderer.color.a == 0)
            {
                tempColor.a = 1f;
                spriteRenderer.color = tempColor;
            }

            else if (spriteRenderer.color.a == 1)
            {
                tempColor.a = 0f;
                spriteRenderer.color = tempColor;
            }
        }
    }
    public void RestoreRightAlpha()
    {
        if (gameObject.GetComponent<SpriteRenderer>())
        {
            SpriteRenderer spriteRenderer;
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Color tempColor = spriteRenderer.color;

            if (spriteRenderer.color.a == 0)
            {
                tempColor.a = 1f;
                spriteRenderer.color = tempColor;
                Debug.Log("Alpha Restored");
            }
            else
                Debug.Log("There is no need for restoring Alpha Restored");
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    private bool LookForTarget()
    {
        RaycastHit2D range = Physics2D.CircleCast(rangeOrigin, rangeRadius, Vector2.zero, 1, heroLayer);

        if(range)
        {
            if (range.collider.gameObject.CompareTag("Hero"))
            {
                target = range.collider.transform;
                return true;
            }
        }
        else
        {
            return false;
        }

        return false;
    }
    private bool IsInRange(Transform targ)
    {
        RaycastHit2D targetLine = Physics2D.Linecast(transform.position, targ.position, (heroLayer));
        DrawTargetLine(targetLine);

        float distance = Vector2.Distance(transform.position, targ.position);
        //Debug.Log(distance);
        if (distance < 2)
        {
            return true;
        }
        else
            return false;
    }
    private void Attack()
    {
        animator.SetFloat("flyingEye_speed", speed);

        if (timeBtwShots <= 0)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    //-------Drawing Rays,Line and Circles 
    private void DrawRaysFromFeet()
    {
        Debug.DrawRay(leftFoot, Vector2.down * rayLenghtFromFeet, Color.blue);
        Debug.DrawRay(rightFoot, Vector2.down * rayLenghtFromFeet, Color.blue);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rangeOrigin, rangeRadius);
    }
    private void DrawTargetLine(RaycastHit2D r)
    {

        Debug.DrawLine(transform.position, r.collider.gameObject.transform.position, Color.yellow);
    }

    //Old methods: i keep them as i want to have a sort of bin where i can come back to easily

    /*public void HeroMovement()
    {
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
        }
        if (isClimbing == true && rayFromHead.collider != null)
        {
            Debug.Log("Let's Climb" + direction);
            //verticalDirection = Input.GetAxisRaw("Vertical");
            rb.gravityScale = 0;
           //b.velocity = new Vector2(rb.velocity.x, verticalDirection * speed);
        }
        else
        {
            rb.gravityScale = 5;
        }

    }*/
    /*private void LookingForHeros()
    {
        //Aggiungi il layer ground
        //RaycastHit2D range = Physics2D.CircleCast(rangeOrigin, rangeRadius, Vector2.zero, 1, ~(enemyLayer + bulletLayer));
        //RaycastHit2D range = Physics2D.CircleCast(rangeOrigin, rangeRadius, Vector2.zero, 1, (Hero)); // This is the best way, remeber to create the layer of the hero, like that he will only lookl into the hero layer and get read of all the rest
        //RaycastHit2D [] arrayOfElements = Physics2D.CircleCastAll(rangeOrigin, rangeRadius, Vector2.zero, 1, ~(enemyLayer + bulletLayer));

        /*for (int i = 0; i < arrayOfElements.Length; i ++)
        {
            if(arrayOfElements [i].collider.gameObject.GetComponent<Hero>())
            {
                target = arrayOfElements[i].collider.transform;
                DrawTargetLine(arrayOfElements[i]);
                movementType = MovementType.MoveTowardsTarget;
                //goingtBackToInitialPos = false;

                if (IsInRange(target))
                {
                    speed = 0;
                    animator.SetFloat("flyingEye_speed", speed);
                    //Debug.Log("I found him");

                    if (timeBtwShots <= 0)
                    {
                        Instantiate(bullet, transform.position, Quaternion.identity);
                        timeBtwShots = startTimeBtwShots;
                        //Debug.Log("Shoot");
                    }
                    else
                    {
                        timeBtwShots -= Time.deltaTime;
                    }
                }
                else if (!IsInRange(target))
                {
                    speed = initialSpeed;
                    animator.SetFloat("flyingEye_speed", speed);
                    movementType = MovementType.MoveTowardsTarget;
                }
            }
            else
            {
                goingtBackToInitialPos = true;
                //if (goingBackmovementType = MovementType.GoBackToInitialPosition;
                goingtBackToInitialPos = true;
                //Da rivedere con il prof 
                speed = initialSpeed;
                animator.SetFloat("flyingEye_speed", speed);
            }

        }

        /////
       
        if (range)
        {
            if (range.collider.gameObject.CompareTag("Hero"))
            {
                //Debug.Log("From this distance i can hit the Hero");
                //RaycastHit2D target = Physics2D.Linecast(transform.position, range.collider.gameObject.transform.position, ~(enemyLayer + bulletLayer));
                target = range.collider.transform;
                DrawTargetLine(range);
                movementType = MovementType.MoveTowardsTarget;

                if (IsInRange(target))
                {
                    speed = 0;
                    animator.SetFloat("flyingEye_speed", speed);
                    //Debug.Log("I found him");

                    if (timeBtwShots <= 0)
                    {
                        Instantiate(bullet, transform.position, Quaternion.identity);
                        timeBtwShots = startTimeBtwShots;
                        //Debug.Log("Shoot");
                    }
                    else
                    {
                        timeBtwShots -= Time.deltaTime;
                    }
                }
                else if (!IsInRange(target))
                {
                    speed = initialSpeed;
                    animator.SetFloat("flyingEye_speed", speed);
                    movementType = MovementType.MoveTowardsTarget;
                }
            }
            else if ((transform.position != initialPosition) && (movementType == MovementType.MoveTowardsTarget || movementType == MovementType.GoBackToInitialPosition))
            {
                movementType = MovementType.GoBackToInitialPosition;

                //Da rivedere con il prof 
                speed = initialSpeed;
                animator.SetFloat("flyingEye_speed", speed);
            }
        }
    }*/
    /*public void HeroMovement()
{
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
    }
    if (isClimbing == true && rayFromHead.collider != null)
    {
        Debug.Log("Let's Climb" + direction);
        //verticalDirection = Input.GetAxisRaw("Vertical");
        rb.gravityScale = 0;
       //b.velocity = new Vector2(rb.velocity.x, verticalDirection * speed);
    }
    else
    {
        rb.gravityScale = 5;
    }

}*/
    /*private void CheckIfLadder(RaycastHit2D rayFromHead)
{
    if (rayFromHead.collider != null)
    {
        canClimb = true;
        Debug.Log("There is a ladder");
    }
    else
    {
        canClimb = false;
        Debug.Log("Let's not climb");
    }
}*/
}
