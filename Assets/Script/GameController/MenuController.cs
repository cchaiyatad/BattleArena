﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnPlayDungeonButtonClick()
    {
        SceneManager.LoadScene("DungeonScene");
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

}