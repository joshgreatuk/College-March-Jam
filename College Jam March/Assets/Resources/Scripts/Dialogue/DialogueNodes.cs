using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using NaughtyAttributes;

namespace Dialogue
{
    public enum DecisionCondition
    {
        QuestGiven = 0,
        QuestCompleted = 1,
        ConditionMet = 2
    }

    public enum TriggerType
    {
        SetCondition = 0,
        AddQuest = 1,
        AddDialogue = 2,
        OpenMenu = 3,
        UnlockArea = 4,
        UnlockQuest = 5,
    }

    [Serializable]
    public class DialogueNode
    {
        //Shared
        [Tooltip("Wont affect the game")]
        [TextArea(1,2)]
        public string description = "";
        [TextArea(1,5)]
        public List<string> speech = new List<string>();

        //ChoiceNode
        public bool autoDecision = false;
        public string decisionSpeech = "";
        public List<Decision> decisions = new List<Decision>();

        //TriggerNode
        public List<Trigger> triggers = new List<Trigger>();
    }

    [Serializable]
    public class Decision
    {
        public List<DecisionConditionClass> conditions = new List<DecisionConditionClass>();
        public string decisionText = "";
        public bool hidden = false;
        public DialogueObject decisionPath = null;
        public int decisionPathIndex = 0;
        public bool playInstantly = false;
    }

    [Serializable]
    public class DecisionConditionClass
    {
        public DecisionCondition decisionCondition = 0;
        [AllowNesting] [ShowIf("decisionCondition", DecisionCondition.ConditionMet)] public bool conditionNeeded = false;
        [AllowNesting] [ShowIf("decisionCondition", DecisionCondition.ConditionMet)] public string conditionName = "";
        [AllowNesting] [ShowIf("decisionCondition", DecisionCondition.QuestGiven)] public Quest questGivenNeeded = null;
        [AllowNesting] [ShowIf("decisionCondition", DecisionCondition.QuestCompleted)] public Quest questCompleteNeeded = null;
    }

    [Serializable]
    public class Trigger
    {
        public TriggerType triggerType = 0;
        [AllowNesting] [ShowIf("triggerType", TriggerType.AddQuest)] public Quest questTarget = null;
        [AllowNesting] [ShowIf("triggerType", TriggerType.AddDialogue)] public DialogueObject dialogueObject = null;
        [AllowNesting] [ShowIf("triggerType", TriggerType.AddDialogue)] public int dialogueIndex = 0;
        [AllowNesting] [ShowIf("triggerType", TriggerType.OpenMenu)] public GameObject menuPrefab = null;
        [AllowNesting] [ShowIf("triggerType", TriggerType.UnlockArea)] public string areaTarget = "";
        [AllowNesting] [ShowIf("triggerType", TriggerType.SetCondition)] public string conditionName = "";
        [AllowNesting] [ShowIf("triggerType", TriggerType.SetCondition)] public bool conditionState = false;
        [AllowNesting] [ShowIf("triggerType", TriggerType.UnlockQuest)] public Quest targetQuest;
    }
}