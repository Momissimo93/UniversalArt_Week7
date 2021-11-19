using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject mainChar;
    SpriteRenderer spriteRend;
    Camera camera;
    public GameObject backGround;

    float verExtent;
    float horExtent;

    float spriteWidth;
    float spriteHeight;

    float leftB;
    float rightB;
    float topB;
    float bottomB;

    [SerializeField] float interpolationSpeed = 10f;
    [SerializeField] Vector2 offset;

    void Start()
    {
        if (backGround.GetComponent<SpriteRenderer>())
        {
            spriteRend = backGround.GetComponent<SpriteRenderer>();
            //spriteSizeMax = sprite.bounds.size.x/2  ;
            //Debug.Log("spriteSizeMax = " + spriteSizeMax);
        }
        else
        {
            Debug.Log("The object does not have a SpriteRendere");
        }

        if (GetComponent<Camera>())
        {
            //Debug.Log("Camera detected");

            camera = GetComponent<Camera>();

            verExtent = camera.orthographicSize;
            horExtent = verExtent * camera.aspect;
            //Debug.Log("Camera orthographicSize is " + verExtent);
            //Debug.Log("Camera cameraWidht is " + horExtent);

            spriteWidth = spriteRend.bounds.size.x / 2f;
            spriteHeight = spriteRend.bounds.size.y / 2f;
            //Debug.Log("spriteWidth" + spriteWidth);
            //Debug.Log("spriteHeight" + spriteHeight);

            leftB = spriteRend.bounds.min.x + horExtent - offset.x;
            rightB = spriteRend.bounds.max.x - horExtent - offset.x;

            bottomB = spriteRend.bounds.min.y + verExtent - offset.y;
            topB = spriteRend.bounds.max.y - verExtent - offset.y;

            //Debug.Log("leftB " + leftB);
            //Debug.Log("rightB" + rightB);
            //Debug.Log("bottomB" + bottomB);
            //Debug.Log("topB" + topB);
        }
        else
        {
            //Debug.Log("Camera NOT detected");
        }

    }
    // Update is called once per frame
    void Update()
    {
        if(mainChar)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(Mathf.Clamp(mainChar.transform.position.x, leftB, rightB) + offset.x, Mathf.Clamp(mainChar.transform.position.y, bottomB, topB) + offset.y, transform.position.z), Time.deltaTime * interpolationSpeed);
        }

        //transform.position = new Vector2 (mainChar.transform.position.x , mainChar.transform.position.y);
    }
}
