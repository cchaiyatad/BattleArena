using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine;

public class ArenaMultiplayerMenuController : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    private string playerName;
    private string roomName;
    private bool isInRoom;

    public Text connectStatusText;
    public Text roomStatusText;
    public InputField playerInputField;
    public InputField roomInputField;
    public Button button;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        
    }

    void Start()
    {
        PlayerPrefs.SetInt("mode", 1);
        ConnectToPhoton();
    }

    public void OnMultiPlayerMenuClick()
    {
        if (!isInRoom)
        {
            playerName = playerInputField.text;
            roomName = roomInputField.text;

            CreateOrJoinRoom();
        }
        else
        {
            LoadLevel();
        }
    }

    private void ConnectToPhoton()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        connectStatusText.text = "Connecting...";
        connectStatusText.gameObject.gameObject.SetActive(true);
    }

    private void CreateOrJoinRoom()
    {
        PhotonNetwork.NickName = playerName;
        RoomOptions roomOptions = new RoomOptions();
        TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default);
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
    }

    private void LoadLevel()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            PlayerPrefs.SetInt("mode", 1);
            PhotonNetwork.LoadLevel("ArenaScene");
        }
        else
        {
            roomStatusText.text = "Waiting for player";
        }
    }

    public override void OnConnected()
    {
        base.OnConnected();
        connectStatusText.text = "Connected";
    }

    public override void OnJoinedRoom()
    {
        isInRoom = true;
        roomStatusText.gameObject.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            roomStatusText.text = "You are host, waiting for player (" +
                PhotonNetwork.CurrentRoom.PlayerCount + "/2)";
        }
        else
        {
            button.enabled = false;
            roomStatusText.text = "Waiting for player (" +
                PhotonNetwork.CurrentRoom.PlayerCount + "/2)";
        }
    }
    
}
