using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Move,
    Attack,
    Die
}

public class EnemyCharacter : Character
{
    public float attackDist = 1.0f;
    public float attackCD = 2.0f;
    Animator animator;
    Vector2 move;
    Rigidbody2D rb;
    Transform player;

    public EnemyState _state;
    public EnemyState state
    {
        get { return _state; }
        set { _state = value; }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = EnemyState.Move;
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        if (state == EnemyState.Move && Vector3.Distance(transform.position, player.position) > 1)
        {
            if (Vector3.Distance(transform.position, player.position) > attackDist || attackCD > 0)
            {
                move = player.position - transform.position;
                move = move.normalized;
                rb.velocity = move * runSpeed;
            }
        }
        if (move.magnitude > 0)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    private void Update()
    {
        Fild(player);
        if (attackCD > 0)
        {
            attackCD -= Time.deltaTime;
        }
        switch (state)
        {
            case EnemyState.Move:
                if (Vector3.Distance(transform.position, player.position) < attackDist && attackCD <= 0)
                {
                    state = EnemyState.Attack;
                    Attack();
                    attackCD = 2.0f;
                }
                break;
            case EnemyState.Attack:

                break;
            case EnemyState.Die:

                break;
        }
    }

    void Attack()
    {
        int random = Random.Range(0, 10);
        if (random < 7)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetTrigger("Skill");
        }
    }

    public void FinishAttack()
    {
        state = EnemyState.Move;
    }

    void Fild(Transform target)
    {
        Vector3 scaleLeft = new Vector3(-1, 1, 1);
        Vector3 scaleRight = new Vector3(1, 1, 1);

        Vector2 tmp = target.position - transform.position;
        float d = Vector2.Dot(tmp, transform.right);
        if (d > 0)
        {
            transform.localScale = scaleRight;
        }
        else if (d < 0)
        {
            transform.localScale = scaleLeft;
        }
    }
}
