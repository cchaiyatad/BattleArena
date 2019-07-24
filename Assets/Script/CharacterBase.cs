using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterBase : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackDelay = 3f;
    public int hp = 3;
    public GameObject hitArea;
    [HideInInspector] public HitAreaScript hitAreaScript;
    public Text UI;
    public bool isHitObstacle;

    public bool attackState;
    public bool isDamaged;
    public float hitDirection;

    public Animator animator { get; set; }
    public float nextAttackTime { get; set; }
    public bool isMove { get; set; }
    public bool isAlreadyDead { get; set; }

    private Vector3 hitLocation;

    protected abstract void Move(Vector3 dir);
    
    protected abstract void CheckObstacle();

    protected void Attack()
    {
        nextAttackTime = Time.time + attackDelay;
        animator.SetTrigger("IsAttack");
        StartCoroutine(SpawnAttack());
    }

    protected void Dead()
    {
        animator.SetTrigger("IsDeath");
    }

    protected void Damaged()
    {
        animator.StopPlayback();

        animator.SetTrigger("IsDameged");
        hp -= 1;
        StartCoroutine(DamagedDelay());
    }

    protected void OnTriggerEnter(Collider other)
    {
        isDamaged = !other.CompareTag("Obstacle") &&
            !gameObject.CompareTag(other.GetComponent<HitAreaScript>().attacker);
    }


    protected virtual void CharacterBehavior()
    {
        if (isDamaged && !isAlreadyDead)
        {
            Damaged();
            isDamaged = false;
            UI.text = gameObject.tag + " HP: " + hp;

        }

        if (hp == 0 && !isAlreadyDead)
        {
            Dead();
            isAlreadyDead = true;

        }

    }

    IEnumerator SpawnAttack()
    {
        attackState = true;
        hitAreaScript.attacker = gameObject.tag;
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
