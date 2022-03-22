using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

namespace Dialogue
{
    public enum DecisionCondition
    {
        QuestGiven,
        QuestCompleted
    }

    public enum TriggerType
    {
        AddQuest,
        UpdateQuest
    }

    [Serializable]
    public class DialogueNode
    {
        //Shared
        public List<string> speech;

        //ChoiceNode
        public bool autoDecision = false;
        public List<Decision> decisions;

        //TriggerNode
        public List<Trigger> triggers;
    }

    [Serializable]
    public class Decision
    {
        public DecisionCondition decisionCondition;
        public bool conditionNeeded = false;
        public string text;
        public bool locked;
        public bool hidden;
        public DialogueObject decisionPath;
        public int decisionPathIndex = 0;
        public bool playInstantly = false;
    }

    [Serializable]
    public class Trigger
    {
        public TriggerType triggerType;
        [ConditionalHide("triggerType", (int)TriggerType.AddQuest)]
        public Quest questTarget;
        public string objectiveTarget;
    }
}