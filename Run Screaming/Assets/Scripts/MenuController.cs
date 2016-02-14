﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour {

    

    public void LoadScene(string maze)
    {
        SceneManager.LoadScene(maze);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
