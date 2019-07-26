using System;
using System.Collections.Generic;

public class Skill
{
    public float coolDown { get; set; }
    public float size { get; set; }
    public float damage { get; set; }
    public float time { get; set; }
    public bool  moving { get; set; }
    public string key;
    public string animation;

    public Skill(string key, string animation)
    {
        coolDown = 8f;
        size = 1;
        damage = 1;
        moving = false;
        time = 0.2f;
        this.key = key;
        this.animation = animation;

    }

    public Skill()
    {
    }

    public List<Skill> generateSkill()
    {
        var skills = new List<Skill> { };

        Skill skill1 = new Skill("U","IsUseSkill1");
        skill1.size = 2f;
        Skill skill2 = new Skill("I", "IsUseSkill2");
        skill2.damage = 2f;
        Skill skill3 = new Skill("O", "IsUseSkill3");
        skill3.moving = true;
        skill3.time = 3;
        skill3.size = 0.2f;
        skills.Add(skill1);
        skills.Add(skill2);
        skills.Add(skill3);

        return skills;

    }

}
