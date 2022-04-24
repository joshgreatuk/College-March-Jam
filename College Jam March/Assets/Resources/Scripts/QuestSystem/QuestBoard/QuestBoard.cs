using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuestSystem
{
    public class QuestBoard : MonoBehaviour 
    {
        [Header("Prefabs")]
        public GameObject categoryPrefab;
        public GameObject questPrefab;
        [Space(5)]
        public GameObject questInfoPrefab;
        public GameObject objectivePrefab;
        public GameObject rewardPrefab;
        public GameObject textPrefab;
        public GameObject dividerPrefab;
        public GameObject buttonPrefab;
        [Header("References")]
        public Transform questListTransform;
        public Transform questInfoTransform;
        public TMP_Text questInfoTitle;
        [Header("Quests")]
        public List<Quest> boardQuestList = new List<Quest>();
        public List<Quest> unlockedQuestList = new List<Quest>();
        private List<BoardCategory> categoryList = new List<BoardCategory>();

        private List<GameObject> questListObjects = new List<GameObject>();
        private List<GameObject> questInfoObjects = new List<GameObject>();

        private int selectedQuest = 0;

        private void Start() 
        {
            UpdateList();
            selectedQuest = 0;
            PopulateQuestInfo();
        }

        public void UpdateList()
        {
            unlockedQuestList = new List<Quest>();
            foreach (Quest quest in boardQuestList)
            {
                if ((QuestLog.instance.unlockedQuests.Contains(quest) || !quest.questLocked) && !QuestLog.instance.CheckForQuest(quest.name))
                {
                    unlockedQuestList.Add(quest);
                }
            }

            for (int i=0; i < unlockedQuestList.Count; i++)
            {
                Quest quest = unlockedQuestList[i];
                BoardCategory category = GetCategory(quest.QuestCategory);
                if (category == null)
                {
                    GameObject categoryObject = Instantiate(categoryPrefab, questListTransform);
                    questListObjects.Add(categoryObject);
                    category = categoryObject.GetComponent<BoardCategory>();
                    categoryList.Add(category);
                    category.catName = quest.QuestCategory;
                    category.nameText.text = quest.QuestCategory;
                }

                GameObject questObject = Instantiate(questPrefab, category.transform);
                BoardQuest newQuest = questObject.GetComponent<BoardQuest>();
                category.quests.Add(newQuest);
                if (quest.questIcon != null) newQuest.questIcon.sprite = quest.questIcon;
                newQuest.questName.text = quest.QuestName;
                newQuest.objCount.text = $"Objectives: ({quest.objList.Count})";
                int x = i;
                newQuest.questButton.onClick.AddListener(() => { B_SelectQuest(x); });
                newQuest.quest = quest;
            }
        }

        public BoardCategory GetCategory(string catName)
        {
            BoardCategory result = null;
            foreach (BoardCategory category in categoryList)
            {
                if (category.catName == catName) result = category;
            }
            return result;
        }

        public void B_SelectQuest(int newIndex)
        {
            selectedQuest = newIndex;
            PopulateQuestInfo();
        }

        public void PopulateQuestInfo()
        {
            ClearQuestInfo();
            if (unlockedQuestList.Count < 1) return;
            Quest quest = unlockedQuestList[selectedQuest];
            questInfoTitle.text = $"Quest ({quest.QuestName})";

            LogQuestInfo questInfo = Instantiate(questInfoPrefab, questInfoTransform).GetComponent<LogQuestInfo>();
            questInfo.questDescription.text = quest.QuestDescription;
            if (quest.questGiver != null) questInfo.questGiver.text = $"Quest Giver ({quest.questGiver.npcName})";
            else questInfo.questGiver.text = $"Quest Board";
            questInfoObjects.Add(questInfo.gameObject);

            questInfoObjects.Add(Instantiate(dividerPrefab, questInfoTransform));

            LogText objText = Instantiate(textPrefab, questInfoTransform).GetComponent<LogText>();
            objText.textField.text = $"Objectives ({quest.objList.Count})";
            questInfoObjects.Add(objText.gameObject);

            foreach (Objective objective in quest.objList)
            {
                LogObjective logObjective = Instantiate(objectivePrefab, questInfoTransform).GetComponent<LogObjective>();
                questInfoObjects.Add(logObjective.gameObject);
                logObjective.objName.text = objective.publicName;
                logObjective.objDescription.text = objective.description;
                if (objective.type == ObjectiveType.KillEnemies)
                {
                    logObjective.objProgress.text = $"Target ({objective.killTarget})";
                }
                else logObjective.objProgress.text = "Target";
            }

            questInfoObjects.Add(Instantiate(dividerPrefab, questInfoTransform));

            LogText rewardText = Instantiate(textPrefab, questInfoTransform).GetComponent<LogText>();
            rewardText.textField.text = $"Rewards ({quest.rewardList.Count})";
            questInfoObjects.Add(rewardText.gameObject);

            foreach (QuestReward reward in quest.rewardList)
            {
                LogReward logReward = Instantiate(rewardPrefab, questInfoTransform).GetComponent<LogReward>();
                logReward.rewardName.text = reward.rewardName;
                if (reward.type == RewardType.XPReward)
                {
                    logReward.rewardInfo.text = $"+{reward.rewardAmount} XP";
                }
                questInfoObjects.Add(logReward.gameObject);
            }

            BoardButton acceptButton = Instantiate(buttonPrefab, questInfoTransform).GetComponent<BoardButton>();
            acceptButton.buttonText.text = "Accept Quest";
            acceptButton.button.onClick.AddListener(B_TakeSelectedQuest);
            questInfoObjects.Add(acceptButton.gameObject);
        }

        public void ClearQuestInfo()
        {
            foreach (GameObject obj in questInfoObjects)
            {
                Destroy(obj);
            }
            questInfoObjects = new List<GameObject>();
        }

        public void B_TakeSelectedQuest()
        {
            //Take the currently selected quest, remove it from the list, if category is now empty remove it, remap buttons, repopulate quest info
            Quest quest = unlockedQuestList[selectedQuest];
            QuestLog.instance.AddQuest(Instantiate(quest));
            unlockedQuestList.Remove(quest);
            BoardCategory category = GetCategory(quest.QuestCategory);
            if (category != null)
            {
                BoardQuest boardQuest = GetQuest(category.catName, quest);
                if (boardQuest != null)
                {
                    category.quests.Remove(boardQuest);
                }
                Destroy(boardQuest.gameObject);
                if (category.quests.Count < 1)
                {
                    Destroy(category.gameObject);
                }
            }
            RemapButtons();
            if (selectedQuest >= unlockedQuestList.Count) selectedQuest--;
            PopulateQuestInfo();
        }

        public BoardQuest GetQuest(string catName, Quest quest)
        {
            BoardQuest result = null;
            foreach (BoardCategory category in categoryList)
            {
                foreach (BoardQuest boardQuest in category.quests)
                {
                    if (boardQuest.quest == quest) result = boardQuest;
                }
            }
            return result;
        }

        public void RemapButtons()
        {
            //Go through buttons and reassign indexes
            foreach (BoardCategory category in categoryList)
            {
                for (int i=0; i < category.quests.Count; i++)
                {
                    BoardQuest quest = category.quests[i];
                    int x = i;
                    quest.questButton.onClick.AddListener(() => { B_SelectQuest(x); });
                }
            }
        }

        public void B_CloseBoard()
        {
            Player.PlayerRefs.instance.playerClass.UnlockMovement();
            Destroy(gameObject);
        }
    }
}