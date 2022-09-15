using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chop,
        Grab,
        Throw,
        Die
    }
    public State curState = State.Idle;
    private State saveState = State.Idle;
    private Animator animator;
    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();    
    }
    private void Update()
    {
        if (curState != saveState)
        {
            ChangeState(curState);
        }
    }

    private void ChangeState(State curState)
    {
        switch (curState)
        {
            case State.Idle:
                animator.SetTrigger("Idle");
                break;
            case State.Chop:
                animator.SetTrigger("Chop");
                break;
            case State.Grab:
                animator.SetTrigger("Grab");
                break;
            case State.Throw:
                animator.SetTrigger("Throw");
                break;
            case State.Die:
                animator.SetTrigger("Die");
                break;
        }
        saveState = curState;
    }

}
