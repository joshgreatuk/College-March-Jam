using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

namespace Dialogue
{
    [Flags]
    public enum DecisionCondition
    {
        None = 0,
        QuestGiven = 1,
        QuestCompleted = 2
    }

    public enum TriggerType
    {
        AddQuest,
        SwapDialogue
    }

    [Serializable]
    public class DialogueNode
    {
        //Shared
        [Tooltip("Wont affect the game")]
        [TextArea(1,3)]
        public string description = "";
        [TextArea(1,3)]
        public List<string> speech = new List<string>();

        //ChoiceNode
        public bool autoDecision = false;
        public List<Decision> decisions = new List<Decision>();

        //TriggerNode
        public List<Trigger> triggers = new List<Trigger>();
    }

    [Serializable]
    public class Decision
    {
        public DecisionCondition decisionCondition = 0;
        public bool conditionNeeded = false;
        public Quest questNeeded = null;
        public string decisionText = "";
        public bool hidden = false;
        public DialogueObject decisionPath = null;
        public int decisionPathIndex = 0;
        public bool playInstantly = false;
    }

    [Serializable]
    public class Trigger
    {
        public TriggerType triggerType = 0;
        [ConditionalHide("triggerType", (int)TriggerType.AddQuest)]
        public Quest questTarget = null;
        public string objectiveTarget = "";
    }
}