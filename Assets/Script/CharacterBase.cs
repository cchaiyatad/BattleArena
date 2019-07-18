using UnityEngine;
using System.Collections;

public abstract class CharacterBase : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackDelay = 4f;
    public int hp = 3;

    public bool isAttack;
    public bool isDamaged;

    public Animator animator { get; set; }
    public float nextAttackTime { get; set; }
    public bool isMove { get; set; }
    public bool isAlreadyDead { get; set; }

    protected abstract void Move(Vector3 dir);

    protected abstract void Dead();

    protected abstract void Damaged();

}
