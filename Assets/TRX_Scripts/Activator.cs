using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    private GameObject player;
    private BoxCollider2D playerCollider;
    private CircleCollider2D activatorCollider;
    private CustomCharacterController script;
    [Header("GameObjet associé")]
    [SerializeField] GameObject associatedObject = null;
    [Header("PlaceHolders Graphs associés")]
    [SerializeField] SpriteRenderer Graph1 = null;
    [SerializeField] SpriteRenderer Graph2 = null;
    [SerializeField] SpriteRenderer Graph3 = null;



    private void Start()
    {
        player = GameObject.Find("Character");
        activatorCollider = GetComponent<CircleCollider2D>();
        playerCollider = player.GetComponent<BoxCollider2D>();
        script = player.GetComponent<CustomCharacterController>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (player.GetComponent<CustomCharacterController>().jumpInput && playerCollider.IsTouching(activatorCollider))
        {
            associatedObject.SetActive(true);
            Graph1.enabled = false;
            Graph2.enabled = false;
            Graph3.enabled = false;

        }
    }
}
