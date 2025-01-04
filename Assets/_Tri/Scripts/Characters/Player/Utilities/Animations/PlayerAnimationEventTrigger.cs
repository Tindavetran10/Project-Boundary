using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventTrigger : MonoBehaviour
{
    private Player Player;

    private void Awake()
    {
        Player = transform.GetComponent<Player>();
    }

    public void TriggerOnMovementStateAnimationEnterEvent()
    {
        if (IsInAnimationTransition())
        {
            return;
        }

        Player.OnMovementStateAnimationEnterEvent();
    }

    public void TriggerOnMovementStateAnimationExitEvent()
    {
        if (IsInAnimationTransition())
        {
            return;
        }

        Player.OnMovementStateAnimationExitEvent();
    }

    public void TriggerOnMovementStateAnimationTransitionEvent()
    {
        if (IsInAnimationTransition())
        {
            return;
        }

        Player.OnMovementStateAnimationTransitionEvent();
    }

    private bool IsInAnimationTransition(int layerIndex = 0)
    {
        return Player.Animator.IsInTransition(layerIndex);
    }
}
