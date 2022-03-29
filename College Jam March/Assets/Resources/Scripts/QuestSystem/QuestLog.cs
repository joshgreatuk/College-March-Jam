using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using QuestSystem;
using Player;

namespace QuestSystem
{
    public class QuestLog : MonoBehaviour
    {
        public Logger logger;

        public bool logShown = true;
        public List<Quest> questList = new List<Quest>();
        public List<Quest> finishedQuestList = new List<Quest>();
        
        private PlayerClass player;
        
        private void Awake() 
        {
            //Subscribe to events
            EventHandler.instance.E_KillEnemy.AddListener(M_KillEnemy);
            EventHandler.instance.E_GatherItem.AddListener(M_GatherItem);
            EventHandler.instance.E_VisitArea.AddListener(M_VisitArea);
            EventHandler.instance.E_TalkToNPC.AddListener(M_TalkToNPC);

            player = PlayerRefs.instance.playerObject.GetComponent<PlayerClass>();

            #if UNITY_EDITOR
            List<Quest> instantQuests = new List<Quest>();
            foreach (Quest quest in questList)
            {
                instantQuests.Add(Object.Instantiate(quest));
            }
            questList = instantQuests;
            #endif
        }

        private void FinishObjective(Quest quest, Objective objective)
        {
            logger.Log($"Objective '{objective}' of quest '{quest}' complete!");
            objective.objectiveComplete = true;
            CheckQuestFinish(quest);
        }

        public void CheckQuestFinish(Quest quest)
        {
            bool questComplete = true;
            foreach (Objective obj in quest.objList)
            {
                if (!obj.objectiveComplete)
                {
                    questComplete = false;
                }
            }
            if (questComplete)
            {
                FinishQuest(quest);
            }
        }

        private void FinishQuest(Quest quest)
        {
            logger.Log($"Quest '{quest}' complete!");
            questList.Remove(quest);
            finishedQuestList.Add(quest);

            //Give rewards
            foreach (QuestReward reward in quest.rewardList)
            {
                switch (reward.type)
                {
                    case RewardType.XPReward:
                        player.AddXP(reward.rewardAmount);
                        break;
                }
            }
        }

        public void M_KillEnemy(EnemyType enemy)
        {
            for (int i=0; i < questList.Count; i++)
            {
                Quest quest = questList[i];
                foreach (Objective objective in quest.objList)
                {
                    if (objective.type == ObjectiveType.KillEnemies && objective.target.name + "(Clone)" == enemy.name)
                    {   
                        objective.killStatus += 1;
                        if (objective.killStatus >= objective.killTarget)
                        {
                            //Finish objective
                            FinishObjective(quest, objective);
                        }
                    }
                }
            }
        }

        public void M_GatherItem()
        {

        }

        public void M_VisitArea()
        {

        }

        public void M_TalkToNPC(NPCClass NPC)
        {
            for (int i=0; i < questList.Count; i++)
            {
                Quest quest = questList[i];
                foreach (Objective objective in quest.objList)
                {
                    if (objective.type == ObjectiveType.TalkToNPC && objective.npcReference == NPC.npcReference)
                    {   
                        FinishObjective(quest, objective);
                    }
                }
            }
        }
    }
}