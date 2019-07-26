using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaGameController : MonoBehaviour
{
    public GameObject PauseMenu;
    public Text text;
    public List<CharacterBase> character;
    public List<Skill> skills;

    private bool isPause;
    private bool isFinish;


    void Awake()
    {
        if (PlayerPrefs.GetInt("mode") == 1)
        {
            GameObject.Find("Player").SetActive(false);
            GameObject.Find("Enemy1").SetActive(false);
            GameObject.Find("Enemy2").SetActive(false);
            gameObject.SetActive(false);

        }
        else
        {
            skills = new Skill().generateSkill();
            while(skills.Count > 0)
            {
                int i = Random.Range(0, skills.Count);
                character[character.Count - skills.Count].skills.Add(skills[i]);
                print(character[character.Count - skills.Count].name + " " + skills[i].key);
                skills.RemoveAt(i);
            }

        }

    }

    private void Start()
    {
        character[0].playerName = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause && !isFinish)
            Time.timeScale = 0f;

        else
            Time.timeScale = 1f;


        if (!isFinish)
        {
            if (Input.GetKeyDown(KeyCode.P))
                isPause = !isPause;
        }

        if (character[0].hp == 0 || character[1].hp == 0)
        {
            isFinish = true;
            isPause = true;
            if (character[0].hp == 0)
            {
                text.text = "You lose";
            }
            if (character[1].hp == 0)
            {
                text.text = "You win";
            }
        }

        PauseMenu.SetActive(isPause);

    }
}
