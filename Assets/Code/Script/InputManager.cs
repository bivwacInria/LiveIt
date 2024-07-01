using System;
using System.Collections.Generic;
using System.Net;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using System.Linq;

namespace Code.Script
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;
        [SerializeField] private HandGrabInteractable experienceScreen;
        [SerializeField] private HandGrabInteractable userScreen;
        [SerializeField] private GameObject handLeft;
        [SerializeField] private GameObject handRight;
        [SerializeField] public TMP_InputField inputField;
        private bool _isLock = true;
        private bool _isMaj = false;
        private bool _capsLock = false;
        private bool _nextIsCirconflexe = false;
        private bool _nextIsTrema = false;
        public bool stopInput = false;
        private Dictionary<KeyCode, char> keyboardRemappingBase = new Dictionary<KeyCode, char>();
        private Dictionary<KeyCode, char> keyboardRemappingShift = new Dictionary<KeyCode, char>();
        static readonly KeyCode[] _keyCodes =
            System.Enum.GetValues(typeof(KeyCode))
                .Cast<KeyCode>()
                .Where(k => k < KeyCode.Mouse0)
                .ToArray();

        /// <summary>
        /// This function initializes key mappings for specific key codes and their corresponding characters.
        /// </summary>
        private void Start()
        {
            AddKeyMapping(KeyCode.Q, 'a', 'A');
            AddKeyMapping(KeyCode.W, 'z', 'Z');
            AddKeyMapping(KeyCode.A, 'q', 'Q');
            AddKeyMapping(KeyCode.Z, 'w', 'W');
            AddKeyMapping(KeyCode.Semicolon, 'm', 'M');
            AddKeyMapping(KeyCode.Quote, 'ù', '%');
            AddKeyMapping(KeyCode.Hash, '*', 'µ');
            AddKeyMapping(KeyCode.M, ',', '?');
            AddKeyMapping(KeyCode.Comma, ';', '.');
            AddKeyMapping(KeyCode.Period, ':', '/');
            AddKeyMapping(KeyCode.Slash, '!', '§');
            AddKeyMapping(KeyCode.Alpha1, '&', '1');
            AddKeyMapping(KeyCode.Alpha2, 'é', '2');
            AddKeyMapping(KeyCode.Alpha3, '"', '3');
            AddKeyMapping(KeyCode.Alpha4, '\'', '4');
            AddKeyMapping(KeyCode.Alpha5, '(', '5');
            AddKeyMapping(KeyCode.Alpha6, '-', '6');
            AddKeyMapping(KeyCode.Alpha7, 'è', '7');
            AddKeyMapping(KeyCode.Alpha8, '_', '8');
            AddKeyMapping(KeyCode.Alpha9, 'ç', '9');
            AddKeyMapping(KeyCode.Alpha0, 'à', '0');
            AddKeyMapping(KeyCode.Minus, ')', '°');
            AddKeyMapping(KeyCode.RightBracket, '$', '£');
        }
        
        /// <summary>
        /// AddKeyMapping method adds a key mapping to the keyboards remapping dictionaries.
        /// </summary>
        /// <param name="key">KeyCode</param>
        /// <param name="baseChar">char</param>
        /// <param name="shiftChar">optional char</param>
        private void AddKeyMapping(KeyCode key, char baseChar, char? shiftChar)
        {
            keyboardRemappingBase.Add(key, baseChar);
            if (shiftChar.HasValue)
            {
                keyboardRemappingShift.Add(key, shiftChar.Value);
            }
        }
        
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
        /// Update function to handle input from the OVRInput and perform corresponding actions.
        /// </summary>
        private void Update()
        {
            if (experienceScreen != null && userScreen != null)
            {
                if (OVRInput.GetDown(OVRInput.RawTouch.A))
                    LockUnlockScreenInteractable();
                else if (OVRInput.GetDown(OVRInput.RawTouch.B))
                    SpacialAnchorManager.Instance.SaveAnchors();
                else if (OVRInput.GetDown(OVRInput.RawTouch.X))
                    SpacialAnchorManager.Instance.ResetAnchors();
                else if (OVRInput.GetDown(OVRInput.RawTouch.Y))
                    SpacialAnchorManager.Instance.RemoveSavedAnchors();
                else if (OVRInput.GetDown(OVRInput.RawButton.Start))
                    GoToPreExperienceScreen();
            }
        }
        
        /// <summary>
        /// This function navigates to the pre-experience screen by loading the "Main" scene.
        /// </summary>
        public void GoToPreExperienceScreen()
        {
            SceneManager.LoadScene("Main");
        }
        
        /// <summary>
        /// Handles the keyboard input and updates the inputField accordingly based on the keys pressed.
        /// </summary>
        public void HandleKeyboardInput()
        {
            if(stopInput || !Input.anyKeyDown)
                return;
            
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || _capsLock)
            {
                _isMaj = true;
            }
            else
            {
                _isMaj = false;
            }
             
            var keys = GetCurrentKeys().ToArray();

            if (!keys.Any()) return;
            
            if (_isMaj && keyboardRemappingShift.ContainsKey(keys.First()))
            {
                UpdateInputField(keyboardRemappingShift[keys.First()].ToString());
            }
                
            else if (keyboardRemappingBase.ContainsKey(keys.First()))
            {
                UpdateInputField(keyboardRemappingBase[keys.First()].ToString());
            }

            else if (_isMaj && Input.GetKeyDown(KeyCode.LeftBracket))
            {
                if(_nextIsTrema)
                    UpdateInputField("¨");
                else
                {
                    _nextIsTrema = true;
                }
            }

            else if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                if(_nextIsCirconflexe)
                    UpdateInputField("^");
                else
                {
                    _nextIsCirconflexe = true;
                }
            }
            
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                inputField.text = inputField.text.Length > 1 ? inputField.text.Substring(0, inputField.text.Length - 1) : "";
            }

            else if (Input.GetKeyDown(KeyCode.Return))
            {
                inputField.text += "\n";
            }
            
            else if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                _capsLock = !_capsLock;
            }
            
            // else if (Input.GetKey(KeyCode.Backspace))
            // {
            //     //do nothing
            // }
            
            else if (Input.inputString != "")
            {
                UpdateInputField(Input.inputString);
            }
        }
        
        /// <summary>
        /// Get current keys pressed on keyboard including special keys like <see cref="KeyCode.LeftControl"/>
        /// </summary>
        /// <returns>Iterator with pressed keys. If nothing is pressed, empty iterator is returned</returns>
        /// <remarks>Be careful with FirstOrDefault. It will return KeyCode.None if nothing is pressed because of its implementation</remarks>
        public static IEnumerable<KeyCode> GetCurrentKeys()
        {
            for (int i = 0; i < _keyCodes.Length; i++) 
                if (Input.GetKey(_keyCodes[i])) 
                    yield return _keyCodes[i];
        }
        
        /// <summary>
        /// A method to update the answer with different versions of characters based on certain conditions.
        /// </summary>
        /// <param name="newChar"></param>
        private void UpdateInputField(string newChar)
        {
            if (_nextIsCirconflexe)
            {
                if(_isMaj)
                    inputField.text += GetTheCirconflexeVersion(newChar.ToUpper());
                else
                    inputField.text += GetTheCirconflexeVersion(newChar);
                _nextIsCirconflexe = false;
            }
            else if (_nextIsTrema)
            {
                if(_isMaj)
                    inputField.text += GetTheTremaVersion(newChar.ToUpper());
                else
                    inputField.text += GetTheTremaVersion(newChar);
                _nextIsTrema = false;
            }
            else if(_isMaj)
                inputField.text += newChar.ToUpper();
            else
                inputField.text += newChar;
        }
        
        /// <summary>
        /// Retrieves the circumflex version of the input character.
        /// </summary>
        /// <param name="newChar"></param>
        /// <returns></returns>
        private string GetTheCirconflexeVersion(string newChar)
        {
            switch (newChar)
            {
                case "a":
                    return "â";
                case "e":
                    return "ê";
                case "i":
                    return "î";
                case "o":
                    return "ô";
                case "u":
                    return "û";
                default:
                    return "^" + newChar;
            }
        }
        
        /// <summary>
        /// Retrieves the trema version of the input character.
        /// </summary>
        /// <param name="newChar"></param>
        /// <returns></returns>
        private string GetTheTremaVersion(string newChar)
        {
            switch (newChar)
            {
                case "e":
                    return "ë";
                case "i":
                    return "ï";
                case "u":
                    return "ü";
                default:
                    return "¨" + newChar;
            }
        }
        
        /// <summary>
        /// Toggle the interactivity of the experienceScreen and userScreen
        /// </summary>
        private void LockUnlockScreenInteractable()
        {
            _isLock = !_isLock;
            
            experienceScreen.enabled = !_isLock;
            userScreen.enabled = !_isLock;
            
            if (_isLock)
            {
                UIManager.Instance.CloseLockSprite();
                handLeft.SetActive(false);
                handRight.SetActive(false);
            }
            else
            {
                UIManager.Instance.OpenLockSprite();
                handLeft.SetActive(true);
                handRight.SetActive(true);
            }
        }
    }
}
