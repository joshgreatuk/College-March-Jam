using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public enum DialoguePlayerState
    {
        Idle,
        Playing,
        Speech,
        Decision,
        Trigger
    }

    public class DialoguePlayer
    {
        public DialogueHandler dialogueHandler;
        public DialogueNode playerNode;
        private Logger logger;
        private List<GameObject> uiObjects = new List<GameObject>();

        public DialoguePlayerState playerState = DialoguePlayerState.Idle;

        public DialoguePlayer (DialogueHandler handler, DialogueNode node)
        {
            dialogueHandler = handler;
            playerNode = node;
            logger = handler.logger;
            logger.Log($"Dialogue player made and set up");
        }

        //Move Player along after each stage
        IEnumerator PlayerLoop()
        {
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

        public void HandleSpeech(string speech)
        {
            //
        }

        //From DialogueHandler, speed up or instantly put text in box
        public void MouseClicked()
        {

        }

        IEnumerator ScrollText()
        {
            //Can use this to scroll the text across the text box with variable delay
            yield return null;
        }

        //Set up decision UI prefabs and change state to decision
        public void HandleDecision(List<Decision> decisions)
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