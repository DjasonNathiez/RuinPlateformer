using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulsCristal : MonoBehaviour
{
    private GameObject player;
    private BoxCollider2D playerCollider;
    private CircleCollider2D cristalCollider;
    private CustomCharacterController script;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Character");
        cristalCollider = GetComponent<CircleCollider2D>();
        playerCollider = player.GetComponent<BoxCollider2D>();
        script = player.GetComponent<CustomCharacterController>();
    }

     private void FixedUpdate()
     {
         if (player.GetComponent<CustomCharacterController>().jumpInput && playerCollider.IsTouching(cristalCollider))
         {
             script.StartCoroutine("Impulse");
         }

     }
     
    public void OnTriggerEnter2D(Collider2D collision)
    {
        script.canJump = false;
    }

      public void OnTriggerExit2D(Collider2D collision)
    {
        script.canJump = true;
    }
}




