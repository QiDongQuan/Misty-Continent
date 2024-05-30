using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimState : StateMachineBehaviour
{
    public PlayerState state;
    bool flag = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCharacter player = animator.GetComponent<PlayerCharacter>();
        player.OnAnimStateEnter(state);
        flag = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (flag)
        {
            PlayerCharacter player = animator.GetComponent<PlayerCharacter>();
            player.OnAnimStateExit(state);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (flag)
        {
            PlayerCharacter player = animator.GetComponent<PlayerCharacter>();
            if (player.state == state)
            {
                player.OnAnimStateUpdate(state);
            }
            else
            {
                player.OnAnimStateExit(state);
                flag = false;
            }
        }
    }
}
