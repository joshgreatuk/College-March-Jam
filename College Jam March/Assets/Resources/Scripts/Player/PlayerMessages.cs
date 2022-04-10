using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Effects;

namespace Player
{
    [System.Serializable]
    public class PlayerMessage
    {
        public string title;
        public string message;
        public Sprite icon;

        public PlayerMessage(string msgTitle, string msgText)
        {
            title = msgTitle;
            message = msgText;
        }

        public PlayerMessage(string msgTitle, string msgText, Sprite msgicon)
        {
            title = msgTitle;
            message = msgText;
            msgicon = icon;
        }
    }

    public class PlayerMessages : MonoBehaviour 
    {
        public static PlayerMessages instance;
        void Awake()
        {
            instance = this;
        }

        public bool canPlay = true;

        public GameObject messageObject;
        public float messageDuration = 3f;
        public float transitionDuration = 0.5f;
        public Image messageIcon;
        public TMP_Text messageTitle;
        public TMP_Text messageText;
        private Coroutine queueCoroutine = null;

        public List<PlayerMessage> messageQueue = new List<PlayerMessage>();

        void FixedUpdate()
        {       
            if (queueCoroutine == null && messageQueue.Count > 0 && canPlay)
            {
                queueCoroutine = StartCoroutine(RunMessageQueue());
            }
        }

        IEnumerator RunMessageQueue()
        {
            messageObject.SetActive(true);
            while (messageQueue.Count > 0)
            {
                Image messageImage = messageObject.GetComponent<Image>();
                messageTitle.text = messageQueue[0].title;
                messageText.text = messageQueue[0].message;
                messageIcon.sprite = messageQueue[0].icon;

                messageTitle.color = new Color(0, 0, 0, 0);
                UIEffects.instance.UIPhaseIn(messageTitle, transitionDuration, Vector3.zero);
                messageText.color = new Color(0, 0, 0, 0);
                UIEffects.instance.UIPhaseIn(messageText, transitionDuration, Vector3.zero);
                UIEffects.instance.UIPhaseIn(messageIcon, transitionDuration, Vector3.zero);
                UIEffects.instance.UIPhaseIn(messageImage, transitionDuration, Vector3.zero);

                yield return new WaitForSeconds(messageDuration + transitionDuration);

                UIEffects.instance.UIPhaseOut(messageTitle, transitionDuration, Vector3.zero);
                UIEffects.instance.UIPhaseOut(messageText, transitionDuration, Vector3.zero);
                UIEffects.instance.UIPhaseOut(messageIcon, transitionDuration, Vector3.zero);
                UIEffects.instance.UIPhaseOut(messageImage, transitionDuration, Vector3.zero);

                yield return new WaitForSeconds(transitionDuration*2);

                messageQueue.RemoveAt(0);
            }
            messageObject.SetActive(false);
            queueCoroutine = null;
            yield return null;
        }
    }
}