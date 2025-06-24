using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class UnitAnimation : MonoBehaviour,
    IEventHandlerProvider<EUnitState>
{
    [SerializeField] Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Initialize()
    {
    }
    public void ChangeAnimation(EUnitState state)
    {
        animator.SetBool("isMove", FlagEnumHas(state, EUnitState.Move));
        animator.SetBool("isDead", FlagEnumHas(state, EUnitState.Die));
    }

    public void CallbackEvent(EUnitState value)
    {
        ChangeAnimation(value);
    }

    Action _unRegistCallback;
    public void SetEventUnRegistCallback(Action callback)
    {
        _unRegistCallback = callback;
    }

    private void OnDestroy()
    {
        _unRegistCallback?.Invoke();
    }
}
