using UnityEngine;

public class PlayerScript : CharacterBase
{
    private bool isHitObstacle;
    protected Vector3 direction;
    protected Skill currentSkill;
    public bool isUseSkill;

    private float usedSkillTime;


    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
        hitAreaScript = hitArea.GetComponent<HitAreaScript>();
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        CheckObstacle();

        if (Time.time > nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
        }
        currentSkill = UseSkill();
        CharacterBehavior();
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        Move(direction);
        AttackRotate(direction);
    }
    public override void CharacterBehavior()
    {
        SpawnAttack(ref isUseSkill, ref isSpawnAttack,usedSkillTime, currentSkill);
        base.CharacterBehavior();
        
    }
    public void CheckObstacle()
    {
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.8f))
        {
            isHitObstacle = hit.transform.CompareTag("Obstacle");
        }
        else
        {
            isHitObstacle = false;
        }

    }

    public void AttackRotate(Vector3 dir)
    {
        if (dir == Vector3.zero)
        {
            return;
        }
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public override void Move(Vector3 dir)
    {
        isMove = (dir != Vector3.zero);
        animator.SetBool("IsMove", isMove);


        if (dir == Vector3.zero || attackState || isHitObstacle || isUseSkill)
        {
            return;
        }

        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);

    }

    public Skill UseSkill()
    {
        if (isUseSkill)
        {
            return currentSkill;
        }
        foreach (Skill skill in skills)
        {
            if (Input.GetKeyDown(skill.key))
            {
                if (skill.nextTime < Time.time)
                {
                    isUseSkill = true;
                    animator.SetTrigger(skill.animation);
                    skill.nextTime = Time.time + skill.coolDown;
                    usedSkillTime = Time.time + 0.7f;
                    nextAttackTime = Time.time + 0.3f;
                    return skill;
                }
            }
        }
        
        return new Skill();
    }
}
