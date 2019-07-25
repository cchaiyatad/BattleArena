﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : CharacterBase
{
    public GameObject target;
    private Vector3 targetPath;

    private List<(float, float)> pathList;
    private Vector3 escapedPoint;
    private bool hasEscapedPoint;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("MoveSpeed", moveSpeed);
        hitAreaScript = hitArea.GetComponent<HitAreaScript>();
        playerName = "enemy";
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

        if (Time.time < nextAttackTime)
        {
            if (!hasEscapedPoint)
            {
                escapedPoint = Escape();
                hasEscapedPoint = true;
            }
            pathList = AStar.FindWay(transform.position, escapedPoint, true);
        }
        else
        {
            hasEscapedPoint = false;
            pathList = AStar.FindWay(transform.position, target.transform.position, false);
        }

        targetPath = new Vector3(pathList[0].Item1 - transform.position.x, 0, pathList[0].Item2 - transform.position.z);


        Move(targetPath);
    }


    protected override void Move(Vector3 dir)
    {

        float distanceToTarget = (target.gameObject.transform.position - transform.position).magnitude;

        if (attackState)
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
                dir *= -1;
                //transform.rotation = Quaternion.LookRotation(dir);
                Attack();
            }
            return;
        }

        animator.SetBool("IsMove", true);
        dir.Normalize();
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(dir);
    }


    protected override void CheckObstacle()
    {
        Debug.Log("obstacle");
    }

    private Vector3 Escape()
    {
        var corners = new List<Vector3> { };
        Vector3 size = GameObject.Find("Plane").GetComponent<Renderer>().bounds.size - (2 * Vector3.one);

        for (int i = -1; i < 2; i += 2)
        {
            for (int j = -1; j < 2; j += 2)
            {
                corners.Add(new Vector3(size.x / 2 * i, 0f, size.z / 2 * j));
            }
        }

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