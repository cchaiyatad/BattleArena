using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemyScript : EnemyScript
{
    public DungeonSceneController script;
    private bool isCount;

    void Awake()
    {
        target = GameObject.Find("Player");
        
        script = GameObject.Find("GameController").GetComponent<DungeonSceneController>();
    }

    public override void CharacterBehavior()
    {
        if (hp == 0 && !isAlreadyDead)
        {
            Dead();
            if (!isCount)
            {
                script.count += 1;
                isCount = true;
            }

            isAlreadyDead = true;
            Destroy(gameObject, 3f);

        }   
    }
}

