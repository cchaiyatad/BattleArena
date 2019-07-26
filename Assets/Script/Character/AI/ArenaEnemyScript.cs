using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ArenaEnemyScript : EnemyScript
{
    [SerializeField]
    private Text UI;

    public override void Damaged(int damage)
    {
        base.Damaged(damage);
        UI.text = playerName + " HP: " + hp;
    }

}
