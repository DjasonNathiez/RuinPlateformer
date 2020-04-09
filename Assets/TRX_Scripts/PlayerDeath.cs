using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private GameObject respawnPoint;
    public GameObject currentPlayer;



    void Start()
    {
        respawnPoint = GameObject.FindWithTag("Respawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Death()
    {
        //play Death animation

        //Teleport the player GameObject to the respawn point location
        currentPlayer = GameObject.FindWithTag("Player");
        currentPlayer.transform.position = respawnPoint.transform.position;
        
    }

}
