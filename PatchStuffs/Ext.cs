using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public static class Ext { }

public class Scriptable<T>
    where T : ScriptableObject, new()
{
    readonly Action<T> modifier;

    public Scriptable() { }

    public Scriptable(Action<T> modifier)
    {
        this.modifier = modifier;
    }

    public static implicit operator T(Scriptable<T> scriptable)
    {
        T result = ScriptableObject.CreateInstance<T>();
        scriptable.modifier?.Invoke(result);
        return result;
    }
}
