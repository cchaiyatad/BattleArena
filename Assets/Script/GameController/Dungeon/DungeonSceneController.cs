using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DungeonSceneController : MonoBehaviour
{
    public GameObject PauseMenu;
    public Text text;
    public Text CountUI;
    public CharacterBase Player;
    public CharacterBase Enemy;
    public int count;
    public GameObject skillMenu;

    private bool isPause;
    private bool isFinish;
    private List<Vector3> corners = new List<Vector3> { };
    private float nextSpawnTime;
    private float spanwCoolDown;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 size = GameObject.Find("Plane").GetComponent<Renderer>().bounds.size - (2 * Vector3.one);
        Player.playerName = "Player";
        for (int i = -1; i < 2; i += 2)
        {
            for (int j = -1; j < 2; j += 2)
            {
                corners.Add(new Vector3(size.x / 2 * i, 0f, size.z / 2 * j));
            }
        }
		nextSpawnTime = Time.time;
        Player.skills = new Skill().generateSkill();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSkillMenu();
        CountUI.text = "Count : " + count;
        spanwCoolDown = 9f - (0.2f * count);
        if(spanwCoolDown <= 4)
        {
            spanwCoolDown = 5f;
        }

        if (nextSpawnTime < Time.time && !isPause && !isFinish)
        {
            nextSpawnTime += spanwCoolDown;
            Instantiate(Enemy, corners[Random.Range(0, 4)], Quaternion.identity);
        }

        if (isPause & !isFinish)
            Time.timeScale = 0f;

        else
            Time.timeScale = 1f;


        if (!isFinish)
        {
            if (Input.GetKeyDown(KeyCode.P))
                isPause = !isPause;
        }

        if (Player.isDead)
        {
            isFinish = true;
            isPause = true;
            text.text = "Kill: " + count;
            CountUI.gameObject.SetActive(false);
        }

        PauseMenu.SetActive(isPause);

    }

    private void UpdateSkillMenu()
    {
        foreach (Skill skill in Player.skills)
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
}
