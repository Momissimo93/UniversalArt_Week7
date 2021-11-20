using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    // Start is called before the first frame update

    Guns revolver;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.name == "Hero")
        {
            //Destroy(gameObject);
        }
    }

    public Guns getGun()
    {
        return revolver;
    }
}
