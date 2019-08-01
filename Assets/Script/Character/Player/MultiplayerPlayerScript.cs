using System.Collections;
using Photon.Pun;
using UnityEngine;

public class MultiplayerPlayerScript : PlayerScript
{
    public ArenaMultiplayerGameController arenaMultiplayerGameController;
    private PhotonView photonView;
    private CameraScript CameraScript;
    public static GameObject Instance;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
        hitAreaScript = hitArea.GetComponent<HitAreaScript>();
        playerName = PhotonNetwork.NickName;
        arenaMultiplayerGameController = GameObject.Find("GameController/MultiPlayerGameController").GetComponent<ArenaMultiplayerGameController>();

        if (photonView.IsMine)
        {
            if (Instance == null)
            {
                Instance = gameObject;
            }
            CameraScript = Camera.main.GetComponent<CameraScript>();
            CameraScript.Player = transform;
        }

        photonView.RPC("ReceivedMessage", RpcTarget.All, playerName);

    }
    [PunRPC]
    void ReceivedMessage(string a)
    {
        Debug.Log(a + "  " + playerName);
    }


    [PunRPC]
    void ReceivedAttackLocation(string attacker, int skillID, Vector3 position, float direction)
    {
        Debug.Log(attacker + " " + skillID + " " + position + " " + direction);
        arenaMultiplayerGameController.CreateAttack(attacker, skillID, position, direction);
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        CheckObstacle();

        if (Time.time > nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
        }
        currentSkill = UseSkill();
        CharacterBehavior();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (isDead)
        {
            return;
        }
        Move(direction);
        AttackRotate(direction);
    }

    public override void SpawnAttack(ref bool check, float spawnTime, Skill skill)
    {

        if (check && Time.time > spawnTime)
        {
            photonView.RPC("ReceivedAttackLocation", RpcTarget.All,
                    playerName, skill.id, transform.position, transform.rotation.eulerAngles.y);
            check = false;
        }

    }
}
