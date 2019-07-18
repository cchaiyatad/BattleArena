using UnityEngine;
using System.Collections;

public abstract class CharacterBase : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackDelay = 3f;
    public int hp = 3;

    public bool isAttack;
    public bool isDamaged;

    public Animator animator { get; set; }
    public float nextAttackTime { get; set; }
    public bool isMove { get; set; }
    public bool isAlreadyDead { get; set; }

    protected abstract void Move(Vector3 dir);
         
    protected void Dead()
    {
        animator.SetTrigger("IsDeath");
    }

    protected void Damaged()
    {
        animator.SetTrigger("IsDameged");
        hp -= 1;
    }

}
