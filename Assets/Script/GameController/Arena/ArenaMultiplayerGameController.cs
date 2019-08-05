using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaMultiplayerGameController : ArenaGameController
{
    public GameObject hitArea;

    private void Awake()
    {

        if (PlayerPrefs.GetInt("mode") == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            HPUI.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    void Start()
    {

        skills = new Skill().generateSkill();

        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("MainMenuScene");
            return;
        }
        //photonView = GetComponent<PhotonView>();
        if (MultiplayerPlayerScript.Instance == null)
        {
            GameObject player;
            if (PhotonNetwork.IsMasterClient)
            {

                print("Master");
                player = PhotonNetwork.Instantiate("Player", new Vector3(-2, 0, -2), Quaternion.identity);
            }
            else
            {
                print("not master");
                player = PhotonNetwork.Instantiate("Player", new Vector3(2, 0, 2), Quaternion.identity);
            }
            character.Add(player.GetComponent<CharacterBase>());
        }

    }
    protected override void CheckFinish()
    {
        if (character[0].isDead)
        {
            isFinish = true;
            isPause = true;

            if (character[0].isDead)
            {
                pauseText.text = "You lose";
            }
        }
    }

    public void CreateAttack(string attacker, int skillID, Vector3 position, float direction)
    {
        Skill skill;
        Vector3 hitLocation = position;
        if (skillID == -1)
        {
            skill = new Skill();
        }
        else
        {
            skill = skills[skillID - 1];
        }

        hitLocation.y = 0.55f;
        hitLocation.x += Mathf.Sin(Mathf.Deg2Rad * direction);
        hitLocation.z += Mathf.Cos(Mathf.Deg2Rad * direction);
        GameObject hit = PhotonNetwork.Instantiate("hitArea", hitLocation, Quaternion.identity);
        hit.SetActive(false);
        HitAreaScript hitAreaScript = hit.GetComponent<HitAreaScript>();

        hitAreaScript.attacker = attacker;
        hitAreaScript.damage = skill.damage;
        hitAreaScript.time = skill.time;
        hit.gameObject.GetComponent<BoxCollider>().size = new Vector3(skill.size, 0.5f, skill.size);
        hit.transform.localScale = new Vector3(skill.size, 0.5f, skill.size);
        if (skill.moving)
        {
            hit.GetComponent<Rigidbody>().velocity = transform.forward * 8f;
            hit.GetComponent<HitAreaScript>().isMoving = true;
            hit.GetComponent<MeshRenderer>().enabled = true;
        }
        hit.SetActive(true);
    }

    protected override void UpdateSkillAndHPMenu()
    {
        base.UpdateSkillAndHPMenu();
    }

}
