using UnityEngine;

namespace Gamekit3D
{
    /// <summary>
    /// Permet de jouer un son aléatoire parmi un ensemble de sons donnés
    /// La variable audioSource doit être remplie
    /// La variable clip doit toujours contenir le clip en cours de lecture
    /// Les variables playing et canPlay sont utilisées ailleurs et n’ont pas besoin d’être modifiées ni utilisées
    /// Si randomizePitch = true, chaque son lancé doit avoir un pitch compris entre 1 - pitchRandomRange et 1 + pitchRandomRange
    /// Chaque son est joué avec playDelay délai
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioPlayer : MonoBehaviour
    {
        public bool randomizePitch = true;
        public float pitchRandomRange = 0.2f;
        public float playDelay = 0;
        // On ajoute un tableau de sons parmi lesquels sélectionner un son aléatoire
        public AudioClip[] clips;

        [HideInInspector]
        public bool playing;
        [HideInInspector]
        public bool canPlay;

        public AudioSource audioSource { get; private set; }

        public AudioClip clip { get; private set; }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayRandomClip()
        {
            // Si on a oublié de mettre au moins un clip dans l'inspecteur, on arrête la fonction afin d'éviter les erreurs
            if (clips.Length == 0)
                return;
            var clipIndex = Random.Range(0, clips.Length);
            // On spécifie que le clip actuellement en cours de lecture est un clip aléatoire du tableau clips
            clip = clips[clipIndex];
            // On applique ce clip comme son assigné à l'audio source
            audioSource.clip = clip;
            if (randomizePitch)
                audioSource.pitch = Random.Range(1 - pitchRandomRange, 1 + pitchRandomRange);
            audioSource.PlayDelayed(playDelay);
        }
    }
}
