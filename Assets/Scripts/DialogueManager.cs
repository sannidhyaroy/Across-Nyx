using System;
using System.Collections;
using System.Collections.Generic;
using Dialogues;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MainScript
{
    public class DialogueManager : MonoBehaviour
    {
        public Image avatar;
        public TextMeshProUGUI avatarName;
        public TextMeshProUGUI dialogueText;
        //public RectTransform BackgroundBox;
        private Dialogue[] currentDialogue;
        private Actor[] currentActors;
        private int activeDialogues = 0;
        //[SerializeField] private float DialogueTypeSpeedDelay;
        private Touch touch;
        private GameObject canvas;
        private GameObject player;
        [SerializeField] private Animator animator;
        public static bool isDialogueActive = false;
        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 0;
            canvas = GameObject.Find("Canvas");
            player = GameObject.FindGameObjectWithTag("Player");
            canvas.SetActive(false);
            player.SetActive(false);
            //BackgroundBox.transform.localScale = Vector3.zero;
        }
        // Update is called once per frame
        /*void Update()
        {
            if (touch.phase == TouchPhase.Began && Input.touchCount > 0 && isDialogueActive)
            {
                NextMsg();
            }
            if (Input.GetKeyDown(KeyCode.Space) && isDialogueActive)
            {
                NextMsg();
            }
        }*/
        public void StartDialogue(Dialogue[] dialogues, Actor[] actors)
        {
            currentDialogue = dialogues;
            currentActors = actors;
            activeDialogues = 0;
            isDialogueActive = true;
            Debug.Log("Conversation started! Loaded " + dialogues.Length + " messages");
            //BackgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo().setIgnoreTimeScale(true);
            //animator.Play("Dialogue box In anim");
            //StartCoroutine(Waiter(0.4f));
            //Invoke("DisplayMsg", 1);
            DisplayMsg();
        }
        public void DisplayMsg()
        {
            Dialogue dialogueToDisplay = currentDialogue[activeDialogues];
            //dialogueText.text = dialogueToDisplay.dialogue;
            StopAllCoroutines();
            StartCoroutine(TypeDialogue(dialogueToDisplay.dialogue));
            Actor actorToDisplay = currentActors[dialogueToDisplay.actor];
            avatarName.text = actorToDisplay.name;
            avatar.sprite = actorToDisplay.avatar;
        }
        public void NextMsg()
        {
            activeDialogues++;
            if (activeDialogues < currentDialogue.Length)
            {
                DisplayMsg();
            }
            else
            {
                Debug.Log("Conversation ended!");
                animator.Play("Dialogue Box out anim");
                isDialogueActive = false;
                //DialogueTrigger.hasKohakuIntroDialogueOccurred = 1;
                //PlayerPrefs.SetInt("hasKohakuIntroDialogueOccurred", FindObjectOfType<DialogueTrigger>().hasKohakuIntroDialogueOccurred);
                Time.timeScale = 1;
                Invoke("disableanim", 1f);
                canvas.SetActive(true);
                player.SetActive(true);
                //this.gameObject.SetActive(false);
            }
        }
        IEnumerator TypeDialogue(string sentence)
        {
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }
        IEnumerator Waiter (float waitTime)
        {
            return new WaitForSecondsRealtime(waitTime);
        }
        void disableanim ()
        {
            this.gameObject.SetActive(false);
            //return new WaitForSecondsRealtime(0.4f);
        }
    }
}
