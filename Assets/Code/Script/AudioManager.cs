using UnityEngine;

namespace Code.Script
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClipIl;
        [SerializeField] private AudioClip audioClipElle;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        
        /// <summary>
        /// Set the audio clip of the audio source based on the value of the "pronom" 
        /// player preference.
        /// </summary>
        private void Start()
        {
            audioSource.clip = PlayerPrefs.GetInt("Pronom") == 0 ? audioClipIl : audioClipElle;
            audioSource.Play();
        }
    }
}
