using System.Collections.Generic;
using UnityEngine;

public class ArenaGameController : GameController
{
    
    private List<Skill> skills;

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
        UpdateSkillAndHPMenu();
        Pause();
        CheckFinish();
    }

    protected override void CheckFinish()
    {
        if (character[0].isDead || (character[1].isDead && character[2].isDead))
        {
            isFinish = true;
            isPause = true;

            if (character[0].isDead)
            {
                pauseText.text = "You lose";
            }
            if (character[1].isDead && character[2].isDead)
            {
                pauseText.text = "You win";
            }
            if (character[0].isDead && character[1].isDead && character[2].isDead)
            {
                pauseText.text = "Tie";
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
