﻿using UnityEngine;

public class ArenaEnemyScript : EnemyScript
{
    protected override void CharacterBehavior()
    {
        currentSkill = SkillAI();
        base.CharacterBehavior();
    }

    protected Skill SkillAI()
    {
        if (isUseSkill)
            return currentSkill;

        foreach (Skill skill in skills)
        {
            if (skill.nextTime > Time.time)
                continue;

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
                    UseSkill(skill);

                return skill;
            }
        }

        return new Skill();
    }

    protected void UseSkill(Skill skill)
    {
        isUseSkill = true;
        animator.SetTrigger(skill.animation);
        skill.nextTime = Time.time + skill.coolDown;
        spawnSkillTime = Time.time + 0.7f;
    }
}
