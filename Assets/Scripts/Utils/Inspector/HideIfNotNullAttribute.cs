using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIfNotNullAttribute : PropertyAttribute
{
    public string PropertyPath { get; set; }

    public HideIfNotNullAttribute(string propertyPath)
    {
        PropertyPath = propertyPath;
    }
}