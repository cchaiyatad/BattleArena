using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaGameController : MonoBehaviour
{
    public GameObject PauseMenu;
    public Text text;
    public List<CharacterBase> character;
    public List<Skill> skills;
    public GameObject skillMenu;

    private bool isPause;
    private bool isFinish;


    void Awake()
    {
        if (PlayerPrefs.GetInt("mode") == 1)
        {
            GameObject.Find("SingleCharacter").SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            skills = new Skill().generateSkill();
            while (skills.Count > 0)
            {
                int i = Random.Range(0, skills.Count);
                character[character.Count - skills.Count].skills.Add(skills[i]);
                skills.RemoveAt(i);
            }
            
        }
    }

    private void Start()
    {
        character[0].playerName = "Player";
        character[1].playerName = "Enemy 1";
        character[2].playerName = "Enemy 2";
    }

    void Update()
    {
        CheckDead();
        UpdateSkillMenu();
        if (isPause && !isFinish)
            Time.timeScale = 0f;

        else
            Time.timeScale = 1f;


        if (!isFinish)
        {
            if (Input.GetKeyDown(KeyCode.P))
                isPause = !isPause;
        }

        if (character[0].isDead || (character[1].isDead && character[2].isDead))
        {
            isFinish = true;
            isPause = true;

            if (character[0].isDead)
            {
                text.text = "You lose";
            }
            if (character[1].isDead && character[2].isDead)
            {
                text.text = "You win";
            }
            if (character[0].isDead && character[1].isDead && character[2].isDead)
            {
                text.text = "Tie";
            }

        }

        PauseMenu.SetActive(isPause);

    }

    private void UpdateSkillMenu()
    {
        foreach (Skill skill in character[0].skills)
        {
            Text skillText = skillMenu.transform.GetChild(skill.id - 1).gameObject.GetComponent<Text>();
            if (skill.nextTime > Time.time)
            {
                skillText.text = skill.key + " - Cool Down";
            }
            else
            {
                skillText.text = skill.key + " - Ready";
            }
        }
    }

    private CharacterBase FindPlayerWithName(string attackerName)
    {
        foreach (CharacterBase chars in character)
        {
            if (chars.playerName == attackerName)
            {
                return chars;
            }
        }
        return null;
    }

    private void CheckDead()
    {
        foreach (CharacterBase chars in character)
        {

            if (chars.isDead && chars.skills.Count > 0)
            {
                FindPlayerWithName(chars.lastAttacker).skills.AddRange(chars.skills);
                chars.skills.Clear();
            }
        }
    }

}
