using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Idle,
    Run,
    Attack,
    Die
}

public class PlayerCharacter : Character
{
    [HideInInspector]
    public int lv = 1;
    //[HideInInspector]
    public int experience;
    Animator animator;
    PlayerController controller;
    Vector2 move;
    Rigidbody2D rb;
    BuffHandler buffHandler;
    Bag bag;
    public Transform target;
    public Transform firePoint;
    Transform weaponSlot;
    SkillBuff skillBuff;
    PlayerState _state;
    public PlayerState state
    {
        get { return _state; }
        set { _state = value; }
    }

    public Slider HpUI;
    public Slider EnenrgyUI;
    public Canvas Canvas;

    private void Start()
    {
        Hp = MaxHp;
        Enenrgy = MaxEnenrgy;
        state = PlayerState.Idle;
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        weaponSlot = transform.Find("WeaponSlot");
        buffHandler = GetComponent<BuffHandler>();
        skillBuff = GetComponent<SkillBuff>();
        bag = GetComponent<Bag>();
        AddBuff("BuffData/Normal_Damage", gameObject, gameObject);
        //AddBuff("BuffData/Skill01_BuffData", gameObject, gameObject);
        AddBuff("BuffData/PlayerLevelUp_BuffData", gameObject, gameObject);
        AddBuff("BuffData/GetEnergy_BuffData",gameObject,gameObject);
    }

    public void AddBuff(string buffData,GameObject craetor,GameObject target)
    {
        BuffInfo buffInfo = new BuffInfo(Resources.Load<BuffData>(buffData), gameObject, gameObject);
        buffHandler.AddBuff(buffInfo);
        if (buffInfo.buffData.isSkill)
        {
            skillBuff.buffDatas.Add(buffInfo);
            skillBuff.Refresh();
        }
    }

    private void Update()
    {
        move = new Vector2 (controller.h, controller.v);
        RefreshUI();
        RefreshAttribute();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        switch (state)
        {
            case PlayerState.Idle:

                break;
            case PlayerState.Run:
                Vector2 dir = move.normalized;
                rb.velocity = new Vector2(dir.x * runSpeed, dir.y * runSpeed);
                break;
        }
    }

    #region ºËÐÄ×´Ì¬»ú
    delegate void FuncStateEnter();
    delegate void FuncStateUpdate();
    delegate void FuncStateExit();

    Dictionary<PlayerState, FuncStateEnter> dictStateEnter;
    Dictionary<PlayerState, FuncStateUpdate> dictStateUpdate;
    Dictionary<PlayerState, FuncStateExit> dictStateExit;

    public void OnAnimStateEnter(PlayerState st)
    {
        if(dictStateEnter == null)
        {
            dictStateEnter = new Dictionary<PlayerState, FuncStateEnter>
            {
                { PlayerState.Idle,IdleEnter},
                { PlayerState.Run,RunEnter},
                { PlayerState.Attack,AttackEnter},
                { PlayerState.Die,DieEnter}
            };
        }
        if(dictStateEnter.ContainsKey(st) && dictStateEnter[st] != null)
        {
            dictStateEnter[st]();
        }
    }

    public void OnAnimStateUpdate(PlayerState st)
    {
        if (dictStateUpdate == null)
        {
            dictStateUpdate = new Dictionary<PlayerState, FuncStateUpdate>
            {
                { PlayerState.Idle,IdleUpdate},
                { PlayerState.Run,RunUpdate},
                { PlayerState.Attack,AttackUpdate},
                { PlayerState.Die,DieUpdate}
            };
        }
        if (dictStateUpdate.ContainsKey(st) && dictStateUpdate[st] != null)
        {
            dictStateUpdate[st]();
        }
    }

    public void OnAnimStateExit(PlayerState st)
    {
        if (dictStateExit == null)
        {
            dictStateExit = new Dictionary<PlayerState, FuncStateExit>
            {
                { PlayerState.Idle,IdleExit},
                { PlayerState.Run,RunExit},
                { PlayerState.Attack,AttackExit},
                { PlayerState.Die,DieExit}
            };
        }
        if (dictStateExit.ContainsKey(st) && dictStateExit[st] != null)
        {
            dictStateExit[st]();
        }
    }

