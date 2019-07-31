using System.Collections;
using Photon.Pun;
using UnityEngine;

public class MultiplayerPlayerScript : MonoBehaviourPun, IPunObservable
{
    private CameraScript cameraScript;
    private PlayerScript playerScript;
    private Vector3 direction;

    void Start()
    {
        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            playerScript = gameObject.GetComponent<PlayerScript>();
            playerScript.animator = gameObject.GetComponent<Animator>();
            cameraScript = GameObject.Find("Main Camera").GetComponent<CameraScript>();

            cameraScript.Player = gameObject.transform;
            playerScript.isMultiplayer = true;
            playerScript.playerName = PhotonNetwork.NickName;
            playerScript.animator.SetFloat("MoveSpeed", playerScript.moveSpeed);
            playerScript.hitAreaScript = playerScript.hitArea.GetComponent<HitAreaScript>();
        }
    }

    void Update()
    {
        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            playerScript.CheckObstacle();

            if (Time.time > playerScript.nextAttackTime)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playerScript.Attack();
                }
            }
            playerScript.CharacterBehavior();

        }

    }

    void FixedUpdate()
    {
        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            if (playerScript.isDead)
            {
                return;
            }
            playerScript.Move(direction);
            playerScript.AttackRotate(direction);
            return;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (other.CompareTag("Obstacle"))
        {
            return;
        }
        HitAreaScript hitpointScript = other.GetComponent<HitAreaScript>();
        if (hitpointScript.attacker != playerScript.playerName && !playerScript.isDead)
        {
            playerScript.Damaged(hitpointScript.damage);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
