using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParser<T>
{
    List<T> Parse(string[] lines);
}
public interface ISingleParser<T>
{
    T Parse(string[] lines);
}
