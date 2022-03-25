using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "Oasis/Quest", order = 1)]
    public class Quest : ScriptableObject
    {
        public string QuestName;
        public string QuestCategory;
        public List<Objective> objList = new List<Objective>();
        public List<QuestReward> rewardList = new List<QuestReward>();
    }
}