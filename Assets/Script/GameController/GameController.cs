using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class GameController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject skillMenu;
    public GameObject HPUI;
    public Text pauseText;
    public List<CharacterBase> character;

    protected bool isPause;
    public bool isFinish;

	protected abstract void CheckFinish();

	protected virtual void UpdateSkillAndHPMenu()
    {

        for (int i = 0; i < character.Count; i++)
        {
            Text hptext = HPUI.transform.GetChild(i).GetComponent<Text>();
            hptext.text = character[i].playerName + " HP : " + character[i].hp;
        }

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

    protected virtual void Pause()
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

        pauseMenu.SetActive(isPause);

    }

}
