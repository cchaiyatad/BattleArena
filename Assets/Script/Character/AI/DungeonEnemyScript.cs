using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemyScript : EnemyScript
{
    public DungeonSceneController gameControllerScript;
    private bool isCount;
    private float disappearTime;

    void Awake()
    {
        playerName = "Enemy";
        gameControllerScript = GameObject.Find("GameController").GetComponent<DungeonSceneController>();
        target = gameControllerScript.character[0].gameObject;
    }

    public override void CharacterBehavior()
    {
        if (hp <= 0 && !isDead)
        {
            Dead();
            if (!isCount)
            {
                gameControllerScript.count += 1;
                isCount = true;
                disappearTime = Time.time + 3f;
            }

        }

        if (isDead && Time.time > disappearTime)
        {
            hp = 2;
            isDead = false;
            isCount = false;
            gameObject.SetActive(false);
            transform.position = gameControllerScript.corners[Random.Range(0, gameControllerScript.corners.Count)];
        }

        SpawnAttack(ref attackState, spawnAttackTime, new Skill());

    }
}

