using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnPlaySingleArenaButtonClick()
    {
        PlayerPrefs.SetInt("mode", 0);
        SceneManager.LoadScene("ArenaScene");

    }

    public void OnPlayDungeonButtonClick()
    {
        SceneManager.LoadScene("DungeonScene");
    }

    public void OnRestartButtonClick()
    {
        if(PlayerPrefs.GetInt("mode") == 1)
        {
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

}
