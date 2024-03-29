using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using AI;
using Effects;

namespace Player
{
    [RequireComponent(typeof(PlayerClass))]
    public class PlayerInteraction : MonoBehaviour
    {
        public enum InteractionStates
        {
            Idle, //Not in interaction collider
            NPCZone, //On NPC interaction collider
            InteractZone, //On general interaction collider
            Busy, //Busy interacting, no movement
        }

        public string interactionKey = "E"; //IMPLEMENT INTO SETTINGS MENU

        public InteractionStates interactionState = InteractionStates.Idle;
        public Collider interactionCollider = null;

        private Collider playerCollider;
        private TMP_Text playerPrompt;
        private Image playerPromptBack;
        private PlayerClass playerClass;

        private void Awake() 
        {
            playerCollider = GetComponent<Collider>();
            playerPrompt = UIRefs.instance.playerPrompt;
            playerPromptBack = playerPrompt.transform.parent.gameObject.GetComponent<Image>();
            playerClass = GetComponent<PlayerClass>();
        }

        private void OnTriggerEnter(Collider other) 
        {
            //If the player is in an interaction collider, check if it is npc or something else
            if (other.gameObject.name == "InteractionZone")
            {
                switch(other.gameObject.tag)
                {
                    //Show interact NPC message and use NPC class
                    case "NPCZone":
                        NPCClass npcClass = other.gameObject.transform.parent.gameObject.GetComponent<NPCClass>();
                        playerPrompt.text = $"Press {interactionKey} to interact with {npcClass.npcName}";
                        UIEffects.instance.UIPhaseIn(playerPromptBack, 0.5f, Vector3.zero, 1, 0, 0.75f);
                        UIEffects.instance.UIPhaseIn(playerPrompt, 0.5f, Vector3.zero, 1, 0, 0.75f);
                        interactionState = InteractionStates.NPCZone;
                        interactionCollider = other;
                        break;
                    //Show InteractMsg and use interaction class
                    case "InteractZone":
                        playerPrompt.text = $"Press {interactionKey} to interact with this interactable";
                        UIEffects.instance.UIPhaseIn(playerPromptBack, 0.5f, Vector3.zero, 1, 0, 0.75f);
                        UIEffects.instance.UIPhaseIn(playerPrompt, 0.5f, Vector3.zero, 1, 0, 0.75f);
                        interactionState = InteractionStates.InteractZone;
                        interactionCollider = other;
                        break;
                }
            }
        }

        private void OnTriggerExit(Collider other) 
        {
            if (other.gameObject.name == "InteractionZone")
            {
                Debug.Log(System.Enum.GetName(typeof(InteractionStates),interactionState));
                if (System.Enum.GetName(typeof(InteractionStates),interactionState) == other.gameObject.tag)
                {
                    UIEffects.instance.UIPhaseOut(playerPromptBack, 0.5f, Vector3.zero, 1, 0, 0.8f);
                    UIEffects.instance.UIPhaseOut(playerPrompt, 0.5f, Vector3.zero, 1, 0, 0.8f);
                }
                interactionCollider = null;
            }
        }

        public void InteractButtonPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                switch(interactionState)
                {
                    case InteractionStates.NPCZone:
                        if (interactionCollider.transform.parent.gameObject.GetComponent<NPCClass>().canTalk)
                        {
                            //Start Conversation
                            playerClass.canMove = false;
                            NPCClass npcClass = interactionCollider.transform.parent.gameObject.GetComponent<NPCClass>();
                            EventHandler.instance.E_TalkToNPC.Invoke(npcClass);
                            if (npcClass.zoomToNpc)
                            {
                                playerClass.transform.LookAt(npcClass.transform.position);
                                playerClass.CameraZoomToNPC();
                                npcClass.NameBarToZoom(playerClass.cameraZoomPoint.eulerAngles);
                            }
                        }
                        break;
                    case InteractionStates.InteractZone:
                        break;
                }
            }
        }
    }
}