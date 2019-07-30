using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public float nextTime { get; set; }
    public float coolDown { get; set; }
    public float size { get; set; }
    public int damage { get; set; }
    public float time { get; set; }
    public bool  moving { get; set; }
    public bool isUse { get; set; }
    public KeyCode key;
    public string animation;

    public Skill()
    {
        nextTime = Time.time;
        coolDown = 8f;
        size = 0.8f;
        damage = 1;
        moving = false;
        isUse = false;
        time = 0.2f;

    }


    public Skill(string key, string animation) : this()
    {
        this.animation = animation;
        this.key = (KeyCode) Enum.Parse(typeof(KeyCode), key);
    }

   
    public List<Skill> generateSkill()
    {
        var skills = new List<Skill> { };

        Skill skill1 = new Skill("U", "IsUseSkill1")
        {
            size = 1.6f
        };
        Skill skill2 = new Skill("I", "IsUseSkill2")
        {
            damage = 2
        };
        Skill skill3 = new Skill("O", "IsUseSkill3")
        {
            moving = true,
            time = 3,
            size = 0.4f
        };
        skills.Add(skill1);
        skills.Add(skill2);
        skills.Add(skill3);

        return skills;

    }

}
