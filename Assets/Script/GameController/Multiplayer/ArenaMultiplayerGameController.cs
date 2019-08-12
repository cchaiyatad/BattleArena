using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MultiKeys;

public class ArenaMultiplayerGameController : ArenaGameController
{
    private string[] multiplayerLists;
    public GameObject hitArea;
    public Text killCountText;
    public Text killByText;


    private void Awake()
    {
        if (PlayerPrefs.GetInt("mode") == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            HPUI.transform.GetChild(1).gameObject.SetActive(false);
            HPUI.transform.GetChild(2).gameObject.SetActive(false);
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

        GameObject player;

        if (PhotonNetwork.IsMasterClient)
        {
            player = PhotonNetwork.Instantiate("Player", new Vector3(-2, 0, -2), Quaternion.identity);
        }
        else
        {
            player = PhotonNetwork.Instantiate("Player", new Vector3(2, 0, 2), Quaternion.identity);
        }
        character.Add(player.GetComponent<CharacterBase>());

    }

    private void Update()
    {
        UpdateSkillAndHPMenu();
        Pause();
        CheckFinish();
    }

    protected override void CheckFinish()
    {
        if (character[0].isDead)
        {
            isFinish = true;
            isPause = true;
            pauseText.text = "You Die";
            return;
        }

        int survivorCount = GetValueByKey<int>("", SURVIVORCOUNTKEY);

        if (survivorCount == 1)
        {
            if (character[0].isDead)
                return;
            
            isFinish = true;
            isPause = true;
            killByText.gameObject.SetActive(false);
        }

    }

    public void CreateAttack(string attacker)
    {
        Skill skill;
        int skillID = GetValueByKey<int>(attacker, SKILLIDKEY);
        float xPosition = GetValueByKey<float>(attacker, ATTACKPOSTIONXKEY);
        float zPosition = GetValueByKey<float>(attacker, ATTACKPOSTIONZKEY);
        float direction = GetValueByKey<float>(attacker, ATTACKDIRECTIONKEY);
        Vector3 hitLocation = new Vector3(xPosition, 0, zPosition);
        Vector3 moveDirection = new Vector3(Mathf.Sin(Mathf.Deg2Rad * direction), 0,
            Mathf.Cos(Mathf.Deg2Rad * direction));

        if (skillID == -1)
            skill = new Skill();
        else
            skill = skills[skillID - 1];

        hitLocation.y = 0.55f;
        hitLocation += moveDirection;
        GameObject hit = Instantiate(hitArea, hitLocation, Quaternion.Euler(0, direction, 0));
        HitAreaScript hitAreaScript = hit.GetComponent<HitAreaScript>();

        hitAreaScript.attacker = attacker;
        hitAreaScript.damage = skill.damage;
        hitAreaScript.time = skill.time;
        print(hitAreaScript.attacker);
        hit.gameObject.GetComponent<BoxCollider>().size = new Vector3(skill.size, 0.5f, skill.size);
        hit.transform.localScale = new Vector3(skill.size, 0.5f, skill.size);
        if (skill.moving)
        {
            hit.GetComponent<Rigidbody>().velocity = moveDirection * 8f;
            hit.GetComponent<HitAreaScript>().isMoving = true;
            hit.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    protected override void Pause()
    {
        pauseMenu.SetActive(isPause);
    }

    public void SetFinishGameText(int count, string attackername)
    {
        killCountText.text = "You kill " + count + " people";
        killByText.text = attackername + " kill you";
    }
}
