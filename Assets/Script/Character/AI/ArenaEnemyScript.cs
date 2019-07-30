using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ArenaEnemyScript : EnemyScript
{
    [SerializeField]
    private Text UI;
    private Skill currentSkill;
    private float usedSkillTime;

    public override void Damaged(int damage)
    {
        base.Damaged(damage);
        UI.text = playerName + " HP: " + hp;
    }

    public override void CharacterBehavior()
    {
        currentSkill = SkillAI();
        SpawnAttack(ref isUseSkill, usedSkillTime, currentSkill);
        base.CharacterBehavior();
    }

    public Skill SkillAI()
    {
        if (isUseSkill)
        {
            return currentSkill;
        }
        foreach (Skill skill in skills)
        {
            if (skill.nextTime > Time.time)
            {
                continue;
            }
            float range = -1;
            switch (skill.id)
            {
                case 1:
                    range = 1.4f;
                    break;
                case 2:
                    range = 0.8f;
                    break;
                case 3:
                    range = 3f;
                    break;
            }


            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    UseSkill(skill);
                }
                return skill;
            }
        }

        return new Skill();
    }

    private void UseSkill(Skill skill)
    {
        isUseSkill = true;
        animator.SetTrigger(skill.animation);
        skill.nextTime = Time.time + skill.coolDown;
        usedSkillTime = Time.time + 0.7f;
        //return skill;
    }
}
