using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogues;

namespace MainScript
{
    public class DialogueTrigger : MonoBehaviour
    {
        public GameObject DialogueBox;
        public string dialogueID;
        public Dialogue[] dialogues;
        public Actor[] actors;
        //[HideInInspector] public static int hasKohakuIntroDialogueOccurred = 0;
        //public Dialogue dialogue;
        private void Awake()
        {
            Debug.Log("Dialogue ID: " + dialogueID);
            if (DialogueID.CheckID(dialogueID))
            {
                Debug.Log("Dialogue with ID " + dialogueID + " has never been shown to the user!");
                DialogueBox.SetActive(true);
                TriggerDialogue();
            }
            else
            {
                Debug.Log("Dialogue with ID " + dialogueID + " has already been shown to the user!");
                DialogueBox.SetActive(false);
                FindObjectOfType<DialogueManager>().enabled = false;
            }
            //hasKohakuIntroDialogueOccurred = PlayerPrefs.GetInt("hasKohakuIntroDialogueOccurred", 0);
            /*if (hasKohakuIntroDialogueOccurred == 1)
            {
                //this.gameObject.SetActive(false);
            }*/
        }
        public void TriggerDialogue()
        {
            DialogueID.dialogueid.Add(dialogueID);
            FindObjectOfType<DialogueManager>().StartDialogue(dialogues, actors);
        }
    }
}
