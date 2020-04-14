using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{

    private Rigidbody2D rockRigidbody;
    private Vector2 velocity;
    private GameObject player;
    private BoxCollider2D playerCollider;
    //private Transform rOrigin;

    void Start()
    {
        rockRigidbody = gameObject.GetComponentInParent(typeof(Rigidbody2D)) as Rigidbody2D;
        player = GameObject.FindWithTag("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();

    }

    private void OnTriggerEnter2D(Collider2D playerCollider)
    {
        Debug.Log("isTriggered");
        rockRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        velocity = rockRigidbody.velocity;
        velocity += (new Vector2(0, -0.1f));
        rockRigidbody.velocity = velocity;
    }
    
}
