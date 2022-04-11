using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "Oasis/Quest", order = 1)]
    public class Quest : ScriptableObject
    {
        public string QuestName;
        public string QuestCategory;
        [TextArea(1,7)] public string QuestDescription;
        public List<Objective> objList = new List<Objective>();
        public List<QuestReward> rewardList = new List<QuestReward>();

        [ReadOnly] public AI.NPCClass questGiver = null;
        [ReadOnly] public MiniLogQuest logQuest = null;
    }
}