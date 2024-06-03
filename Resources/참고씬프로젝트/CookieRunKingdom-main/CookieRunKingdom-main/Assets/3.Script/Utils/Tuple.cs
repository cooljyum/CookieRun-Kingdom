using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Pair<T, U>
{
    public T First { get; set; }
    public U Second { get; set; }

    public Pair(T first, U second)
    {
        First = first;
        Second = second;
    }
}

public struct Tuple<T, U, V>
{
    public T First { get; set; }
    public U Second { get; set; }
    public V Third { get; set; }

    public Tuple(T first, U second, V third)
    {
        First = first;
        Second = second;
        Third = third;
    }
}
