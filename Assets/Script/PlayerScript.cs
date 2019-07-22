using UnityEngine;

public class PlayerScript : CharacterBase

{
    private Vector3 direction;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
        hitAreaScript = hitArea.GetComponent<HitAreaScript>();
    }

    void Update()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        CheckObstacle();

        if (Time.time > nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
        }

        CharacterBehavior();
    }

    void FixedUpdate()
    {
        if(isAlreadyDead)
        {
            return;
        }
        Move(direction);
        AttackRotate(direction);

    }

    protected override void Move(Vector3 dir)
    {
        isMove = (direction != Vector3.zero);
        animator.SetBool("IsMove", isMove);

        if (dir == Vector3.zero || attackState|| isHitObstacle)
        {
           return;
        }

        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
        
    }

    protected override void AttackRotate(Vector3 dir) 
    {
        if (dir == Vector3.zero)
        {
            return;
        }
        transform.rotation = Quaternion.LookRotation(dir);
    }

    protected override void CheckObstacle()
    {
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.8f))
        {
            isHitObstacle = hit.transform.CompareTag("Obstacle");
        }
        else
        {
            isHitObstacle = false;
        }

    }
}
