using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Gamekit3D
{
    /// <summary>
    /// Récupère la valeur d’un paramètre de mixer au démarrage et l’applique au slider
    /// Permet d’appliquer la valeur du slider à un paramètre de mixer
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class MixerSliderLink : MonoBehaviour
    {
        public AudioMixer mixer;
        // Le nom du paramètre du mixer à modifier
        public string mixerParameter;

        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void Start()
        {
            /*
             * GetFloat() permet de récupérer la valeur d'un paramètre de mixer
             * Il renvoie un booléen disant si le paramètre mixerParameter existe. 
             * Si le paramètre existe, sa valeur est enregistrée dans mixerValue 
             * Puis on l'applique au slider
             */
            if(mixer.GetFloat(mixerParameter, out var mixerValue))
                slider.value = mixerValue;
        }

        // Cette fonction est appelée directement par le slider quand sa valeur est modifiée à l'aide d'un UnityEvent
        public void SetMixerParameter(float value)
        {
            // On vient appliquer la valeur récupérée du slider au paramètre mixerParameter de notre mixer
            mixer.SetFloat(mixerParameter, value);
        }
    }
}
