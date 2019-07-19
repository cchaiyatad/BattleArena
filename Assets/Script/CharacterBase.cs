using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackDelay = 3f;
    public int hp = 3;
    public GameObject hitArea;
    public HitAreaScript hitAreaScript;

    public byte attackState;
    public bool isDamaged;
    public float hitPositionX;
    public float hitPositionZ;

    public Animator animator { get; set; }
    public float nextAttackTime { get; set; }
    public bool isMove { get; set; }
    public bool isAlreadyDead { get; set; }

    protected abstract void Move(Vector3 dir);

    protected abstract void Attack();

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

            Vector3 hitLocation = new Vector3(hitPositionX, 0.5f, hitPositionZ);
            hitLocation.x += Mathf.Sin(Mathf.PI / 180 * gameObject.transform.eulerAngles.y);
            hitLocation.z += Mathf.Cos(Mathf.PI / 180 * gameObject.transform.eulerAngles.y);
            
            Instantiate(hitArea, hitLocation, Quaternion.identity);
            attackState = 0;
        }
    }


}
