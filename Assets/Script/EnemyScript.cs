using UnityEngine;

public class EnemyScript : CharacterBase
{
    
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
        hitAreaScript = hitArea.GetComponent<HitAreaScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Attack();
        }

        CharacterBehavior();

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
