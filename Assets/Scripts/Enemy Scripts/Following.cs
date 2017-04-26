﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : MonoBehaviour {

    private bool targetOn = false, atLeft;
    private GameObject target;
    private float speed;
    private Rigidbody2D rb2d;
    public bool isAttacking = false;
    public bool isBeaten = false;

    private void Start()
    {
        speed = GetComponent<Patrol>().getSpeed();
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(CheckDirection());
    }
    

    private void FixedUpdate()
    {
        if (targetOn)
        {
            if (!isAttacking && !isBeaten)
            {
                if (atLeft)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                    rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
                }
            }
        }
    }

    public void targeting(GameObject target)
    {
        GetComponent<Patrol>().targeting();
        targetOn = true; 
        this.target = target;
    }

    IEnumerator CheckDirection() //따라갈때 방향전환 딜레이
    {
        if (targetOn)
        {
            if (target.transform.position.x > transform.position.x) // 타겟이 오른쪽에 있으면
                atLeft = false;
            else                                                    // 타겟이 왼쪽에 있으면
                atLeft = true;
        }

        yield return new WaitForSecondsRealtime(0.5f);
        StartCoroutine(CheckDirection());
    }
    
}
