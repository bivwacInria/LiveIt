using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Script
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        [SerializeField] private Sprite OpenLock;
        [SerializeField] private Sprite CloseLock;
        [SerializeField] private Image UserScreenLock;
        [SerializeField] private Image ExperienceScreenLock;
        private Coroutine makeLockDisappear { get; set; }
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
        /// Start function that initializes makeLockDisappear coroutine.
        /// </summary>
        private void Start()
        {
            makeLockDisappear = StartCoroutine(MakeLockDisappear());
        }
        /// <summary>
        /// CloseLockSprite function to close the lock sprite and make it disappear from the UI.
        /// </summary>
        public void CloseLockSprite()
        {
            UserScreenLock.sprite = CloseLock;
            ExperienceScreenLock.sprite = CloseLock;
            makeLockDisappear = StartCoroutine(UIManager.Instance.MakeLockDisappear());
        }

        /// <summary>
        /// Sets the sprite for UserScreenLock and ExperienceScreenLock to OpenLock,
        /// activates ExperienceScreenLock and UserScreenLock game objects, and stops
        /// the coroutine makeLockDisappear.
        /// </summary>
        public void OpenLockSprite()
        {
            UserScreenLock.sprite = OpenLock;
            ExperienceScreenLock.sprite = OpenLock;
            ExperienceScreenLock.gameObject.SetActive(true);
            UserScreenLock.gameObject.SetActive(true);
            StopCoroutine(makeLockDisappear);
        }
        
        /// <summary>
        /// Coroutine to make the lock disappear after a delay.
        /// </summary>
        public IEnumerator MakeLockDisappear()
        {
            yield return new WaitForSeconds(5f);
            UserScreenLock.GameObject().SetActive(false);
            ExperienceScreenLock.GameObject().SetActive(false);
        }
    }
}
