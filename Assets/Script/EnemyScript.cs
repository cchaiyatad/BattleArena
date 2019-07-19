using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : CharacterBase
{
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Attack();
        }



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


    }

    protected override void Move(Vector3 dir)
    {
        Debug.Log("Move");
    }

    protected override void Attack()
    {
        animator.SetTrigger("IsAttack");
    }



}
