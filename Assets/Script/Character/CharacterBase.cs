using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public int hp = 3;
    public float moveSpeed = 3f;
    public float attackDelay = 3f;

    public GameObject hitArea;

    private float hitDirection;
    private Vector3 hitLocation;
    protected Skill currentSkill;

    protected bool isAttack;
    protected float spawnAttackTime;

    protected bool isUseSkill;
    protected float spawnSkillTime;

    protected bool isSpawnAttack;

    [HideInInspector]
    public List<Skill> skills = new List<Skill> { };
    [HideInInspector]
    public string playerName;
    [HideInInspector]
    public HitAreaScript hitAreaScript;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public float nextAttackTime;
    [HideInInspector]
    public bool isMove;
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public float nextMoveAfterWasHittedTime;
    [HideInInspector]
    public string lastAttacker;

    protected abstract void Move(Vector3 dir);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
            return;

        HitAreaScript hitpointScript = other.GetComponent<HitAreaScript>();
        if (hitpointScript.attacker != playerName && !isDead)
        {
            if (hitpointScript.attacker == "")
                return;

            lastAttacker = hitpointScript.attacker;
            Damaged(hitpointScript.damage);
        }
    }

    protected void Attack()
    {
        nextAttackTime = Time.time + attackDelay;
        animator.SetTrigger("IsAttack");
        isAttack = true;
        spawnAttackTime = Time.time + 0.55f;
        foreach (Skill skill in skills)
        {
            if (skill.nextTime < Time.time)
                skill.nextTime = Time.time;

            skill.nextTime += 0.3f;
        }
    }

    protected void Dead()
    {
        hp = 0;
        isDead = true;
        animator.SetTrigger("IsDeath");
    }

    protected virtual void Damaged(int damage)
    {
        animator.StopPlayback();
        animator.SetTrigger("IsDameged");
        isAttack = false;
        isUseSkill = false;
        nextMoveAfterWasHittedTime = Time.time + 0.8f;

        hp -= damage;
    }

    protected virtual void CharacterBehavior()
    {
        if (hp <= 0 && !isDead)
            Dead();

        SpawnAttack(ref isAttack, ref isSpawnAttack, spawnAttackTime, new Skill());
        SpawnAttack(ref isUseSkill, ref isSpawnAttack, spawnSkillTime, currentSkill);
    }

    protected virtual void SpawnAttack(ref bool check, ref bool spawnAttack, float spawnTime, Skill skill)
    {
        if (check && !spawnAttack && Time.time > spawnTime)
        {
            hitAreaScript.attacker = playerName;
            hitAreaScript.damage = skill.damage;
            hitAreaScript.time = skill.time;
            hitDirection = transform.rotation.eulerAngles.y;
            hitLocation = gameObject.transform.position;
            hitLocation.y = 0.55f;
            hitLocation.x += Mathf.Sin(Mathf.Deg2Rad * hitDirection);
            hitLocation.z += Mathf.Cos(Mathf.Deg2Rad * hitDirection);
            hitArea.gameObject.GetComponent<BoxCollider>().size = new Vector3(skill.size, 0.5f, skill.size);
            hitArea.transform.localScale = new Vector3(skill.size, 0.5f, skill.size);
            GameObject hit = Instantiate(hitArea, hitLocation, Quaternion.Euler(0, hitDirection, 0));
            if (skill.moving)
            {
                hit.GetComponent<Rigidbody>().velocity = transform.forward * 8f;
                hit.GetComponent<HitAreaScript>().isMoving = true;
                hit.GetComponent<MeshRenderer>().enabled = true;
            }
            spawnAttack = true;

        }

        if (check && Time.time > spawnTime + 0.3f)
        {
            check = false;
            spawnAttack = false;
        }

    }

    protected bool CheckCannotMove()
    {
        return nextMoveAfterWasHittedTime > Time.time || isAttack || isUseSkill;
    }

    protected void AttackRotate(Vector3 dir)
    {
        if (dir == Vector3.zero)
            return;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
