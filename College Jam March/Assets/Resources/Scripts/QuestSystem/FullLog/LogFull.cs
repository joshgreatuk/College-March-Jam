using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class LogFull : MonoBehaviour 
    {
        //For references on the prefab
        public TabManager tabManager;
        public LogPage questPage;
        public LogPage finishedQuestPage;

        [Space(10)]
        public GameObject categoryPrefab;
        public GameObject questPrefab;
        [Space(5)]
        public GameObject questInfoPrefab;
        public GameObject objectivePrefab;
        public GameObject rewardPrefab;
        public GameObject textPrefab;
        public GameObject dividerPrefab;

        LogPage selectedPage;
        public List<Quest> questListCached = new List<Quest>();
        Quest selectedQuest;

        //Do everything here
        public void UpdateFullLog(List<Quest> questList)
        {
            switch (tabManager.currentTab)
            {
                case 0:
                    selectedPage = questPage;
                    ClearPageList();
                    ClearPageInfo();
                    UpdatePageList(questPage, questList);
                    break;
                case 1:
                    selectedPage = finishedQuestPage;
                    ClearPageList();
                    ClearPageInfo();
                    UpdatePageList(finishedQuestPage, questList);
                    break;
            }
        }

        //Called when a quest in the list is clicked
        public void SelectQuest(int questIndex)
        {
            if (questListCached[questIndex] != selectedQuest)
            {
                ClearPageInfo();
                selectedQuest = questListCached[questIndex];
                UpdatePageInfo(selectedQuest);
            }
        }

        //Create the new list of quests on the selected page
        public void UpdatePageList(LogPage page, List<Quest> questList)
        {
            questListCached.Clear();
            questListCached.AddRange(questList);

            //Draw the quest list with buttons and categories
            for (int i=0; i < questList.Count; i++)
            {
                Quest quest = questList[i];
                LogCategory category = GetLogCategory(quest.QuestCategory);
                if (category == null)
                {
                    category = Instantiate(categoryPrefab, selectedPage.questList).GetComponent<LogCategory>();
                    category.title.text = quest.QuestCategory;
                    page.logListObjects.Add(category);
                }
                LogQuest logQuest = Instantiate(questPrefab, category.transform).GetComponent<LogQuest>();
                category.logQuestList.Add(logQuest);
                logQuest.questName.text = quest.QuestName;
                logQuest.objectiveText.text =  $"Objectives: {GetObjectivesComplete(quest)}/{quest.objList.Count}";
                if (quest.questIcon != null) logQuest.questIcon.sprite = quest.questIcon;
                if (quest.questGiver != null) logQuest.questGiver.text = quest.questGiver.npcName;
                else logQuest.questGiver.text = "";
                int x = i;
                logQuest.questButton.onClick.AddListener(() => SelectQuest(x));
            }
            if (questList.Count > 0)
            {
                selectedQuest = questList[0];
                UpdatePageInfo(selectedQuest);
            }   
        }

        //Create the info page of the selected quest
        public void UpdatePageInfo(Quest quest)
        {
            //Draw the quest info, then objectives, then rewards
            selectedPage.infoTitle.text = $"Quest ({quest.QuestName})";

            LogQuestInfo questInfo = Instantiate(questInfoPrefab, selectedPage.questInfo).GetComponent<LogQuestInfo>();
            questInfo.questDescription.text = quest.QuestDescription;
            if (quest.questGiver != null) questInfo.questGiver.text = $"Quest Giver ({quest.questGiver.npcName})";
            else questInfo.questGiver.text = $"Quest Giver (Quest Board)";
            selectedPage.logInfoObjects.Add(questInfo.gameObject);

            selectedPage.logInfoObjects.Add(Instantiate(dividerPrefab, selectedPage.questInfo));

            LogText objText = Instantiate(textPrefab, selectedPage.questInfo).GetComponent<LogText>();
            objText.textField.text = $"Objectives ({GetObjectivesComplete(quest)}/{quest.objList.Count})";
            selectedPage.logInfoObjects.Add(objText.gameObject);

            foreach (Objective objective in quest.objList)
            {
                LogObjective logObjective = Instantiate(objectivePrefab, selectedPage.questInfo).GetComponent<LogObjective>();
                selectedPage.logInfoObjects.Add(logObjective.gameObject);
                logObjective.objName.text = objective.publicName;
                logObjective.objDescription.text = objective.description;
                if (objective.type == ObjectiveType.KillEnemies)
                {
                    logObjective.objProgress.text = $"Progress ({objective.killStatus}/{objective.killTarget})";
                }
                else logObjective.objProgress.text = "Progress";
                logObjective.objToggle.isOn = objective.objectiveComplete;
                if (objective.objectiveComplete)
                {
                    logObjective.objName.fontStyle = TMPro.FontStyles.Strikethrough;
                    logObjective.objDescription.fontStyle = TMPro.FontStyles.Strikethrough;
                    logObjective.objProgress.fontStyle = TMPro.FontStyles.Strikethrough;
                }
            }

            selectedPage.logInfoObjects.Add(Instantiate(dividerPrefab, selectedPage.questInfo));

            LogText rewardText = Instantiate(textPrefab, selectedPage.questInfo).GetComponent<LogText>();
            rewardText.textField.text = $"Rewards ({quest.rewardList.Count})";
            selectedPage.logInfoObjects.Add(rewardText.gameObject);

            foreach (QuestReward reward in quest.rewardList)
            {
                LogReward logReward = Instantiate(rewardPrefab, selectedPage.questInfo).GetComponent<LogReward>();
                logReward.rewardName.text = reward.rewardName;
                if (reward.type == RewardType.XPReward)
                {
                    logReward.rewardInfo.text = $"+{reward.rewardAmount} XP";
                }
                selectedPage.logInfoObjects.Add(logReward.gameObject);
            }
        }

        //Clear the list of quests
        public void ClearPageList()
        {
            foreach (LogCategory obj in selectedPage.logListObjects)
            {
                Destroy(obj.gameObject);
            }
            selectedPage.logListObjects = new List<LogCategory>();
        }

        //Clears the page info on the quest
        public void ClearPageInfo()
        {
            foreach (GameObject obj in selectedPage.logInfoObjects)
            {
                Destroy(obj);
            }
            selectedPage.logInfoObjects = new List<GameObject>();
        }

        public LogCategory GetLogCategory(string target)
        {
            LogCategory result = null;
            foreach (LogCategory category in selectedPage.logListObjects)
            {
                if (category.title.text == target) result = category;
            }
            return result;
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
    }
}