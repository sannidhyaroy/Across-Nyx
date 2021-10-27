using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    [System.Serializable]
    public class Dialogue
    {
        public int actor;

        [TextArea(3, 10)]
        public string dialogue;
    }

    [System.Serializable]
    public class Actor
    {
        public string name;
        public Sprite avatar;
    }
    public static class DialogueID
    {
        public static List<string> dialogueid = new List<string>();
        public static bool CheckID(string id)
        {
            return !dialogueid.Contains(id);
        }
    }
}
