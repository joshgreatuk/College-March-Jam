using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public enum DialogueNodeType
    {
        Speech,
        Choice,
        Trigger
    }

    public enum TriggerType
    {
        
    }

    [Serializable]
    public class DialogueNode
    {
        //Base
        public DialogueNodeType nodeType;

        //Shared
        public List<string> speech;

        //ChoiceNode
        public List<Decision> decisions;

        //TriggerNode
        public List<Trigger> triggers;
    }

    [Serializable]
    public class Decision
    {
        public string text;
        public bool locked;
        public bool hidden;
        public DialogueObject decisionPath;
    }

    [Serializable]
    public class Trigger
    {
        public TriggerType type;
    }
}