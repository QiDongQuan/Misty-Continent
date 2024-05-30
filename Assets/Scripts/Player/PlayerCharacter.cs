using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Run,
    Attack,
    Die
}

public class PlayerCharacter : Character
{
    Animator animator;
    PlayerController controller;
    Vector2 move;
    Rigidbody2D rb;
    Transform target;
    PlayerState _state;
    public PlayerState state
    {
        get { return _state; }
        set { _state = value; }
    }

    private void Start()
    {
        state = PlayerState.Idle;
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (target)
        {
            Fild(target);
        }
        else
        {
            Fild(controller.h);
        }
        move = new Vector2 (controller.h, controller.v);
    }

    private void FixedUpdate()
    {
        if(state == PlayerState.Run)
        {
            Vector2 dir = move.normalized;
            rb.velocity = new Vector2(dir.x*runSpeed,dir.y*runSpeed);
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
    }
    public void RunUpdate()
    {
        if(move.magnitude < 0.1f)
        {
            animator.SetBool("Run",false);
        }
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

    void Fild(float h)
    {
        Vector3 scaleLeft = new Vector3(-1, 1, 1);
        Vector3 scaleRight = new Vector3(1, 1, 1);
        if(h > 0.1)
        {
            transform.localScale = scaleRight;
        }else if(h < 0.1)
        {
            transform.localScale = scaleLeft;
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
        }else if(d < 0)
        {
            transform.localScale = scaleLeft;
        }
    }
}
