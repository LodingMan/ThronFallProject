using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitPair
{
    public string Key;
    public Unit Unit;
}

[CreateAssetMenu(fileName = "ScriptableUnitBundle", menuName = "Scriptable/Units")]
public class ScriptableUnits : ScriptableObject
{
    public List<UnitPair> Units;

    public Unit FindUnit(string name)
    {
        return Units.Find(u => u.Key == name).Unit;
    }

}
