using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AI;
using QuestSystem;
using Player;
using Inventory;
using Areas;

namespace QuestSystem
{
    public class QuestLog : MonoBehaviour
    {
        public static QuestLog instance;

        public Logger logger;

        public GameObject categoryPrefab;
        public GameObject questPrefab;
        public GameObject objectivePrefab;

        public GameObject questLog;
        public List<MiniLogCategory> questLogObjects = new List<MiniLogCategory>();
        public GameObject fullQuestLog;
        public LogFull logFull;

        public bool logShown = false;
        public List<Quest> questList = new List<Quest>();
        public List<Quest> finishedQuestList = new List<Quest>();
        
        private PlayerClass player;
        private bool addedLogListener = false;
        
        private void Awake()
        {
            instance = this;

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
            UpdateMiniLog();
            #endif
        }

        public void AddQuest(Quest quest)
        {
            logger.Log($"Added quest '{quest.name}'");
            PlayerMessages.instance.messageQueue.Add(new PlayerMessage($"{quest.name.Split('(')[0]}", "New quest"));
            questList.Add(Instantiate(quest));
            UpdateMiniLog();
        }

        public void AddQuest(Quest quest, NPCClass questGiver)
        {
            logger.Log($"Added quest '{quest.name}'");
            PlayerMessages.instance.messageQueue.Add(new PlayerMessage($"{quest.name.Split('(')[0]}", "New quest"));
            Quest newQuest = Instantiate(quest);
            newQuest.questGiver = questGiver;
            questList.Add(newQuest);
            UpdateMiniLog();
        }

        public void ToggleQuestLog()
        {
            if (fullQuestLog.activeInHierarchy)
            {
                player.UnlockMovement();
                fullQuestLog.SetActive(false);
            }
            else
            {
                player.LockMovement();
                fullQuestLog.SetActive(true);
                if (!addedLogListener)
                {
                    logFull.tabManager.E_ChangeTab.AddListener(UpdateFullLog);
                    addedLogListener = true;
                }
                UpdateFullLog(logFull.tabManager.currentTab);
            }
        }

        public void UpdateFullLog(int tab)
        {
            switch (tab)
            {
                case 0:
                    logFull.UpdateFullLog(questList);
                    break;
                case 1:
                    logFull.UpdateFullLog(finishedQuestList);
                    break;
            }
        }

