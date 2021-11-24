using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard
{
    public Dictionary<string, object> Values = new Dictionary<string, object>();
    public T GetValue<T>(string name)
    {
        return Values.ContainsKey(name) ? (T)Values[name] : default(T);
    }
    public void SetValue<T>(string name, T value)
    {
        if (Values.ContainsKey(name))
        {
            Values[name] = value;
        }
        else
        {
            Values.Add(name, value);
        }
    }
}
