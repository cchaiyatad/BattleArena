using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine;

public class ArenaMultiplayerMenuController : MonoBehaviourPunCallbacks
{
    private string gameVersion = "0";
    private string playerName;
    private string roomName;
    private bool isInRoom;

    public Text connectStatusText;
    public Text roomStatusText;
    public InputField playerInputField;
    public InputField roomInputField;
    public Button startButton;
    public Button randomOrLeaveButton;

    public void ConnectToPhoton()
    {
        print("connect");
        if (PhotonNetwork.IsConnected)
        {
            print("isconnect");
            connectStatusText.text = "Connected";
            SetInteractable(true, true, true, true);
            return;
        }
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        connectStatusText.text = "Connecting...";
    }

    public void OnMultiPlayerMenuClick()
    {
        if (playerInputField.text == "")
        {
            roomStatusText.text = "Enter player name";
            return;
        }
        if (roomInputField.text == "" && !isInRoom)
        {
            roomStatusText.text = "Enter room name";
            return;
        }

        if (!isInRoom)
        {
            playerName = playerInputField.text;
            roomName = roomInputField.text;
            randomOrLeaveButton.GetComponentInChildren<Text>().text = "Leave";
            CreateOrJoinRoom();
        }
        else
        {
            LoadLevel();
        }
    }

    private void CreateOrJoinRoom()
    {
        PhotonNetwork.NickName = playerName;
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = false,
            MaxPlayers = 2
        };
        TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default);
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
    }

    private void LoadLevel()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            PhotonNetwork.LoadLevel("ArenaScene");
        else
            roomStatusText.text = "Waiting for player";
    }

    private void SetInteractable(bool playerInput, bool roomInput,
        bool startButton, bool randomButton)
    {
        playerInputField.interactable = playerInput;
        roomInputField.interactable = roomInput;
        this.startButton.interactable = startButton;
        randomOrLeaveButton.interactable = randomButton;

    }

    private void UpdateStatusText()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            roomStatusText.text = "You are host, waiting for player (" +
                PhotonNetwork.CurrentRoom.PlayerCount + "/2)";
        }
        else
        {
            roomStatusText.text = "Waiting for player/host (" +
                PhotonNetwork.CurrentRoom.PlayerCount + "/2)";
        }
    }

    public override void OnConnected()
    {
        base.OnConnected();
        SetInteractable(true, true, true, true);
        connectStatusText.text = "Connected";
    }

    public override void OnJoinedRoom()
    {
        isInRoom = true;
        roomStatusText.gameObject.SetActive(true);
        SetInteractable(false, false, PhotonNetwork.IsMasterClient, true);
        randomOrLeaveButton.GetComponentInChildren<Text>().text = "Leave";
        UpdateStatusText();
    }

    public void OnRandomButtonClick()
    {
        if (playerInputField.text == "")
        {
            roomStatusText.text = "Enter player name";
            return;
        }
        if (!PhotonNetwork.InRoom)
        {
            playerName = playerInputField.text;
            PhotonNetwork.NickName = playerName;
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("join fail");
        CreateRandomRoom();
    }

    public void OnLeaveButtonClick()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("create Fail");
        if (roomInputField.text == "")
        {
            CreateRandomRoom();
        }
        else
        {
            roomStatusText.text = "Room name is used";
        }
    }

    private void CreateRandomRoom()
    {
        int roomNumber = Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions
        {
            IsVisible = true,
            MaxPlayers = 2
        };
        PhotonNetwork.CreateRoom("Room" + roomNumber, roomOptions);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        isInRoom = false;
        roomStatusText.text = "You already leave the room";
        randomOrLeaveButton.GetComponentInChildren<Text>().text = "Random";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdateStatusText();
    }

    

}
