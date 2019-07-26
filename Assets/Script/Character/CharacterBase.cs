using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterBase : MonoBehaviour
{

    public int hp = 3;
    public float moveSpeed = 3f;
    public float attackDelay = 3f;

    public GameObject hitArea;

    [HideInInspector]
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
    public bool isAlreadyDead;

    private float hitDirection;
    private Vector3 hitLocation;

    public abstract void Move(Vector3 dir);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            return;
        }
        HitAreaScript hitpointScript = other.GetComponent<HitAreaScript>();
        if (hitpointScript.attacker != playerName && !isAlreadyDead)
        {
            Damaged(hitpointScript.damage);
        }
    }

    public void Attack()
    {
        nextAttackTime = Time.time + attackDelay;
        animator.SetTrigger("IsAttack");
        StartCoroutine(SpawnAttack());
    }

    public void Dead()
    {
        hp = 0;
        animator.SetTrigger("IsDeath");
        isAlreadyDead = true;
    }

    public virtual void Damaged(int damage)
    {
        animator.StopPlayback();
        animator.SetTrigger("IsDameged");

        hp -= damage;
        StartCoroutine(DamagedDelay());
    }

    public virtual void CharacterBehavior()
    {
        if (hp <= 0 && !isAlreadyDead)
            Dead();
    }

    IEnumerator SpawnAttack()
    {
        attackState = true;
        hitAreaScript.attacker = playerName;
        hitAreaScript.damage = 1;
        yield return new WaitForSeconds(0.5f);
        hitDirection = transform.rotation.eulerAngles.y;
        hitLocation = gameObject.transform.position;
        hitLocation.y = 0.5f;
        hitLocation.x += Mathf.Sin(Mathf.Deg2Rad * hitDirection);
        hitLocation.z += Mathf.Cos(Mathf.Deg2Rad * hitDirection);
        Instantiate(hitArea, hitLocation, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        attackState = false;
    }

    IEnumerator DamagedDelay()
    {
        attackState = true;
        yield return new WaitForSeconds(0.3f);
        attackState = false;
    }
}
