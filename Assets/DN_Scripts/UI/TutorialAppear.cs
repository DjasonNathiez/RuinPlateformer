using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAppear : MonoBehaviour
{
    public GameObject Tuto;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tutorial"))
        {
            Tuto.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Tuto.SetActive(false);
        }
    }

}
