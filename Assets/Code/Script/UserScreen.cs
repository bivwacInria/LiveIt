using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Script
{
    public class UserScreen : MonoBehaviour
    {
        public static UserScreen Instance;
        [SerializeField] private TextMeshProUGUI consigne;
        [SerializeField] private List<string> consigneList;
        [SerializeField] private TextMeshProUGUI questionNumber;
        [SerializeField] private TextMeshProUGUI question;
        [SerializeField] private List<string> questions;
        [SerializeField] private TextMeshProUGUI prenomText;
        [SerializeField] private Animation animation;
        [SerializeField] private Image background;
        [SerializeField] private Image answer;
        [SerializeField] private Image image;
        public readonly List<string> Answers = new List<string>();
        private int _questionIndex = 0;
        private bool _noFreezingAfter5thQuestion = false;
        private Coroutine _freezeUntilTheatreStart;
        
        
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
        /// Initialize the function to set the 'prenomText' text to the value
        /// stored in the PlayerPrefs under the key "prenom".
        /// </summary>
        private void Start()
        {
            prenomText.text = PlayerPrefs.GetString("Prenom");
            StartCoroutine(TimerToUnFreeze());
        }
        
        /// <summary>
        /// Update function that checks for any key press and updates the answer text accordingly. 
        /// It also checks for the 'Return' key press and validates the answer.
        /// </summary>
        private void Update()
        {
            InputManager.Instance.HandleKeyboardInput();
            
            if (!InputManager.Instance.stopInput && Input.GetKeyDown(KeyCode.RightArrow))
            {
                ValidateAnswer();
            }
        }
        
        /// <summary>
        /// Increments the question index, adds the answer to the answers list,
        /// clears the answer field, updates the question and question number
        /// displayed.
        /// </summary>
        public void ValidateAnswer()
        {
            _questionIndex++;
            
            if (_questionIndex > questions.Count - 1) return;
            
            Answers.Add(InputManager.Instance.inputField.text);
            InputManager.Instance.inputField.text = "";
            
            StartCoroutine(FreezeAndWait());
        }
        
        /// <summary>
        /// Coroutine to freeze and wait for a specified time period.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FreezeAndWait()
        {
            Freeze();
            
            yield return new WaitForSeconds(2f);
            
            Unfreeze();
            
            question.text = questions[_questionIndex];
            questionNumber.text = "Question " + (_questionIndex + 1);
            
            if (_questionIndex == 5 && !_noFreezingAfter5thQuestion)
            {
                Freeze();
                consigne.text = consigneList[1];
            }
        }
        
        /// <summary>
        /// A coroutine that waits for 373 seconds before setting _noFreezingAfter5thQuestion to true and then calling Unfreeze().
        /// </summary>
        /// <returns></returns>
        private IEnumerator TimerToUnFreeze()
        {
            yield return new WaitForSeconds(373f);
            _noFreezingAfter5thQuestion = true;
            Unfreeze();
        }

        /// <summary>
        /// Freezes the animation and disables user input by setting the stopInput flag to true.
        /// Changes the background, answer, and image colors to a grayscale color.
        /// </summary>
        private void Freeze()
        {
            animation.gameObject.SetActive(true);
            animation.Play();
            InputManager.Instance.stopInput = true;
            Color color = new Color(0.75f, 0.75f, 0.75f);
            background.color = color;
            answer.color = color;
            image.color = color;
        }
        
        /// <summary>
        /// A function to unfreeze the state by stopping animation, enabling input, and setting colors to white.
        /// </summary>
        private void Unfreeze()
        {
            animation.Stop();
            InputManager.Instance.stopInput = false;
            Color color = Color.white;
            background.color = color;
            answer.color = color;
            image.color = color;
            animation.gameObject.SetActive(false);
        }
    }
}
