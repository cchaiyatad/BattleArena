
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DungeonSceneController : GameController
{
    public Text CountUI;
    public CharacterBase Enemy;
    public int count;

    private List<Vector3> corners = new List<Vector3> { };
    private float nextSpawnTime;
    private float spanwCoolDown;


    void Start()
    {
		character[0].playerName = "Player";
        character[0].skills = new Skill().generateSkill();
        Vector3 size = GameObject.Find("Plane").GetComponent<Renderer>().bounds.size - (2 * Vector3.one);
        for (int i = -1; i < 2; i += 2)
        {
            for (int j = -1; j < 2; j += 2)
            {
                corners.Add(new Vector3(size.x / 2 * i, 0f, size.z / 2 * j));
            }
        }
		nextSpawnTime = Time.time;
        
    }

    
    void Update()
    {
        UpdateSkillAndHPMenu();
        Pause();
        CheckFinish();

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
    }

    protected override void CheckFinish()
    {
        if (character[0].isDead)
        {
            isFinish = true;
            isPause = true;
            pauseText.text = "Kill: " + count;
            CountUI.gameObject.SetActive(false);
        }
    }
}
