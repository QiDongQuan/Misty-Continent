using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Move,
    Attack,
    Die
}

public class BossCharacter : Character
{
    public float attackDist = 8.0f;
    public float attackCD = 2.0f;
    Animator animator;
    Vector2 move;
    Rigidbody2D rb;
    Transform player;

    public BossState _state;
    public BossState state
    {
        get { return _state; }
        set { _state = value;}
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = BossState.Move;
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        if (state == BossState.Move && Vector3.Distance(transform.position, player.position) > 1)
        {
            if(Vector3.Distance(transform.position,player.position) > attackDist || attackCD > 0)
            {
                move = player.position - transform.position;
                move = move.normalized;
                rb.velocity = move * runSpeed;
            }
        }
        if(move.magnitude > 0)
        {
            animator.SetBool("Run",true);
        }
        else
        {
            animator.SetBool("Run",false);
        }
    }

    private void Update()
    {
        Fild(player);
        if(attackCD > 0)
        {
            attackCD -= Time.deltaTime;
        }
        switch (state)
        {
            case BossState.Move:
                if (Vector3.Distance(transform.position, player.position) < attackDist && attackCD <= 0)
                {
                    state = BossState.Attack;
                    Attack();
                    attackCD = 2.0f;
                }
                break;
            case BossState.Attack:

                break;
            case BossState.Die:

                break;
        }
    }

    void Attack()
    {
        int random = Random.Range(0, 10);
        if(random < 7)
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
        state = BossState.Move;
    }

    public override void GetHit(int damage)
    {
        if (state == BossState.Die)
        {
            return;
        }

        Hp -= damage;
        animator.SetTrigger("GetHit");
        if (Hp <= 0)
        {
            Hp = 0;
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        state = BossState.Die;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
    }
}
