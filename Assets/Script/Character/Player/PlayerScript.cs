using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : CharacterBase
{
    private bool isHitObstacle;
    private Vector3 direction;

    [SerializeField]
    private Text UI;

    public bool isMultiplayer;

    void Start()
    {
        if (isMultiplayer)
        {
            return;
        }
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
        hitAreaScript = hitArea.GetComponent<HitAreaScript>();
    }

    void Update()
    {
        if (isMultiplayer)
        {
            return;
        }
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
        if (isMultiplayer)
        {
            return;
        }
        if (isAlreadyDead)
        {
            return;
        }
        Move(direction);
        AttackRotate(direction);
    }

    public void CheckObstacle()
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

    public void AttackRotate(Vector3 dir)
    {
        if (dir == Vector3.zero)
        {
            return;
        }
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public override void Move(Vector3 dir)
    {
        isMove = (dir != Vector3.zero);
        animator.SetBool("IsMove", isMove);

        if (dir == Vector3.zero || attackState || isHitObstacle)
        {
            return;
        }

        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);

    }

    public override void Damaged(int damage)
    {
        base.Damaged(damage);
        UI.text = playerName + " HP: " + hp;
    }
}
