using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using QuestSystem;

namespace Dialogue
{
    public enum DialoguePlayerState
    {
        Idle,
        Speech,
        Decision,
        Trigger,
        Finished
    }

    [System.Serializable]
    public class DialoguePlayer
    {
        public DialogueHandler dialogueHandler;
        public DialogueNode playerNode;

        private Logger logger;
        private List<GameObject> uiObjects = new List<GameObject>();

        private GameObject dialoguePanel;
        private TMP_Text dialogueText = null;

        private int nodeIndex = 0;
        private bool decisionSpeechDone = false;

        public bool playing = true;
        public bool nextStage = true;
        public DialoguePlayerState overallState = DialoguePlayerState.Idle;

        public DialoguePlayer (DialogueHandler handler, DialogueNode node, Logger passedLogger)
        {
            dialogueHandler = handler;
            playerNode = node;
            logger = passedLogger;
            logger.Log($"Player made and set up");
            dialoguePanel = UIRefs.instance.dialoguePanel;
        }

        //Move Player along after each stage
        public IEnumerator PlayerLoop()
        {
            logger.Log($"Dialogue player started");
            while (overallState != DialoguePlayerState.Finished)
            {
                if (nextStage)
                {
                    //Go to next stage
                    switch (overallState)
                    {
                        case DialoguePlayerState.Idle:
                            //Change to speech
                            logger.Log("Idle - Speech");
                            overallState = DialoguePlayerState.Speech;
                            nodeIndex = 0;
                            break;
                        case DialoguePlayerState.Speech:
                            //Play next speech node
                            if (playerNode.speech.Count > 0){
                                if (nodeIndex == playerNode.speech.Count)
                                {
                                    logger.Log("Speech - Decision");
                                    overallState = DialoguePlayerState.Decision;
                                }
                                else
                                {
                                    nextStage = false;
                                    HandleSpeech(playerNode.speech[nodeIndex]);
                                    nodeIndex += 1;
                                }
                            }
                            else
                            {
                                logger.Log("Speech - Decision");
                                overallState = DialoguePlayerState.Decision;
                            }
                            break;
                        case DialoguePlayerState.Decision:
                            //Make decision node
                            if (playerNode.decisions.Count > 0) 
                            {
                                if (playerNode.decisionSpeech.Trim() == "") decisionSpeechDone = true;

                                if (playerNode.decisionSpeech != "" && !decisionSpeechDone)
                                {
                                    //Handle speech then go to the decisions
                                    nextStage = false;
                                    HandleSpeech(playerNode.decisionSpeech, playerNode.decisions.Count*20);
                                    decisionSpeechDone = true;
                                }
                                else if (decisionSpeechDone)
                                {
                                    nextStage = false;
                                    HandleDecision(playerNode.decisions, playerNode.autoDecision);
                                    decisionSpeechDone = false;
                                }
                            }
                            else
                            {
                                logger.Log("Decision - Trigger");
                                overallState = DialoguePlayerState.Trigger; 
                            }
                            break;
                        case DialoguePlayerState.Trigger:
                            //Do Triggers and finish
                            nextStage = false;
                            foreach (Trigger trigger in playerNode.triggers)
                            {
                                HandleTrigger(trigger);
                            }
                            
                            logger.Log("Trigger - Finished");
                            overallState = DialoguePlayerState.Finished;
                            break;
                    }
                }
                yield return new WaitForFixedUpdate();
            }
            logger.Log($"Dialogue player has finished");
            playing = false;
            yield return null;
        }

        public void ClearUIObjects()
        {
            for (int i=0; i < uiObjects.Count; i++)
            {
                GameObject currentObject = uiObjects[i];
                GameObject.Destroy(currentObject);
            }
            uiObjects = new List<GameObject>();
            dialogueText = null;
        }

        public void HandleSpeech(string speech, float verticalOffset = 0)
        {
            //Create textbox from prefab, size it to how big the text will be, start ScrollText
            if (speech.Trim() == "") nextStage = true;
            else
            {
                if (dialogueText == null)
                {
                    dialogueText = MonoBehaviour.Instantiate(UIRefs.instance.dialogueTextPrefab, dialoguePanel.transform).GetComponent<TMP_Text>();
                    uiObjects.Add(dialogueText.gameObject);
                }
                textToScroll = speech;
                dialogueHandler.StartCoroutine(ScrollText());
            }
        }

        //From DialogueHandler, speed up or instantly put text in box
        public void MouseClicked()
        {
            if (!textDone)
            {
                switch (textStage)
                {
                    case 0:
                        textDelay = 0.03f;
                        break;
                    case 1:
                        textSkip = true;
                        textDelay = 0.075f;
                        break;
                }
                textStage += 1;
            }
            else if (overallState == DialoguePlayerState.Speech)
            {
                ClearUIObjects();
                nextStage = true;
            }
        }

