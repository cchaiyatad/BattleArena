using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackDelay = 3f;
    public int hp = 3;
    public GameObject hitArea;
    [HideInInspector] public HitAreaScript hitAreaScript;

    public byte attackState;
    public bool isDamaged;
    public float hitDirection;

    public Animator animator { get; set; }
    public float nextAttackTime { get; set; }
    public bool isMove { get; set; }
    public bool isAlreadyDead { get; set; }

    protected abstract void Move(Vector3 dir);

    protected abstract void AttackRotate(Vector3 dir);

    protected void Attack()
    {
        nextAttackTime = Time.time + attackDelay;
        animator.SetTrigger("IsAttack");
    }

    protected void Dead()
    {
        animator.SetTrigger("IsDeath");
    }

    protected void Damaged()
    {
        //animator.StopPlayback();
        animator.SetTrigger("IsDameged");
        hp -= 1;
    }

    protected void OnTriggerEnter(Collider other)
    {
        isDamaged = other.GetComponent<HitAreaScript>().attacker != gameObject.tag;
    }

    protected void CharacterBehavior()
    {
        if (isDamaged && !isAlreadyDead)
        {
            Damaged();
            isDamaged = false;
        }

        if (hp == 0 && !isAlreadyDead)
        {
            Dead();
            isAlreadyDead = true;

        }

        if (attackState == 2)
        {
            hitAreaScript.attacker = gameObject.tag;

            Vector3 hitLocation = gameObject.transform.position;
            hitLocation.y = 0.5f;
            hitLocation.x += Mathf.Sin(Mathf.Deg2Rad * hitDirection);
            hitLocation.z += Mathf.Cos(Mathf.Deg2Rad * hitDirection);
            
            Instantiate(hitArea, hitLocation, Quaternion.identity);
            attackState = 0;
        }

    }


}
