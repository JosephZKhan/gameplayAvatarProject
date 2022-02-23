using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenuScript : MonoBehaviour
{

    GameObject pauseMenu;

    void Awake()
    {
        pauseMenu = gameObject.transform.Find("pauseMenu").gameObject;
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (playerController2.gamePaused)
        {
            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(false);
        }
    }
}
