using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour 
{
    public string logPrefix;
    public string prefixColor;

    public bool loggerActive = true;

    public void Log(string message)
    {
        if (loggerActive) Debug.Log($"<color={prefixColor}><b>({logPrefix})</b></color> '{message}'");
    }

    public void Log(string message, Object reference)
    {
        if (loggerActive) Debug.Log($"<color={prefixColor}><b>({logPrefix})</b></color> '{message}'", reference);
    }
}