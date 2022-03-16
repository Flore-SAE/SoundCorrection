using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit3D
{
    /// <summary>
    /// Ancien manager pour les audio sources de musique
    /// Peut être réutilisé pour faciliter les transitions entre musiques
    /// </summary>
    public class SoundTrack : MonoBehaviour
    {
        // On drag & drop l'AudioSource de la musique principale pour la différencier des musique "locales", c'est à dire celles liées à une zone/un collider
        public AudioSource mainMusic;

        private AudioSource[] localMusics;

        private void Awake()
        {
            // On récupère un tableau de tous les SoundtrackVolume en enfant de cet objet
            var soundtrackVolumes = GetComponentsInChildren<SoundTrackVolume>();
            // On crée une liste d'AudioSources
            var localMusicsList = new List<AudioSource>();
            /* 
             * Pour chaque enfant possédant un script SoundtrackVolume, on ajoute l'AudioSource qui lui est attachée à la liste
             * 
             * Plus précisément : foreach(var soundtrackVolume in soundtrackVolumes) -> Pour chaque élément présent dans la liste soundtrackVolumes,
             * on crée une variable soundtrackVolume qui représente chaque SoundtrackVolume 1 par 1
             * Puis pour chacun des soundtrackVolume trouvés, on ajoute à la liste localMusicList le component AudioSource attaché à l'objet qui a un SoundtrackVolume
             * 
             * Tldr: on isole les AudioSource qui doivent se déclencher à des moments précis par rapport à l'AudioSource de la musique principale
             */
            foreach (var soundtrackVolume in soundtrackVolumes)
            {
                localMusicsList.Add(soundtrackVolume.GetComponent<AudioSource>());
            }
            // On transforme la liste précédente en tableau car sa longueur ne variera plus par la suite
            localMusics = localMusicsList.ToArray();
        }

        private void Start()
        {
            PlayMainMusic();
        }

        /// <summary>
        /// Va augmenter le volume d'une AudioSource précise et diminuer tous les autres
        /// </summary>
        /// <param name="source"></param>
        public void PlayLocalMusic(AudioSource source)
        {
            // On arrête les précédentes coroutines au cas où on modifiait toujours leur volume
            StopAllCoroutines();
            // On arrête la musique principale en 1 seconde
            StartCoroutine(ChangeVolumeOverTime(mainMusic, 0, 1));
            // Pour chacune des AudioSource en enfant qui ne représente pas la musique principale
            foreach(var localMusic in localMusics)
            {
                // Si on veut activer cette AudioSource précise
                if(localMusic == source)
                    // On augmente son volume jusqu'à 100% en une seconde
                    StartCoroutine(ChangeVolumeOverTime(localMusic, 1, 1));
                else
                    // Sinon on diminue son volume jusqu'à 0% en une seconde
                    StartCoroutine(ChangeVolumeOverTime(localMusic, 0, 1));
            }
        }

        public void PlayMainMusic()
        {
            // On arrête les précédentes coroutines au cas où on modifiait toujours leur volume
            StopAllCoroutines();
            // On augmente le son de la musique principale jusqu'à 100% en une seconde
            StartCoroutine(ChangeVolumeOverTime(mainMusic, 1, 1));
            // Et pour les autres AudioSource, on réduit leur volume à 0% en une seconde
            foreach (var localMusic in localMusics)
            {
                StartCoroutine(ChangeVolumeOverTime(localMusic, 0, 1));
            }
        }

        /// <summary>
        /// Cette coroutine va bouger le volume de l'AudioSource "source" jusqu'à "targetVolume" en "delay" secondes 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetVolume"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator ChangeVolumeOverTime(AudioSource source, float targetVolume, float delay)
        {
            // Phase d'initialisation
            var startVolume = source.volume;
            var startTime = Time.time;
            var endTime = startTime + delay;
            // Boucle
            while (Time.time < endTime)
            {
                // Le temps passé en secondes depuis le lancement de la coroutine
                var timePassed = Time.time - startTime;
                // Le pourcentage d'avancée de la coroutine par rapport au temps passé
                var completion = timePassed / delay;
                // On applique un pourcentage de volume entre le startVolume et le targetVolume selon le pourcentage d'avancée
                source.volume = Mathf.Lerp(startVolume, targetVolume, completion);
                // On attend une frame
                yield return null;
            }
            // On s'assure qu'à la fin de la coroutine on a la bonne valeur et non 0.9898988999998999999999 au lieu de 1 par exemple
            source.volume = targetVolume;
        }
    }
}
