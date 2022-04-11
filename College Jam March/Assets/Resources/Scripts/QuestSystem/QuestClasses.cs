using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using NaughtyAttributes;

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
        public string publicName = "";
        [TextArea(1,7)] public string description = "";
        public bool hidden = false;
        [AllowNesting] [ReadOnly] [ShowIf("type", ObjectiveType.KillEnemies)] public int killStatus = 0;
        [AllowNesting] [ShowIf("type", ObjectiveType.KillEnemies)] public int killTarget = 3;
        [AllowNesting] [ShowIf("type", ObjectiveType.KillEnemies)] public EnemyType enemyTypeTarget = null;
        [AllowNesting] [ShowIf("type", ObjectiveType.TalkToNPC)] public string npcReference = "";

        [AllowNesting] [ReadOnly] public bool objectiveComplete = false;
        [AllowNesting] [ReadOnly] public MiniLogObjective logObjective = null;

        public List<Objective> nextObjectives = new List<Objective>();       
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