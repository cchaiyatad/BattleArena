using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterBase : MonoBehaviour
{
    public int hp = 3;
    public float moveSpeed = 3f;
    public float attackDelay = 3f;

    public GameObject hitArea;

    public List<Skill> skills = new List<Skill> { };
    //[HideInInspector]
    public string playerName;
    [HideInInspector]
    public HitAreaScript hitAreaScript;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public bool attackState;
    [HideInInspector]
    public float nextAttackTime;
    [HideInInspector]
    public bool isMove;
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public string lastAttacker;

    private float hitDirection;
    private Vector3 hitLocation;

    public abstract void Move(Vector3 dir);

    public float spawnAttackTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            return;
        }
        HitAreaScript hitpointScript = other.GetComponent<HitAreaScript>();
        if (hitpointScript.attacker != playerName && !isDead)
        {
            if (hitpointScript.attacker == "")
            {
                return;
            }
            print(hitpointScript.attacker + " " + playerName);
            lastAttacker = hitpointScript.attacker;
            Damaged(hitpointScript.damage);
        }
    }

    public void Attack()
    {
        nextAttackTime = Time.time + attackDelay;
        animator.SetTrigger("IsAttack");
        attackState = true;
        spawnAttackTime = Time.time + 0.55f;
        foreach (Skill skill in skills)
        {
            if (skill.nextTime < Time.time)
            {
                skill.nextTime = Time.time;
            }
            skill.nextTime += 0.3f;
        }
    }

    public void Dead()
    {
        hp = 0;
        isDead = true;
        animator.SetTrigger("IsDeath");

    }

    public virtual void Damaged(int damage)
    {
        animator.StopPlayback();
        animator.SetTrigger("IsDameged");

        hp -= damage;
    }

    public virtual void CharacterBehavior()
    {
        if (hp <= 0 && !isDead)
            Dead();

        SpawnAttack(ref attackState, spawnAttackTime, new Skill());

    }

    public virtual void SpawnAttack(ref bool check, float spawnTime, Skill skill)
    {
        if (check && Time.time > spawnTime)
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
            GameObject hit = Instantiate(hitArea, hitLocation, Quaternion.identity);
            if (skill.moving)
            {
                hit.GetComponent<Rigidbody>().velocity = transform.forward * 8f;
                hit.GetComponent<HitAreaScript>().isMoving = true;
                hit.GetComponent<MeshRenderer>().enabled = true;
            }
            check = false;
        }

    }
}
