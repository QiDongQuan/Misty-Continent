using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Move,
    Attack,
    Die
}

public class EnemyCharacter : Character
{
    //[HideInInspector]
    public List<ItemData> dropList;
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

    public Slider HpUI;
    public Canvas Canvas;

    private void Start()
    {
        Hp = MaxHp;
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
        RefreshUI();
        if (state != EnemyState.Die)
        {
            Fild(player);
        }
        
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
                    AAttack();
                    attackCD = 2.0f;
                }
                break;
            case EnemyState.Attack:

                break;
            case EnemyState.Die:

                break;
        }
    }

    void AAttack()
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

    public void RefreshUI()
    {
        HpUI.maxValue = MaxHp;
        HpUI.value = Hp;
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
            Canvas.transform.localScale = scaleRight;
        }
        else if (d < 0)
        {
            transform.localScale = scaleLeft;
            Canvas.transform.localScale = scaleLeft;
        }
    }

    public override void GetHit(int damage)
    {
        if (state == EnemyState.Die)
        {
            return;
        }

        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        state = EnemyState.Die;
        transform.GetComponent<Collider2D>().enabled = false;
        Canvas.enabled = false;
        DeathDrop();
        StartCoroutine(Delete());
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    void DeathDrop()
    {
        float angleDelta = 360.0f / dropList.Count; // 计算每个道具之间的角度差
        float dropRadius = 1.0f; // 设置道具分散的半径大小
        for (int i = 0; i < dropList.Count; i++)
        {
            // 计算当前道具的角度（转换为弧度）
            float angleRad = (angleDelta * i) * Mathf.Deg2Rad;

            // 计算当前道具的位置偏移
            Vector3 offset = new Vector3(Mathf.Cos(angleRad) * dropRadius, Mathf.Sin(angleRad) * dropRadius, 0);

            GameObject tmp = Instantiate(Resources.Load<GameObject>("ItemDrop"));
            tmp.transform.position = transform.position + offset;
            tmp.GetComponent<ItemDrop>().item = dropList[i];
        }
    }
}
