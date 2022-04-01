using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    public class DialoguePlayer
    {
        public DialogueHandler dialogueHandler;
        public DialogueNode playerNode;

        private Logger logger;
        private List<GameObject> uiObjects = new List<GameObject>();

        private GameObject dialoguePanel;
        private Text dialogueText = null;

        private int nodeIndex = 0;

        public bool nextStage = true;
        public DialoguePlayerState overallState = DialoguePlayerState.Idle;

        public DialoguePlayer (DialogueHandler handler, DialogueNode node)
        {
            dialogueHandler = handler;
            playerNode = node;
            logger = handler.logger;
            logger.Log($"Dialogue player made and set up");
            dialoguePanel = UIRefs.instance.dialoguePanel;
        }

        //Move Player along after each stage
        IEnumerator PlayerLoop()
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
                            overallState = DialoguePlayerState.Speech;
                            nodeIndex = 0;
                            break;
                        case DialoguePlayerState.Speech:
                            //Play next speech node
                            if (nodeIndex == playerNode.speech.Count-1)
                            {
                                overallState = DialoguePlayerState.Decision;
                            }
                            else
                            {
                                HandleSpeech(playerNode.speech[nodeIndex]); 
                                nodeIndex += 1;
                                nextStage = false;
                            }
                            break;
                        case DialoguePlayerState.Decision:
                            //Make decision node
                            if (dialogueText == null)
                            {
                                //Handle speech then go to the decisions
                                HandleSpeech(playerNode.decisionSpeech, playerNode.decisions.Count*20);
                                nextStage = false;
                            }
                            else
                            {
                                HandleDecision(playerNode.decisions, playerNode.autoDecision);
                                overallState = DialoguePlayerState.Trigger;
                                nextStage = false;
                            }                           
                            break;
                        case DialoguePlayerState.Trigger:
                            //Do Triggers and finish
                            foreach (Trigger trigger in playerNode.triggers)
                            {
                                HandleTrigger(trigger);
                            }
                            nextStage = false;
                            overallState = DialoguePlayerState.Finished;
                            break;
                    }
                }
                yield return new WaitForFixedUpdate();
            }
            logger.Log($"Dialogue player has finished");
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
        }

        public void HandleSpeech(string speech, float verticalOffset = 0)
        {
            //Create textbox from prefab, size it to how big the text will be, start ScrollText
        }

        //From DialogueHandler, speed up or instantly put text in box
        public void MouseClicked()
        {
            if (!textDone)
            {
                switch (textStage)
                {
                    case 0:
                        textDelay = 0.1f;
                        break;
                    case 1:
                        textSkip = true;
                        textDelay = 0.25f;
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
        bool textSkip;
        bool textDone = false;
        IEnumerator ScrollText()
        {
            textDone = false;
            textSkip = false;
            textStage = 0;
            textDelay = 0.25f;
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
                    currentScrollText += textToScroll[currentScrollText.Length+1];
                    dialogueText.text = currentScrollText;
                }
            }
            textDone = true;
            if (overallState == DialoguePlayerState.Decision)
            {

            }
            yield return null;
        }

        //Create decision UI, this is called after the text is displayed
        public void HandleDecision(List<Decision> decisions, bool autoDecide)
        {

        }

        //Attack to buttons with index int
        public void DecisionChosen(int decisionIndex)
        {

        }

        public void HandleTrigger(Trigger trigger)
        {

        }
    }
}