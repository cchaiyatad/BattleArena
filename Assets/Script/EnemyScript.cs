using UnityEngine;

public class EnemyScript : CharacterBase
{
    public GameObject target;


    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
        hitAreaScript = hitArea.GetComponent<HitAreaScript>();
    }

    // Update is called once per frame
    void Update()
    {

        CharacterBehavior();

    }

    private void FixedUpdate()
    {

        if (isAlreadyDead)
        {
            return;
        }
        Vector3 targetPath = target.gameObject.transform.position - transform.position;
        if (Time.time < nextAttackTime && attackState == 0)
        {
            targetPath *= -1;
            moveSpeed = 2;
        }
        Move(targetPath);
    }


    protected override void Move(Vector3 dir)
    {
        if (attackState == 1)
        {
            return;
        }
        if (dir.magnitude <= 0.5)
        {
            dir *= -1;
        }
        else if (dir.magnitude <= 0.7)
        {
            animator.SetBool("IsMove", false);

            if (Time.time > nextAttackTime)
            {
                Attack();
            }
            return;
        }
        animator.SetBool("IsMove", true);
        dir.Normalize();
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    protected override void AttackRotate(Vector3 dir)
    {
        Debug.Log("rotate");
    }

    protected override void Attack()
    {
        nextAttackTime = Time.time + attackDelay;
        animator.SetTrigger("IsAttack");
    }



}
