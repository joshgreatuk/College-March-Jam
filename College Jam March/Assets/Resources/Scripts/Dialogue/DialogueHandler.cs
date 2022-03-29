using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialogue
{
    public class DialogueHandler : MonoBehaviour
    {
        /* SUMMARY 
        Dialogue Handler will instantiate the dialogue UI, and control the dialogue of the NPC and the choices of the player
        Dialogue is to be made and put into classes by an editor

        Conversations will go through text only then to the decisions
        The decisions will have a text box then decision boxes, prefabs are in UIRefs
        After decisions, the triggers will be done and if there is no other dialogues in the queue
        the conversation will end and the camera will zoom back to normal, aswell as the name box being put back.

        2 classes
        Dialogue Handler - Handles queuing of dialogue and any actions it needs to, monobehaviour
        Dialogue Player - runs through a DialogueNode and displays it on the UI
        */

        public Logger logger;
        public static DialogueHandler instance;
        
        public List<DialogueNode> dialogueQueue = new List<DialogueNode>();

        private DialoguePlayer dialoguePlayer;
        private Coroutine queueLoopCoroutine = null;

        private void Awake() 
        {
            instance = this;
        }

        //Check for if there is queued dialogue, if there is then start the player if not started
        private void FixedUpdate()
        {
            if (dialogueQueue.Count > 0 && queueLoopCoroutine == null)
            {
                queueLoopCoroutine = StartCoroutine(QueueLoop());
            }
        }

        public void MouseClicked (InputAction.CallbackContext context)
        {
            if (context.performed && dialoguePlayer != null)
            {
                dialoguePlayer.MouseClicked();
            }
        }

        public void AddToQueue(DialogueNode nodeToAdd)
        {
            logger.Log($"Dialogue Node Added '{nodeToAdd.description}'");
            dialogueQueue.Add(nodeToAdd);
        }

        IEnumerator QueueLoop()
        {
            while (dialogueQueue.Count > 0)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}