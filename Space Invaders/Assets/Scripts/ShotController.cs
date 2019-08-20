using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public float _speed;
    
    void Update()
    {
        //We move the bullet until it collides with the screen limit
        if(transform.position.y < 4.5)
        {
            transform.position += new Vector3(0, _speed * Time.deltaTime, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Detect if the bullet collide with the base
        if(collision.gameObject.tag == "Base")
        {
            Destroy(gameObject);
        }
    }
}
