using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : CharacterBase
{
    public GameObject target;
    public bool isUseSkill;
    private Vector3 targetPath;

    private Vector3 path;
    private Vector3 escapedPoint;
    private bool hasEscapedPoint;
    private List<Vector3> corners = new List<Vector3> { };

    void Start()
    {
        Vector3 size = GameObject.Find("Plane").GetComponent<Renderer>().bounds.size - (2 * Vector3.one);
        for (int i = -1; i < 2; i += 2)
        {
            for (int j = -1; j < 2; j += 2)
            {
                corners.Add(new Vector3(size.x / 2 * i, 0f, size.z / 2 * j));
            }
        }

        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
        hitAreaScript = hitArea.GetComponent<HitAreaScript>();
        
    }

    void Update()
    {
        CharacterBehavior();
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        if (Time.time < nextAttackTime)
        {
            if (!hasEscapedPoint)
            {
                escapedPoint = Escape();
                hasEscapedPoint = true;
            }
            path = AStar.FindWay(transform.position, escapedPoint, true);
        }
        else
        {
            hasEscapedPoint = false;
            path = AStar.FindWay(transform.position, target.transform.position, false);
        }

        targetPath = path - transform.position;
        

        Move(targetPath);
    }

    public override void Move(Vector3 dir)
    {

        float distanceToTarget = (target.gameObject.transform.position - transform.position).magnitude;

        if (attackState || isUseSkill)
        {
            return;
        }

        if (distanceToTarget <= 0.6)
        {
            dir *= -1;
        }


        if (distanceToTarget <= 0.75)
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

    private Vector3 Escape()
    {
        float max = -1;
        int index = -1;
        for (int i = 0; i < corners.Count; i++)
        {
            float distance = (corners[i] - transform.position).magnitude;
            if (max < distance)
            {
                max = distance;
                index = i;
            }

        }
        return corners[index];
    }
    
}
