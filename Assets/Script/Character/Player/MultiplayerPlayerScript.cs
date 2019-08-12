using Photon.Pun;
using UnityEngine;
using static MultiKeys;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MultiplayerPlayerScript : PlayerScript
{
    public ArenaMultiplayerGameController arenaMultiplayerGameController;
    private PhotonView photonView;
    private CameraScript CameraScript;
    private int count;
    private bool isCountDead;

    [PunRPC]
    void SendAttack(string attacker)
    {
        arenaMultiplayerGameController.CreateAttack(attacker);
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        skills = new Skill().generateSkill();
        photonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
        hitAreaScript = hitArea.GetComponent<HitAreaScript>();
        playerName = PhotonNetwork.NickName;
        arenaMultiplayerGameController = GameObject.Find("GameController/MultiPlayerGameController").GetComponent<ArenaMultiplayerGameController>();

        if (photonView.IsMine)
        {
            CameraScript = Camera.main.GetComponent<CameraScript>();
            CameraScript.Player = transform;

            string allPlayer = GetValueByKey<string>("", ALLPLAYERKEY);
            SetValueByKey("", ALLPLAYERKEY, allPlayer + playerName + "|");

            Hashtable hash = new Hashtable
            {
                { playerName + COUNTKEY, count },
                { playerName + ISDEADKEY, count },
                { SURVIVORCOUNTKEY,PhotonNetwork.CurrentRoom.PlayerCount },
                { playerName + SKILLIDKEY, -1 },
                { playerName + ATTACKPOSTIONXKEY, 0f },
                { playerName + ATTACKPOSTIONZKEY, 0f },
                { playerName + ATTACKDIRECTIONKEY, 0f },
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }
    }

    private void Update()
    {

        if (!photonView.IsMine)
            return;


        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        CheckObstacle();

        if (Time.time > nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Attack();
        }

        currentSkill = UseSkill();

        if (arenaMultiplayerGameController.isFinish)
        {
            count = GetValueByKey<int>(playerName, COUNTKEY);
            arenaMultiplayerGameController.SetFinishGameText(count, lastAttacker);
        }

        CharacterBehavior();

        if (isDead)
        {
            arenaMultiplayerGameController.SetFinishGameText(count, lastAttacker);
            if (!isCountDead)
            {
                isCountDead = true;
                SetOtherCount(lastAttacker, COUNTKEY);
                count = GetValueByKey<int>(playerName, COUNTKEY);
                SetValueByKey(playerName, ISDEADKEY, isDead);
                SetSurvivorCount(-1);
            }
        }

    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        if (isDead)
            return;

        Move(direction);
        AttackRotate(direction);
    }

    public override void SpawnAttack(ref bool check, float spawnTime, Skill skill)
    {
        
        if (check && Time.time > spawnTime)
        {
            if (skill.id < 4)
            {
                SetValueByKey(playerName, SKILLIDKEY, skill.id);
                SetValueByKey(playerName, ATTACKPOSTIONXKEY, transform.position.x);
                SetValueByKey(playerName, ATTACKPOSTIONZKEY, transform.position.z);
                SetValueByKey(playerName, ATTACKDIRECTIONKEY, transform.rotation.eulerAngles.y);
                photonView.RPC("SendAttack", RpcTarget.All, playerName);
            }
            check = false;
        }
    }

    private void SetOtherCount(string otherPlayerName, string key)
    {
        int otherPlayerKillCount = GetValueByKey<int>(otherPlayerName, key);
        SetValueByKey(otherPlayerName, key, otherPlayerKillCount + 1);
    }

    private void SetSurvivorCount(int i)
    {
        int newCount = GetValueByKey<int>("", SURVIVORCOUNTKEY);
        SetValueByKey("", SURVIVORCOUNTKEY, newCount + i);
    }

}