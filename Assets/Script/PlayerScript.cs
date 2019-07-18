using UnityEngine;

public class PlayerScript : MonoBehaviour

{
    public float moveSpeed = 2f;
    public float attackDelay = 4f;
    public int hp = 3;

    private Animator animator;
    private bool isMove;
    private float nextAttackTime;

    public bool isAttack;

    private Vector3 direction;
    private bool isAlreadyDead;

    public bool isDamaged;

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

    private void Move(Vector3 dir)
    {
        if (dir == Vector3.zero || isAttack)
        {
            return;
        }
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void Dead()
    {
        animator.SetTrigger("IsDeath");
    }

    private void Damaged()
    {
        animator.SetTrigger("IsDameged");
        hp -= 1;
    }

    
}
