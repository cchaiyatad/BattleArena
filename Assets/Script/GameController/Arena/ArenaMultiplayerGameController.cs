using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaMultiplayerGameController : MonoBehaviourPunCallbacks
{  
    private void Awake()
    {
        if (PlayerPrefs.GetInt("mode") == 0)
        {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("MainMenuScene");
            return;
        }
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);

    }


}
