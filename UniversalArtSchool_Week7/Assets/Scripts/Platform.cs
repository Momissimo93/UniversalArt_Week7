using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : Character
{
    [SerializeField] float distanceAlongTheAxis;
    [SerializeField] float initialDirection;
    //[SerializeField] MovementType typeOfMov;
    [SerializeField] Vector2 unitsToMoveBy;

    void Start()
    {
        originalPos = transform.position;
        distance = distanceAlongTheAxis;
        //movementType = typeOfMov;
        direction = initialDirection;
        numOfUnits = unitsToMoveBy;

        if (movementType == MovementType.Diagonal)
        {
            finalPosition = new Vector3(originalPos.x + numOfUnits.x, originalPos.y + numOfUnits.y, 0);
            //Vector3 versor = (finalPosition - originalPos).normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Hero>())
        {
            collision.transform.SetParent(transform);
            Debug.Log("Entered");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Hero>())
        {
            collision.transform.SetParent(null);
            Debug.Log("Exit");
        }
    }
}
