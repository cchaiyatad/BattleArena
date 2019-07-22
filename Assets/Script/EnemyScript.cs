using UnityEngine;

public class EnemyScript : CharacterBase
{
    public GameObject target;
    private Vector3 targetPath;
    private int newVector;
    private bool isEscaping;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
        hitAreaScript = hitArea.GetComponent<HitAreaScript>();
    }

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
        targetPath = target.gameObject.transform.position - transform.position;
        if (Time.time < nextAttackTime && attackState == 0)
        {
            targetPath *= -1;
        }
        CheckObstacle();
        Move(targetPath);
    }


    protected override void Move(Vector3 dir)
    {

        if (attackState == 1)
        {
            return;
        }

        if (dir.magnitude <= 0.6)
        {
            dir *= -1;
        }

        else if (dir.magnitude <= 0.75)
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

    protected override void CheckObstacle()
    {

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            isHitObstacle = (transform.position - hit.transform.position).magnitude < 0.8;

            if (hit.transform.CompareTag("Obstacle") && !isEscaping)
            {
                isEscaping = true;
                newVector = Random.Range(-1, 2);
                while (newVector == 0)
                {
                    newVector = Random.Range(-1, 2);
                }

            }
        }

        if (isEscaping)
        {
            

            if (Physics.Raycast(transform.position, targetPath, out RaycastHit side))
            {
                if (side.transform.CompareTag("Player"))
                {
                    isEscaping = false;
                    return;
                }
            }

            targetPath = Vector3.Cross(transform.up * newVector, targetPath);
            if (isHitObstacle)
            {
                Debug.Log("foo");
            }
        }




    }
}

