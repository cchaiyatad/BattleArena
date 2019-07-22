using UnityEngine;

public class EnemyScript : CharacterBase
{
    public GameObject target;
    private Vector3 targetPath;
    private int newVector;
    private bool isEscaping;

    private bool forwardHit;
    private bool leftHit;
    private bool rightHit;

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
        if (Time.time < nextAttackTime && !attackState)
        {
            targetPath *= -1;
        }
        CheckObstacle();
        Move(targetPath);
    }


    protected override void Move(Vector3 dir)
    {

        if (attackState)
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
        

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.8f))
        {
            //isHitObstacle = (transform.position - hit.transform.position).magnitude < 0.8;

            if (hit.transform.CompareTag("Obstacle"))
            {
                forwardHit = true;
                isHitObstacle = true;
                Debug.Log(targetPath + " "+ (hit.transform.position - transform.position) + " "
                    + targetPath.magnitude + " " +
                    (hit.transform.position - transform.position - new Vector3(0, 0.5f, 0)).magnitude + " "
                    + Vector3.Dot(targetPath, hit.transform.position - transform.position));
                Debug.Log(Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(targetPath, hit.transform.position - transform.position)/(targetPath.magnitude *
                    (hit.transform.position - transform.position - new Vector3(0, 0.5f, 0)).magnitude)));
                Debug.Log(targetPath - (hit.transform.position - transform.position));
            }

        }

        /*if (isHitObstacle)
        {
            

            if (Physics.Raycast(transform.position, transform.right, out RaycastHit rightHitRay, 0.8f))
            {
                rightHit = rightHitRay.transform.CompareTag("Obstacle");
            }
            if (Physics.Raycast(transform.position, transform.right * (-1), out RaycastHit leftHitRay, 0.8f))
            {
                leftHit = rightHitRay.transform.CompareTag("Obstacle");
            }



        }*/

    }
}

