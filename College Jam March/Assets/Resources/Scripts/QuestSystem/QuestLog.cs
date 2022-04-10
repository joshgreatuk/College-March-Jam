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

        public GameObject categoryPrefab;
        public GameObject questPrefab;
        public GameObject objectivePrefab;

        public GameObject questLog;
        public List<LogCategory> questLogObjects = new List<LogCategory>();
        public GameObject fullQuestLog;
        public List<LogCategory> fullQuestLogObjects = new List<LogCategory>();

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
                instantQuests.Add(Instantiate(quest));
            }
            questList = instantQuests;
            #endif
        }

        public void ClearFullLogObjects()
        {
            for (int i=0; i < fullQuestLogObjects.Count; i++)
            {
                Destroy(fullQuestLogObjects[i]);
            }
            fullQuestLogObjects = new List<LogCategory>();
        }

        public void AddQuest(Quest quest)
        {
            logger.Log($"Added quest '{quest.name}'");
            PlayerMessages.instance.messageQueue.Add(new PlayerMessage($"{quest.name.Split('(')[0]}", "New quest"));
            questList.Add(Instantiate(quest));
            UpdateMiniLog();
        }

        public void ToggleQuestLog()
        {
            if (questLog.activeInHierarchy)
            {
                player.UnlockMovement();
                questLog.SetActive(false);
            }
            else
            {
                player.LockMovement();
                questLog.SetActive(true);
                ClearFullLogObjects();
                PopulateFullLog();
            }
        }

        public void PopulateFullLog()
        {

        }

        public void UpdateMiniLog()
        {
            
        }

        //Is quest in the list
        public bool QuestListCheck(string questName)
        {
            bool result = false;
            foreach (Quest quest in questList)
            {
                if (questName == quest.name.Split('(')[0])
                {
                    result = true;
                }
            }
            return result;
        }
        
        public bool FinishedQuestListCheck(string questName)
        {
            bool result = false;
            foreach (Quest quest in finishedQuestList)
            {
                if (questName == quest.name.Split('(')[0])
                {
                    result = true;
                }
            }
            return result;
        }

        private void FinishObjective(Quest quest, Objective objective)
        {
            logger.Log($"Objective '{objective.name}' of quest '{quest.name}' complete!");
            PlayerMessages.instance.messageQueue.Add(new PlayerMessage($"{quest.name}", $"Objective Complete '{objective.publicName}'"));
            objective.objectiveComplete = true;
            //Add any objectives that the objective comes with
            foreach (Objective obj in objective.nextObjectives)
            {
                logger.Log($"Added objective '{obj.name}' to '{quest.name}'");
                PlayerMessages.instance.messageQueue.Add(new PlayerMessage($"{quest.name}", $"New objective '{obj.publicName}'"));
                quest.objList.Add(obj);
            }
            UpdateMiniLog();
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
            PlayerMessages.instance.messageQueue.Add(new PlayerMessage($"{quest.name}", $"Quest Completed"));
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

            UpdateMiniLog();
        }

        public void M_KillEnemy(EnemyType enemy)
        {
            for (int i=0; i < questList.Count; i++)
            {
                Quest quest = questList[i];
                for (int j=0; j < quest.objList.Count; j++)
                {
                    Objective objective = quest.objList[j];
                    if (objective.type == ObjectiveType.KillEnemies && objective.enemyTypeTarget.name + "(Clone)" == enemy.name)
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