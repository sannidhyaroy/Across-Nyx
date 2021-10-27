using UnityEngine;
using UnityEngine.Audio;
using System;

namespace MainScript
{
    public class AudioManager : MonoBehaviour
    {

        public AudioMixerGroup audioMixer;
        public Sounds[] sounds;

        private void Awake()
        {
            foreach (Sounds s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.outputAudioMixerGroup = audioMixer;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Play(string name)
        {
            Sounds s = Array.Find(sounds, sounds => sounds.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound with name '" + name + "' does not exist! Please input a valid name");
                return;
            }
            s.source.Play();
        }

        public void PlayDelayed(string name, float delaytime)
        {
            Sounds s = Array.Find(sounds, sounds => sounds.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound with name '" + name + "' does not exist! Please input a valid name");
                return;
            }
            s.source.PlayDelayed(delaytime);
        }

        public void Stop(string sound)
        {
            Sounds s = Array.Find(sounds, sounds => sounds.name == sound);
            if (s == null)
            {
                Debug.LogWarning("Sound with name '" + sound + "' does not exist! Please input a valid name");
                return;
            }

            //s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
            //s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

            s.source.Stop();
        }
    }
}
