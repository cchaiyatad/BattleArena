using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemyScript : EnemyScript
{
    public DungeonSceneController gameControllerScript;
    private bool isCount;

    void Awake()
    {
        target = GameObject.Find("Player");
        gameControllerScript = GameObject.Find("GameController").GetComponent<DungeonSceneController>();
        playerName = "Enemy";
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
            }

            gameObject.SetActive(false);
            isDead = false;
            hp = 2;
            transform.position = gameControllerScript.corners[Random.Range(0, gameControllerScript.corners.Count)];
            isCount = false;

            //Destroy(gameObject, 3f);
        }
        SpawnAttack(ref attackState, spawnAttackTime, new Skill());

    }
}