        //Can use this to scroll the text across the text box with variable delay
        int textStage; //0 - normal, 1 - sped up, 2 - skip
        float textDelay;
        string textToScroll;
        string currentScrollText;
        bool textSkip = false;
        bool textDone = false;
        IEnumerator ScrollText()
        {
            textDone = false;
            textSkip = false;
            textStage = 0;
            textDelay = 0.075f;
            currentScrollText = "";
            while (currentScrollText.Length != textToScroll.Length)
            {
                yield return new WaitForSeconds(textDelay);
                if (textSkip)
                {
                    currentScrollText = textToScroll;
                    dialogueText.text = textToScroll;
                }
                else
                {
                    currentScrollText += textToScroll[currentScrollText.Length];
                    dialogueText.text = currentScrollText;
                }
            }
            textDone = true;
            if (overallState == DialoguePlayerState.Decision)
            {
                nextStage = true;
            }
            yield return null;
        }

        //Create decision UI, this is called after the text is displayed
        public void HandleDecision(List<Decision> decisions, bool autoDecide)
        {
            if (autoDecide)
            {
                for (int i=0; i < decisions.Count; i++)
                {
                    //Check if the condition is true and use the first one that is true
                    if (ConditionCheck(decisions[i]))
                    {
                        if (decisions[i].decisionPath != null)
                        {
                            dialogueHandler.AddToQueue(decisions[i].decisionPath, decisions[i].decisionPathIndex);
                            if (decisions[i].playInstantly)
                            {
                                logger.Log("Decision - Finished");
                                overallState = DialoguePlayerState.Finished;
                                playing = false;
                            }
                            else
                            {
                                logger.Log("Decision - Trigger");
                                overallState = DialoguePlayerState.Trigger;
                                nextStage = true;
                            }
                        }
                    }
                }
            }
            else
            {
                logger.Log("Creating decision buttons");
                for (int i=0; i < decisions.Count; i++)
                {
                    //If the conditions are met, make the decision buttons
                    if (ConditionCheck(decisions[i]) || decisions[i].conditions.Count < 1)
                    {
                        int j = i;
                        GameObject newButton = MonoBehaviour.Instantiate(UIRefs.instance.dialogueDecisionPrefab, dialoguePanel.transform);
                        TMP_Text buttonText = newButton.transform.Find("Text").GetComponent<TMP_Text>();
                        newButton.GetComponent<Button>().onClick.AddListener(delegate { DecisionChosen(j); });
                        buttonText.text = decisions[i].decisionText;
                        uiObjects.Add(newButton);
                    }
                }
            }
        }

        //Attach to buttons with index int
        public void DecisionChosen(int decisionIndex)
        {
            Decision chosenDecision = playerNode.decisions[decisionIndex];
            logger.Log($"Decision chosen '{chosenDecision.decisionText}'");
            ClearUIObjects();
            if (chosenDecision.decisionPath != null)
            {
                dialogueHandler.AddToQueue(chosenDecision.decisionPath, chosenDecision.decisionPathIndex);
                if (chosenDecision.playInstantly)
                {
                    overallState = DialoguePlayerState.Finished;
                    playing = false;
                }
                else
                {
                    logger.Log("Decision - Trigger");
                    overallState = DialoguePlayerState.Trigger;
                }
            }
            nextStage = true;
        }

        public bool ConditionCheck(Decision checkDecision)
        {
            bool decision = false;
            foreach (DecisionConditionClass conditionClass in checkDecision.conditions)
            {
                DecisionCondition condition = conditionClass.decisionCondition;
                switch (conditionClass.decisionCondition)
                {
                    case DecisionCondition.QuestGiven:
                        decision = dialogueHandler.questLog.QuestListCheck(conditionClass.questGivenNeeded.name);
                        if (!decision)
                        { decision = dialogueHandler.questLog.FinishedQuestListCheck(conditionClass.questGivenNeeded.name); }
                        break;
                    case DecisionCondition.QuestCompleted:
                        decision = dialogueHandler.questLog.FinishedQuestListCheck(conditionClass.questCompleteNeeded.name);
                        break;
                    case DecisionCondition.ConditionMet:
                        decision = dialogueHandler.dialogueConditions.GetConditionState(conditionClass.conditionName);
                        break;
                }

                if (!conditionClass.conditionNeeded) decision = !decision;
                if (!decision) break;
            }
            return decision;
        }

        public void HandleTrigger(Trigger trigger)
        {
            switch (trigger.triggerType)
            {
                case TriggerType.AddQuest:
                    dialogueHandler.questLog.AddQuest(trigger.questTarget, dialogueHandler.npcTalkingTo);
                    break;
                case TriggerType.AddDialogue:
                    dialogueHandler.AddToQueue(trigger.dialogueObject.nodeList[trigger.dialogueIndex]);
                    break;
                case TriggerType.OpenMenu:
                    break;
                case TriggerType.SetCondition:
                    dialogueHandler.dialogueConditions.SetConditionState(trigger.conditionName, trigger.conditionState);
                    break;
            }
        }
    }
}