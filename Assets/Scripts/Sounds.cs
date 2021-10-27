using UnityEngine;
using UnityEngine.Audio;

namespace MainScript
{
    [System.Serializable]
    public class Sounds
    {
        public string name; // name of the clip

        public AudioClip clip; // reference to an audio clip

        [Range(0f, 1f)]
        public float volume; // volume of the clip

        [Range(.1f, 3f)]
        public float pitch; // pitch of the clip

        public bool loop;

        [HideInInspector]
        public AudioSource source;

    }
}