    //---------------------------Enter----------------------------//
    public void IdleEnter()
    {
        state = PlayerState.Idle;
    }
    public void RunEnter()
    {
        state = PlayerState.Run;
    }
    public void AttackEnter()
    {
        state = PlayerState.Attack;
    }
    public void DieEnter()
    {
        state = PlayerState.Die;
    }

    //---------------------------Update---------------------------//
    public void IdleUpdate()
    {
        if (move.magnitude > 0.1f)
        {
            animator.SetBool("Run",true);
        }
        AttackAndFildUpdate();
    }
    public void RunUpdate()
    {
        if(move.magnitude < 0.1f)
        {
            animator.SetBool("Run",false);
        }
        AttackAndFildUpdate();
    }
    public void AttackUpdate()
    {

    }
    public void DieUpdate()
    {

    }
    //---------------------------Exit-----------------------------//
    public void IdleExit()
    {

    }
    public void RunExit()
    {

    }
    public void AttackExit()
    {

    }
    public void DieExit()
    {

    }
    #endregion

    void PlayerAttack()
    {

    }

    void AttackAndFildUpdate()
    {
        if (target && target.CompareTag("Enemy") && target.GetComponent<EnemyCharacter>().state == EnemyState.Die)
        {
            target = null;
        }
        if (target && target.CompareTag("Boss") && target.GetComponent<BossCharacter>().state == BossState.Die)
        {
            target = null;
        }

        target = TargetScan();
        if (target)
        {
            Fild(target);
            weaponSlot.LookAt(target);
            PlayerAttack();
        }
        else
        {
            Fild(controller.h);
        }
    }

    public Transform TargetScan()
    {
        List<Transform> targetList = new List<Transform>();
        Collider2D[] collides = Physics2D.OverlapCircleAll(transform.position, 4.0f, LayerMask.GetMask("CanHit"));
        foreach(Collider2D col in collides)
        {
            if (col.CompareTag("Enemy") && col.GetComponent<EnemyCharacter>().state == EnemyState.Die)
            {
                continue;
            }
            if (col.CompareTag("Boss") && col.GetComponent<BossCharacter>().state == BossState.Die)
            {
                continue;
            }
            targetList.Add(col.transform);
        }
        targetList.Sort((a,b) =>
        {
            float distanceA = Vector3.Distance(transform.position, a.position);
            float dictanceB = Vector3.Distance(transform.position,b.position);
            return distanceA.CompareTo(dictanceB);
        });
        if(targetList.Count == 0)
        {
            return null;
        }
        return targetList[0];
    }

    public override void GetHit(int damage)
    {
        if(state == PlayerState.Die)
        {
            return;
        }

        Hp-=damage;

        if(Hp <= 0)
        {
            Hp = 0;
            Die();
        }
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        transform.GetComponent<Collider2D>().enabled = false;
        Canvas.enabled = false;
    }

    public void RefreshUI()
    {
        HpUI.maxValue = MaxHp;
        HpUI.value = Hp;
        EnenrgyUI.maxValue = MaxEnenrgy;
        EnenrgyUI.value = Enenrgy;
    }

    public void RefreshAttribute()
    {
        Attack = lv * 5;
        MaxHp = 40+lv*10;
        Armor = lv * 5;
        for (int i = 0; i < bag.playerItems.Count; i++)
        {
            if (bag.playerItems[i] != null)
            {
                Attack += bag.playerItems[i].jsonData.Attack[bag.playerItems[i].quality - 1];
                MaxHp += bag.playerItems[i].jsonData.Hp[bag.playerItems[i].quality - 1];
                Armor += bag.playerItems[i].jsonData.Defensive[bag.playerItems[i].quality - 1];
            }
        }
        
    }

    void Fild(float h)
    {
        Vector3 scaleLeft = new Vector3(-1, 1, 1);
        Vector3 scaleRight = new Vector3(1, 1, 1);
        if(h > 0)
        {
            transform.localScale = scaleRight;
            Canvas.transform.localScale = scaleRight;
        }
        else if(h < 0)
        {
            transform.localScale = scaleLeft;
            Canvas.transform.localScale = scaleLeft;
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
        else if(d < 0)
        {
            transform.localScale = scaleLeft;
            Canvas.transform.localScale = scaleLeft;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,4.0f);
    }
}
