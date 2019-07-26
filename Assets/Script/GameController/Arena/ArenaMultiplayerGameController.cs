using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaMultiplayerGameController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private List<GameObject> players = new List < GameObject >{};

   
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

        
        players.Add(PhotonNetwork.Instantiate("player", Vector3.zero, Quaternion.identity));
        
        
    }


}
