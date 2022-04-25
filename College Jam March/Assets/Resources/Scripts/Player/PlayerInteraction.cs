using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using AI;
using Dialogue;
using Areas;

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
            OnPickup, //On an item pickup
            AreaZone, //On an area transition
            Busy, //Busy interacting, no movement
        }
        public Logger logger;

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
                        logger.Log($"Player in NPCZone '{npcClass.npcName}'");
                        break;
                    //Show InteractMsg and use interaction class
                    case "InteractZone":
                        playerPrompt.text = $"Press {interactionKey} to interact with this interactable";
                        UIEffects.instance.UIPhaseIn(playerPromptBack, 0.5f, Vector3.zero, 1, 0, 0.75f);
                        UIEffects.instance.UIPhaseIn(playerPrompt, 0.5f, Vector3.zero, 1, 0, 0.75f);
                        interactionState = InteractionStates.InteractZone;
                        interactionCollider = other;
                        logger.Log($"Player in InteractZone", interactionCollider.gameObject);
                        break;
                    case "AreaZone":
                        Area area = other.gameObject.transform.parent.gameObject.GetComponent<Area>();
                        bool areaUnlocked = false;
                        if (area.GetType().ToString() == "Areas.AreaWorld") { AreaWorld areaWorld = (AreaWorld)area; areaUnlocked = areaWorld.targetArea.unlocked; }
                        if (area.GetType().ToString() == "Areas.AreaScene") 
                        { 
                            AreaScene areaWorld = (AreaScene)area; areaUnlocked = areaWorld.unlocked; 
                            playerClass.spawnPointReference = areaWorld.spawnPoint;
                        }
                        if (areaUnlocked)
                        { 
                            playerPrompt.text = $"Press {interactionKey} to move to {area.destinationName}";
                            interactionState = InteractionStates.AreaZone;
                            interactionCollider = other;
                            logger.Log($"Player in AreaZone", interactionCollider.gameObject);
                        }
                        else playerPrompt.text = area.lockedText;
                        UIEffects.instance.UIPhaseIn(playerPromptBack, 0.5f, Vector3.zero, 1, 0, 0.75f);
                        UIEffects.instance.UIPhaseIn(playerPrompt, 0.5f, Vector3.zero, 1, 0, 0.75f);
                        break;
                }
            }
        }

        private void OnTriggerExit(Collider other) 
        {
            if (other.gameObject.name == "InteractionZone")
            {
                if (System.Enum.GetName(typeof(InteractionStates),interactionState) == other.gameObject.tag)
                {
                    UIEffects.instance.UIPhaseOut(playerPromptBack, 0.5f, Vector3.zero, 1, 0, 0.8f);
                    UIEffects.instance.UIPhaseOut(playerPrompt, 0.5f, Vector3.zero, 1, 0, 0.8f);
                }
                logger.Log($"Player left InteractionZone", other.gameObject);
                interactionState = InteractionStates.Idle;
                interactionCollider = null;
            }
        }

        public void OnPickupTooltip(Inventory.InventoryItem item)
        {
            playerPrompt.text = $"Press {interactionKey} to pick up with {item.publicName} ({item.quantity})";
            UIEffects.instance.UIPhaseIn(playerPromptBack, 0.5f, Vector3.zero, 1, 0, 0.75f);
            UIEffects.instance.UIPhaseIn(playerPrompt, 0.5f, Vector3.zero, 1, 0, 0.75f);
            interactionState = InteractionStates.OnPickup;
        }

        public void OnPickupTooltipDestroyed()
        {
            UIEffects.instance.UIPhaseOut(playerPromptBack, 0.5f, Vector3.zero, 1, 0, 0.8f);
            UIEffects.instance.UIPhaseOut(playerPrompt, 0.5f, Vector3.zero, 1, 0, 0.8f);
            interactionState = InteractionStates.Idle;
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
                            playerClass.LockMovement(true);
                            NPCClass npcClass = interactionCollider.transform.parent.gameObject.GetComponent<NPCClass>();
                            EventHandler.instance.E_TalkToNPC.Invoke(npcClass);
                            if (npcClass.zoomToNpc)
                            {
                                playerClass.transform.LookAt(npcClass.transform.position);
                                playerClass.CameraZoomToNPC();
                                npcClass.NameBarToZoom(playerClass.cameraZoomPoint.eulerAngles);
                                logger.Log($"Zoomed Camera");
                            }
                            DialogueHandler.instance.npcTalkingTo = npcClass;
                            DialogueHandler.instance.AddToQueue(npcClass.npcDialogue.nodeList[0]);
                            logger.Log($"Started conversation with '{npcClass.npcName}'");
                        }
                        break;
                    case InteractionStates.InteractZone:
                        break;
                    case InteractionStates.OnPickup:
                        if (playerClass.itemPickup != null && playerClass.itemPromptShown)
                        {
                            playerClass.playerInventory.AddItem(playerClass.itemPickup.item);
                            EventHandler.instance.E_GatherItem.Invoke(playerClass.itemPickup.item);
                            Destroy(playerClass.itemPickup.pickupUI);
                            Destroy(playerClass.itemPickup.gameObject);
                            playerClass.itemPickup = null;
                            OnPickupTooltipDestroyed();
                        }
                        interactionState = InteractionStates.Idle;
                        break;
                    case InteractionStates.AreaZone:
                        Area area = interactionCollider.transform.parent.gameObject.GetComponent<Area>();
                        if (area.GetType().ToString() == "Areas.AreaWorld")
                        {
                            AreaWorld areaWorld = (AreaWorld)area;
                            Image loadScreen = UIRefs.instance.loadingScreen.GetComponent<Image>();
                            UIEffects.instance.UIPhaseIn(loadScreen, 2f, Vector3.zero);
                            StartCoroutine(PhaseOutAfter(3f, loadScreen, 2f));
                            StartCoroutine(TeleportPlayerAfter(2f, areaWorld.targetArea.spawnPoint.position, areaWorld.targetArea));
                        }
                        else if (area.GetType().ToString() == "Areas.AreaScene")
                        {
                            AreaScene areaScene = (AreaScene)area;
                            GameManager.instance.LoadLevel((int)areaScene.targetArea, areaScene.GetScene());
                            playerClass.FreezePlayer();
                            StartCoroutine(TeleportPlayerAfter(1f, areaScene.spawnPoint, areaScene));
                            UIEffects.instance.UIPhaseOut(playerPromptBack, 0.5f, Vector3.zero, 1, 0, 0.8f);
                            UIEffects.instance.UIPhaseOut(playerPrompt, 0.5f, Vector3.zero, 1, 0, 0.8f);
                            interactionState = InteractionStates.Idle;
                            interactionCollider = null;
                            foreach (Transform child in UIRefs.instance.itemPickupTransform)
                            {
                                Destroy(child.gameObject);
                            }
                        }
                        interactionCollider = null;
                        interactionState = InteractionStates.Idle;
                        break;
                }
            }
        }
        
        public IEnumerator TeleportPlayerAfter(float seconds, Vector3 position, Area area)
        {
            yield return new WaitForSeconds(seconds);
            playerClass.transform.position = position;
            position.y = playerClass.playerCamera.transform.position.y;
            playerClass.playerCamera.transform.position = position;
            EventHandler.instance.E_VisitArea.Invoke(area);
            logger.Log("Player and Camera teleported");
        }

        public IEnumerator PhaseOutAfter(float seconds, Image image, float phaseTime)
        {
            yield return new WaitForSeconds(seconds);
            UIEffects.instance.UIPhaseOut(image, phaseTime, Vector3.zero);
        }
    }
}