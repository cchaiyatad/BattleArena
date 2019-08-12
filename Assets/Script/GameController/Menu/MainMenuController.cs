using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private int index;
    public GameObject menu;
    public GameObject backButton;

    private void Start()
    {
        index = 0;
    }

    private void Update()
    {
        backButton.SetActive(index != 0);

        for (int i = 0; i < menu.transform.childCount; i++)
        {
            menu.transform.GetChild(i).gameObject.SetActive(i == index);
        }
    }

    public void OnPlaySingleArenaButtonClick()
    {
        PlayerPrefs.SetInt("mode", 0);
        SceneManager.LoadScene("ArenaScene");
    }


    public void OnPlayMultiArenaButtonClick()
    {
        PlayerPrefs.SetInt("mode", 1);
        PhotonNetwork.AutomaticallySyncScene = true;


    }

    public void OnPlayDungeonButtonClick()
    {
        SceneManager.LoadScene("DungeonScene");

    }

    public void OnMainMenuBackButtonClick()
    {
        index -= 1;
    }

    public void OnArenaButtonClick()
    {
        index += 1;
    }

}
