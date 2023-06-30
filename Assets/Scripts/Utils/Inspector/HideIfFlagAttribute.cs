using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIfFlagAttribute : PropertyAttribute
{
    public string PropertyPath { get; set; }
    public bool HideValue { get; set; }

    public HideIfFlagAttribute(string propertyPath, bool hideValue = true)
    {
        PropertyPath = propertyPath;
        HideValue = hideValue;
    }
}