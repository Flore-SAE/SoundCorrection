using UnityEngine;

namespace Gamekit3D
{
    /// <summary>
    /// Quand cet objet est trigger, il doit fade out les autres musiques et fade in sa propre musique
    /// Quand cet objet n’est plus trigger, il doit fade out sa musique et fade in la musique de base
    /// </summary>
    public class SoundTrackVolume : MonoBehaviour
    {
        private AudioSource audioSource;
        // On récupère le script Soundtrack qu'on a écrit et qui est appliqué au parent des SoundtrackVolume
        private SoundTrack soundTrack;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            soundTrack = GetComponentInParent<SoundTrack>();
        }

        private void OnTriggerEnter(Collider other)
        {
            /*
             * Si le parent le plus haut de la hiérarchie de l'objet avec lequel on collisionné possède le tag "Player"
             * Cela permet d'éviter de devoir appliquer le bon tag à l'ensemble des objets constituant l'avatar
             * Attention ! Ce tag n'est pas appliqué par défaut sur le prefab d'Ellen, il faut le modifier
             */

            if(other.transform.root.CompareTag("Player"))
                soundTrack.PlayLocalMusic(audioSource);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.CompareTag("Player"))
                soundTrack.PlayMainMusic();
        }
    }
}
