using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEnums
{
    public enum ECoinUseType
    {
        None = -1,
        Buy,
        Sell,
    }

    public enum EGameResult
    {
        None = -1,
        GameStart,
        CombatStart,
        RoundClear,
        GameClear,
        GameOver,
    }

    public enum EGameState
    {
        None = -1,
        Waiting,
        Combat,
        GameOver,
        GameClear,
        Result
    }

    [Flags]
    public enum EInputType
    {
        None = 0,
        Down = 1 << 0,   // 1
        Press = 1 << 1,  // 2
        Up = 1 << 2,     // 4
    }

    [Flags]
    public enum ETownState
    {
        None    = 0,
        Enable = 1 << 0,
        Disable = 1 << 1, // 1  
        Attack  = 1 << 2, // 2         
        Break     = 1 << 3,  // 4               
    }

    public enum ETownType
    {
        None = -1,
        PreTown,
        Commander,
        Turret,
        Barracks,
    }

    public enum EUnitCommand
    {
        None = -1,
        StartMove,
        StopMove,
        StartAttack,
        StopAttack,
        Die
    }

    public enum EUnitLifecycleEventType
    {
        Created,
        Destroyed
    }

    [Flags]
    public enum EUnitState
    {
        None    = 0,
        Move    = 1 << 0,  // 2
        Attack  = 1 << 1,  // 4
        Die     = 1 << 2,  // 8
    }

    public enum EUnitType
    {
        None,
        Unit,
        Town
    }

    public static void FlagEnumAdd<T>(ref T flag, T value) where T : Enum
    {
        flag = (T)(object)((int)(object)flag | (int)(object)value);
    }

    public static void FlagEnumRemove<T>(ref T flag, T value) where T : Enum
    {
        flag = (T)(object)((int)(object)flag & ~(int)(object)value);
    }

    public static bool FlagEnumHas<T>(T flag, T value) where T : Enum
    {
        return ((int)(object)flag & (int)(object)value) != 0;
    }

    public static void FlagEnumClear<T>(ref T flag) where T : Enum
    {
        flag = (T)(object)0; // 모든 플래그를 제거
    }
}