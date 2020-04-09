using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class DoDamage :  MonoBehaviour
{
    private GameObject player;
    private BoxCollider2D playerCollider;
    private BoxCollider2D objectCollider;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();
        objectCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerCollider.IsTouching(objectCollider))
        GameObject.FindWithTag("Player").SendMessage("Death");
    }
    


}
