using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Script
{
    public class MainScene : MonoBehaviour
    {
        [SerializeField] private Toggle _toggleIl;
        [SerializeField] private Toggle _toggleElle;

        /// <summary>
        /// Start method that sets the input field text
        /// to empty and toggles the _toggleIl to true.
        /// </summary>
        private void Start()
        {
            InputManager.Instance.inputField.text = "";
            _toggleIl.isOn = true;
        }

        /// <summary>
        /// Update function that handles keyboard input using the InputManager class.
        /// </summary>
        void Update()
        {
            InputManager.Instance.HandleKeyboardInput();
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SwitchPronom();
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SwitchPronom();
            }
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                GoToScene1();
            }
        }
        
        /// <summary>
        /// SwitchPronom method toggles between the _toggleIl and _toggleElle based
        /// on the state of _toggleIl.
        /// </summary>
        private void SwitchPronom()
        {
            if (_toggleIl.isOn)
            {
                _toggleElle.isOn = true;
            }
            else
            {
                _toggleIl.isOn = true;
            }
        }

        /// <summary>
        /// A function that sets player preferences for "Pronom" and "Prenom" based
        /// on the state of a toggle and the input field, and then loads "Scene1".
        /// </summary>
        public void GoToScene1()
        {
            PlayerPrefs.SetInt("Pronom", _toggleIl.isOn ? 0 : 1);
            PlayerPrefs.SetString("Prenom", InputManager.Instance.inputField.text);
            SceneManager.LoadScene("Scene1");
        }
    }
}
