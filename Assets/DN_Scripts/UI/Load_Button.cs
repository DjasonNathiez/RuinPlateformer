using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_Button : MonoBehaviour
{

    public GameObject Settings;

    public void ButtonStart()
    {
        SceneManager.LoadScene(1);
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }

    public void ButtonSetting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Settings.SetActive(true);

        }

        if (Input.GetButtonDown("Cancel"))
        {
            Settings.SetActive(false);
        }
    }
}
