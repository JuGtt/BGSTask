using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "Variables/BoolVariable")]
public class BoolVariable : ScriptableObject
{
    public bool Value;

#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public static implicit operator bool(BoolVariable variable)
    {
        return variable.Value;
    }
    public void SetValue(bool value)
    {
        Value = value;
    }

    public void SetValue(BoolVariable value)
    {
        Value = value.Value;
    }
}
