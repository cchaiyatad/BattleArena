using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float attackDelay = 2f;


    private Vector3 direction;
    private Animator animator;
    private bool isMove;
    private float nextAttackTime;

    public bool isAttack;


    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
    }

    void Update()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        isMove = (direction != Vector3.zero);
        animator.SetBool("IsMove", isMove );

        if (Time.time > nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nextAttackTime = Time.time + attackDelay;
                animator.SetTrigger("IsAttack");
            }
        }

    }

    void FixedUpdate()
    {
        Move(direction);

    }

    void Move(Vector3 dir){

        
        if (dir == Vector3.zero || isAttack)
        {
            return;
        }
        transform.Translate(dir * moveSpeed * Time.deltaTime,Space.World);
        transform.rotation = Quaternion.LookRotation(dir);

    }
}
