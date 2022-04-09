using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueCondition 
{
    public DialogueCondition (string conditionName, bool conditonState)
    {
        name = conditionName;
        state = conditonState;
    }
    
    public string name;
    public bool state;
}

public class DialogueConditions : MonoBehaviour 
{
    [SerializeReference]
    List<DialogueCondition> dialogueConditions = new List<DialogueCondition>();

    public bool GetConditionState(string conditionName)
    {
        DialogueCondition condition = GetCondition(conditionName);
        if (condition == null) return false;
        else return condition.state;
    }

    public void SetConditionState(string conditionName, bool conditionState)
    {
        DialogueCondition condition = GetCondition(conditionName);
        if (condition == null) 
        {
            condition = new DialogueCondition(conditionName, conditionState); 
            dialogueConditions.Add(condition);
        }
        else condition.state = conditionState;
    }

    public DialogueCondition GetCondition(string conditionName)
    {
        DialogueCondition result = null;
        foreach (DialogueCondition condition in dialogueConditions)
        {
            if (condition.name == conditionName)
            {
                result = condition;
            }
        }
        return result;
    }
}