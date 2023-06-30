using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Memo<T>
{
    private T value;
    private bool hasValue;
    private System.Func<T> constructor;

    public Memo(System.Func<T> constructor)
    {
        value = default;
        hasValue = false;
        this.constructor = constructor;
    }

    public T GetValue()
    {
        if (hasValue)
            return value;

        value = constructor();
        hasValue = true;
        return value;
    }

    public void Clear()
    {
        hasValue = false;
    }


    public static implicit operator T(Memo<T> memo)
    {
        return memo.GetValue();
    }
}

public struct Memo<C, T>
{
    private T value;
    private bool hasValue;
    private C host;
    private System.Func<C, T> constructor;

    public Memo(C host, System.Func<C, T> constructor)
    {
        value = default;
        hasValue = false;
        this.host = host;
        this.constructor = constructor;
    }

    public T GetValue()
    {
        if (hasValue)
            return value;

        value = constructor(host);
        hasValue = true;
        return value;
    }

    public void Clear()
    {
        hasValue = false;
    }


    public static implicit operator T(Memo<C, T> memo)
    {
        return memo.GetValue();
    }
}