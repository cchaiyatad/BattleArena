using UnityEngine;

public class PlayerScript : CharacterBase

{
    private Vector3 direction;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);

        HitAreaScript myHitArea = hitArea.GetComponent<HitAreaScript>();
        myHitArea.myTag = gameObject.tag;
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
                Attack();
            }
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

        if (attackState == 2)
        {
            attackState = 0;
            Vector3 hitLocation = new Vector3(this.transform.position.x,
                0.5f, this.transform.position.z);
            hitLocation.x += Mathf.Sin(Mathf.PI / 180 * gameObject.transform.eulerAngles.y);
            hitLocation.z += Mathf.Cos(Mathf.PI / 180 * gameObject.transform.eulerAngles.y);
            Debug.Log(hitLocation);
            Instantiate(hitArea, hitLocation, Quaternion.identity);
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
        if (dir == Vector3.zero || attackState == 1)
        {
            return;
        }
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    protected override void Attack()
    {
        nextAttackTime = Time.time + attackDelay;
        animator.SetTrigger("IsAttack");
    }

    private void OnTriggerEnter(Collider other)
    {
        isDamaged = other.GetComponent<HitAreaScript>().myTag != gameObject.tag;
    }
}
