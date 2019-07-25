﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemyScript : EnemyScript
{
    public DungeonSceneController script;
    private bool isCount;

    void Awake()
    {
        UI = null;
        target = GameObject.Find("Player");
        
        script = GameObject.Find("GameController").GetComponent<DungeonSceneController>();
    }

    protected override void CharacterBehavior()
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
