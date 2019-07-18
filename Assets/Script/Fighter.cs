using UnityEngine;
using System.Collections;

public abstract class Fighter : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackDelay = 4f;
	public int hp = 3;

	protected Animator animator;
   	protected bool isMove;
	protected float nextAttackTime;

    public bool isAttack;

    protected abstract void Move(Vector3 dir);

    protected virtual void Dead()
	{
		animator.SetTrigger("IsDeath");
	}
}
