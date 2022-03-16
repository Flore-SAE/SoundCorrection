using System.Collections;
using UnityEngine;

namespace Gamekit3D
{
    /// <summary>
    /// Lance le son lié à son audio source toutes les X secondes
    /// Si randomDelays est faux, X = la durée du clip de l’audio source
    /// Sinon X = une valeur aléatoire comprise entre la durée du clip de l’audio source et 3 secondes
    /// À chaque fois qu’un son est joué, le pitch de l’audio source doit être une nouvelle valeur aléatoire entre minPitch et maxPitch
    /// À chaque fois qu’un son est joué, le volume de l’audio source doit être une nouvelle valeur aléatoire entre minVolume et maxVolume
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AmbientAudio : MonoBehaviour
    {
        public float minPitch = 0.99f;
        public float maxPitch = 1.01f;
        public float minVolume = 0.5f;
        public float maxVolume = 0.9f;
        public bool randomDelays = true;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            StartCoroutine(RepeatSound());
        }

        private IEnumerator RepeatSound()
        {
            // "Tant que tout le temps", formulation permettant de faire tourner une boucle à l'infinie
            while (true)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.volume = Random.Range(minVolume, maxVolume);
                audioSource.Play();
                // timeToWait représente le temps à attendre avant de relancer un son. On peut choisir une valeur fixe (le temps de durée du son actuel) ou une valeur aléatoire
                float timeToWait;
                if (randomDelays)
                    timeToWait = Random.Range(audioSource.clip.length, 3f);
                else
                    timeToWait = audioSource.clip.length;
                yield return new WaitForSeconds(timeToWait);
            }
        }
    }
}
