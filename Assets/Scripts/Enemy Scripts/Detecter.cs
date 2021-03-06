﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detecter : MonoBehaviour {
    
    private GameObject target;
    private BoxCollider2D collider2d;
    private Animator ani;
    public AudioSource growlSound;

    public bool targeted = false;
    public bool isBeaten = false;
    private bool isAttacking = false;
    private bool canAttack = false;
    
    private float attackDelay = 0.5f;
    private float attackTime = 0.2f;

    private int attackDamage = 10;
    
    private float detectRange;
    private int mask;
    private RaycastHit2D hit2d;
    private RaycastHit2D attackHit2d;
    
    private void Start()
    {
        collider2d = GetComponent<BoxCollider2D>();
        detectRange = collider2d.size.x * 2;
        mask = 1 << LayerMask.NameToLayer("Character");
        ani = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!targeted)
        {
            if (GetComponent<SpriteRenderer>().flipX)
            {
                hit2d = Physics2D.Raycast(transform.position, Vector2.right, detectRange, mask);
            }
            else
            {
                hit2d = Physics2D.Raycast(transform.position, Vector2.left, detectRange, mask);
            }
            if (hit2d)
            {
                GetComponent<Following>().targeting(hit2d.collider.gameObject);
                targeted = true;
                growlSound.Play();
                target = hit2d.collider.gameObject;
                detectRange = detectRange * 0.2f;
            }
        }
        else
        {
            if (targeted && !isBeaten)
            {
                if (GetComponent<SpriteRenderer>().flipX)
                {
                    attackHit2d = Physics2D.Raycast(transform.position, Vector2.right, detectRange, mask);
                }
                else
                {
                    attackHit2d = Physics2D.Raycast(transform.position, Vector2.left, detectRange, mask);
                }
            }

            if (attackHit2d)
            {
                if (!isAttacking && !isBeaten)
                {
                    GetComponent<Following>().isAttacking = true;
                    StartCoroutine(waitAndAttack());
                }

                if (canAttack && !isBeaten)
                {
                    GetComponent<Following>().isAttacking = true;
                    if (target.transform.position.x > transform.position.x)
                        target.GetComponent<CharacterStatus>().knockFromRight = false;
                    else
                        target.GetComponent<CharacterStatus>().knockFromRight = true;
                    target.GetComponent<CharacterStatus>().attacked(attackDamage);
                }
            }
            else if(!isAttacking)
                GetComponent<Following>().isAttacking = false;
        }
    }
    
    private void Update()
    {
        if (isBeaten)
        {
            canAttack = false;
            StopCoroutine(waitAndAttack());
        }
    }

    IEnumerator waitAndAttack()
    {
        GetComponent<Following>().isAttacking = true;
        yield return new WaitForSecondsRealtime(0.25f); //공격 선딜
        GetComponent<EnemyStatus>().isHitting = true;
        isAttacking = true;        
        ani.SetBool("EnemyAttack", true);
        yield return new WaitForSecondsRealtime(attackDelay);
        canAttack = true;
        yield return new WaitForSecondsRealtime(attackTime);
        isAttacking = false;
        canAttack = false;
        GetComponent<EnemyStatus>().isHitting = false;
        ani.SetBool("EnemyAttack", false);            
    }
}
