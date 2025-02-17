using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossState
{
    Move,
    Attack,
    Die
}

public class BossCharacter : Character
{
    [HideInInspector]
    public List<ItemData> dropList;
    public float attackDist = 8.0f;
    public float attackCD = 2.0f;
    Animator animator;
    Vector2 move;
    Rigidbody2D rb;
    Transform player;

    public Transform skillTip;
    public Transform skillBullet;
    public Transform fireball;
    Transform firePoint;

    public Slider HpUI;
    public Canvas Canvas;

    public BossState _state;
    public BossState state
    {
        get { return _state; }
        set { _state = value;}
    }

    private void Start()
    {
        Hp = MaxHp;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = BossState.Move;
        firePoint = transform.Find("FirePoint");
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
        RefreshUI();
        if (Hp>0)
        {
            Fild(player);
        }
        if(attackCD > 0)
        {
            attackCD -= Time.deltaTime;
        }
        switch (state)
        {
            case BossState.Move:
                if (attackCD <= 0)
                {
                    state = BossState.Attack;
                    AAttack();
                    attackCD = 0.5f;
                }
                break;
            case BossState.Attack:
                if (attackCD <= 0)
                {
                    state = BossState.Attack;
                    AAttack();
                    attackCD = 0.5f;
                }
                break;
            case BossState.Die:

                break;
        }
    }

    public void RefreshUI()
    {
        HpUI.maxValue = MaxHp;
        HpUI.value = Hp;
    }

    void AAttack()
    {
        int random = Random.Range(0, 10);
        if (random < 4)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetTrigger("Skill");
        }
    }

    public void NormalAttack()
    {
        Transform tmp = Instantiate(fireball);
        tmp.position = firePoint.position;
        tmp.GetComponent<FireBall>().creator = gameObject;
        tmp.up = transform.position - player.position;
    }

    public void Skill()
    {
        StartCoroutine(UseSkill());
    }

    IEnumerator UseSkill()
    {
        Transform tmp = Instantiate(skillTip);
        tmp.position = player.position;
        yield return new WaitForSeconds(1);
        Transform bullet = Instantiate(skillBullet);
        bullet.position = tmp.position;
        bullet.GetComponent<Fire>().creator = gameObject;
        Destroy(tmp.gameObject);
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
        UIManager.Instance.ShowRecived();
        animator.SetTrigger("Die");
        animator.ResetTrigger("GetHit");
        state = BossState.Die;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
    }
}
