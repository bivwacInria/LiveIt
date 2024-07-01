using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Code.Script
{
    public class Setup : MonoBehaviour
    {
        public static Setup Instance;

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

        private void Start()
        {
            StartCoroutine(waitAndEndSimulation());
        }

        private IEnumerator waitAndEndSimulation()
        {
            yield return new WaitForSeconds(480f);
            InputManager.Instance.GoToPreExperienceScreen();
        }
    }
}