        public void UpdateMiniLog()
        {
            //Update Stages
            //Loop through questList
            //Check if it has an object, if not create it
            //Check if its objectives have objects, if not create them, if they are completed then tick them and strikethrough the text
            //After checking all quest objects through, loop through questLogObjects
            //If QuestListCheck(quest object name) returns false destroy the quest object
            //After destroying missing quests loop through categories and destroy empty categories

            foreach (Quest quest in questList)
            {
                if (quest.logQuest == null)
                {
                    //If category doesnt exist, create it
                    MiniLogCategory newCat = GetMiniLogCategory(quest.QuestCategory);
                    if (newCat == null)
                    {
                        newCat = Instantiate(categoryPrefab, questLog.transform).GetComponent<MiniLogCategory>();
                        newCat.catName.text = quest.QuestCategory;
                        questLogObjects.Add(newCat);
                    }

                    //Create Quest Object
                    MiniLogQuest newQuest = Instantiate(questPrefab, newCat.transform).GetComponent<MiniLogQuest>();
                    quest.logQuest = newQuest;
                    newCat.logQuestList.Add(newQuest);
                    newQuest.questName.text = quest.QuestName;
                    
                    //Iterate through objectives and create their objects
                    foreach (Objective objective in quest.objList)
                    {
                        MiniLogObjective newObjective = Instantiate(objectivePrefab, newQuest.objectiveListTransform).GetComponent<MiniLogObjective>();
                        objective.logObjective = newObjective;
                        newQuest.logObjectiveList.Add(newObjective);
                        newObjective.objText.text = objective.publicName;
                        UpdateLogObjective(quest, objective);
                    }

                    //Update objective text
                    newQuest.objText.text = $"Objectives {GetObjectivesComplete(quest)}/{quest.objList.Count}";
                }
                else
                {
                    //Update objectives and add missing ones
                    foreach (Objective objective in quest.objList)
                    {
                        if (objective.logObjective == null)
                        {
                            MiniLogObjective newObjective = Instantiate(objectivePrefab, quest.logQuest.objectiveListTransform).GetComponent<MiniLogObjective>();
                            objective.logObjective = newObjective;
                            quest.logQuest.logObjectiveList.Add(newObjective);
                            newObjective.objText.text = objective.publicName;
                            UpdateLogObjective(quest, objective);
                        }
                        else
                        {
                            UpdateLogObjective(quest, objective);
                        }
                    }
                }
            }

            //Now look empty categories
            foreach (MiniLogCategory category in questLogObjects)
            {
                if (category.logQuestList.Count < 1)
                {
                    Destroy(category.gameObject);
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(questLog.GetComponent<RectTransform>());
        }

        public void UpdateLogObjective(Quest quest, Objective logObjective)
        {
            if (logObjective.type == ObjectiveType.KillEnemies)
            {
                logObjective.logObjective.objStatus.text = $"{logObjective.killStatus}/{logObjective.killTarget}";
            }
            else
            {
                logObjective.logObjective.objStatus.text = "";
            }
            logObjective.logObjective.objTick.isOn = logObjective.objectiveComplete;
            if (logObjective.objectiveComplete) logObjective.logObjective.objText.fontStyle = TMPro.FontStyles.Strikethrough;
            quest.logQuest.objText.text = $"Objectives {GetObjectivesComplete(quest)}/{quest.objList.Count}";
            LayoutRebuilder.ForceRebuildLayoutImmediate(questLog.GetComponent<RectTransform>());
        }

        //Get amount of objectives complete
        public int GetObjectivesComplete(Quest quest)
        {
            int result = 0;
            foreach (Objective obj in quest.objList)
            {
                if (obj.objectiveComplete) result++;
            }
            return result;
        }

        //Is category object made
        public MiniLogCategory GetMiniLogCategory(string catName)
        {
            MiniLogCategory result = null;
            foreach (MiniLogCategory category in questLogObjects)
            {
                if (category.catName.text == catName)
                {
                    result = category;
                }
            }
            return result;
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

        public bool CheckForQuest(string questName)
        {
            bool result = false;
            result = QuestListCheck(questName);
            if (!result) result = FinishedQuestListCheck(questName);
            return result;
        }

        private void FinishObjective(Quest quest, Objective objective)
        {
            logger.Log($"Objective '{objective.publicName}' of quest '{quest.name}' complete!");
            PlayerMessages.instance.messageQueue.Add(new PlayerMessage($"{quest.name}", $"Objective Complete '{objective.publicName}'"));
            objective.objectiveComplete = true;
            //Add any objectives that the objective comes with
            foreach (Objective obj in objective.nextObjectives)
            {
                logger.Log($"Added objective '{obj.publicName}' to '{quest.name}'");
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
                    case RewardType.ItemReward:
                        player.playerInventory.AddItem(Instantiate(reward.targetItem));
                        break;
                    case RewardType.UnlockAreaReward:
                        AreaController.instance.UnlockArea(reward.targetArea);
                        break;
                }
            }

            MiniLogCategory miniLogCategory = GetMiniLogCategory(quest.QuestCategory);
            if (miniLogCategory != null && quest.logQuest != null)
            {
                miniLogCategory.logQuestList.Remove(quest.logQuest);
                Destroy(quest.logQuest.gameObject);
            }
            StartCoroutine(DelayMiniLogUpdate());
        }

        IEnumerator DelayMiniLogUpdate()
        {
            yield return new WaitForSeconds(0.1f);
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
                    if (objective.type == ObjectiveType.KillEnemies && objective.enemyTypeTarget.name + "(Clone)" == enemy.name && !objective.objectiveComplete)
                    {   
                        objective.killStatus += 1;
                        if (objective.killStatus >= objective.killTarget)
                        {
                            //Finish objective
                            FinishObjective(quest, objective);
                        }
                        else
                        {
                            UpdateLogObjective(quest, objective);
                        }
                    }
                }
            }
        }

        public void M_GatherItem(InventoryItem item)
        {
            for (int i=0; i < questList.Count; i++)
            {
                Quest quest = questList[i];
                for (int j=0; j < quest.objList.Count; j++)
                {
                    Objective objective = quest.objList[j];
                    if (objective.type == ObjectiveType.GatherItems && objective.targetItem.publicName == item.publicName && !objective.objectiveComplete)
                    {
                        FinishObjective(quest, objective);
                    }
                }
            }
        }

        public void M_VisitArea(Area area)
        {
            for (int i=0; i < questList.Count; i++)
            {
                Quest quest = questList[i];
                foreach (Objective objective in quest.objList)
                {
                    if (objective.type == ObjectiveType.VisitArea && objective.targetArea == area.areaName && !objective.objectiveComplete)
                    {   
                        FinishObjective(quest, objective);
                    }
                }
            }
        }

        public void M_TalkToNPC(NPCClass NPC)
        {
            for (int i=0; i < questList.Count; i++)
            {
                Quest quest = questList[i];
                foreach (Objective objective in quest.objList)
                {
                    if (objective.type == ObjectiveType.TalkToNPC && objective.npcReference == NPC.npcReference && !objective.objectiveComplete)
                    {   
                        FinishObjective(quest, objective);
                    }
                }
            }
        }
    }
}