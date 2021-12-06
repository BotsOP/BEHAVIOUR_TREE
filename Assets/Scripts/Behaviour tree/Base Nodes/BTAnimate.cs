using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTAnimate : BTBaseNode
{
    private Animator anim;
    private AnimatePackage animatePackage;
    private float animationTime;
    private float currentValue;
    private bool equalAnimations;
    private bool debug;
    
    public BTAnimate(Animator _anim, float _animationTime, AnimatePackage _animatePackage)
    {
        animationTime = _animationTime;
        animatePackage = _animatePackage;

        anim = _anim;
    }
    public BTAnimate(Animator _anim, float _animationTime, AnimatePackage _animatePackage, bool _debug)
    {
        animationTime = _animationTime;
        animatePackage = _animatePackage;
        debug = _debug;

        anim = _anim;
    }
    public override TaskStatus Run()
    {
        if (equalAnimations)
            return TaskStatus.Success;
        
        int animationsSuccess = 0;
        
        if (currentValue == 0)
        {
            currentValue = (animatePackage.animationValue - anim.GetFloat(animatePackage.animationName)) / (50 * animationTime);
        }
        //Debug.Log(currentValue + "      " + animatePackage.animationValue + "   " + anim.GetFloat(animatePackage.animationName));
        
        anim.SetFloat(animatePackage.animationName, anim.GetFloat(animatePackage.animationName) + currentValue);

        if (Math.Abs(anim.GetFloat(animatePackage.animationName) - animatePackage.animationValue) < 0.1)
        {
            anim.SetFloat(animatePackage.animationName, animatePackage.animationValue);
            return TaskStatus.Success;
        }

        
        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        equalAnimations = false;
        bool animationsSuccess = false;
        currentValue = 0;
        
        if (Math.Abs(animatePackage.animationValue - anim.GetFloat(animatePackage.animationName)) < 0.1f)
        {
            equalAnimations = true;
        }
    }
    public override void OnExit()
    {
        currentValue = 0;
    }
}

public struct AnimatePackage {
    public float animationValue;
    public string animationName;
    
    public AnimatePackage(float _animationValue, string _animationName)
    {
        animationValue = _animationValue;
        animationName = _animationName;
    }
}
