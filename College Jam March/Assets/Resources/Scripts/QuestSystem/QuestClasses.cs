using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public enum ObjectiveType
    {
        TalkToNPC,
        KillEnemies,
        GatherItems,
        VisitArea
    }

    public enum RewardType
    {
        XPReward
    }

    [Serializable]
    public class Objective
    {
        public ObjectiveType type = 0;
        public string name = "";
        public string publicName = "";
        public bool hidden = false;
        public int killStatus = 0;
        public int killTarget = 3;
        public ScriptableObject target = null;
        public string npcReference = "";

        public List<Objective> nextObjectives = new List<Objective>();

        [SerializeReference]
        public bool objectiveComplete = false;
    }

    [Serializable]
    public class QuestReward
    {
        public RewardType type = 0;
        public string rewardName = "";
        public float rewardAmount = 1;
        public bool hidden = false;
    }
}