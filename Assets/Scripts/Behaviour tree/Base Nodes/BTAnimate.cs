using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTAnimate : BTBaseNode
{
    private Animator anim;
    private float[] newValues;
    private string[] animations;
    private float animationTime;
    private float[] currentValues;
    private bool equalAnimations;
    
    public BTAnimate(float[] _newValues, string[] _animations, Animator _anim, float _animationTime)
    {
        newValues = _newValues;
        animations = _animations;
        animationTime = _animationTime;
        currentValues = new float[newValues.Length];
        
        if(animations.Length != newValues.Length)
            Debug.LogError("you dont have matching amount of arguments in your BTAnimate");

        anim = _anim;
    }
    public override TaskStatus Run()
    {
        if (equalAnimations)
            return TaskStatus.Success;
        
        int animationsSuccess = 0;
        for (int i = 0; i < newValues.Length; i++)
        {
            if (currentValues[i] == 0)
            {
                currentValues[i] = (newValues[i] - anim.GetFloat(animations[i])) / (50 * animationTime);
            }
            
            anim.SetFloat(animations[i], anim.GetFloat(animations[i]) + currentValues[i]);
            
            if (Math.Abs(anim.GetFloat(animations[i]) - newValues[i]) < 0.01)
            {
                animationsSuccess++;
                anim.SetFloat(animations[i], newValues[i]);
            }
            
        }

        if (newValues.Length == animationsSuccess)
        {
            currentValues[0] = 0;
            return TaskStatus.Success;
        }
        
        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        
        equalAnimations = false;
        int animationsSuccess = 0;
        for (int i = 0; i < currentValues.Length; i++)
        {
            currentValues[i] = 0;
            if (Math.Abs(newValues[i] - anim.GetFloat(animations[i])) < 0.01f)
            {
                animationsSuccess++;
            }
        }
        
        if (newValues.Length == animationsSuccess)
        {
            equalAnimations = true;
        }
    }
    public override void OnExit()
    {
        for (int i = 0; i < newValues.Length; i++)
        {
            currentValues[i] = 0;
        }
    }
}
