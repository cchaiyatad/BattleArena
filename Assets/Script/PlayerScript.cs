using UnityEngine;

public class PlayerScript : CharacterBase

{
    private Vector3 direction;

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

        if (hp == 0 && !isAlreadyDead)
        {
            Dead();
            isAlreadyDead = true;
        }

        if (isDamaged && !isAlreadyDead)
        {
            Damaged();
            isDamaged = false;
        }
    }

    void FixedUpdate()
    {
        if(isAlreadyDead)
        {
            return;
        }
        Move(direction);

    }

    protected override void Move(Vector3 dir)
    {
        if (dir == Vector3.zero || isAttack)
        {
            return;
        }
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    protected override void Dead()
    {
        animator.SetTrigger("IsDeath");
    }

    protected override void Damaged()
    {
        animator.SetTrigger("IsDameged");
        hp -= 1;
    }

    
}
