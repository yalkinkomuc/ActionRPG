using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
    
{
    
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;
    protected float xInput;
    protected float yInput;
    protected float stateTimer;
    protected bool triggerCalled;

    private string animBoolName;

    public PlayerState(Player _player,PlayerStateMachine _stateMachine,string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb =player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer-= Time.deltaTime;
        xInput = player.input.horizontal;
        yInput = player.input.vertical;
        player.anim.SetFloat("yVelocity", rb.velocity.y);

    }
    public virtual void AnimationFinish()
    {
        triggerCalled = true;
    }





    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);


    }
}
