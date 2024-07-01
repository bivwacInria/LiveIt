using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

namespace Code.Script
{
    public class ScrollTextManager : MonoBehaviour
    {
        [SerializeField] private List<String> textList = new List<string>();
        [SerializeField] private List<int> indexsTextToReplace;
        [SerializeField] private List<int> indexsAnswerToShow;
        [SerializeField] private List<string> prefacesTextToReplace;
        [SerializeField] private int indexPrenomAtStart;
        [SerializeField] private TMP_Text scrollText;
        [SerializeField] private float timeBetweenEachTextChange;
        private int _indexText = 1;
        private int _indexAnswers = 0;
        private string prenom;
        
        /// <summary>
        /// This function is called when the script instance is being loaded.
        /// Set the initial textList[indexPrenomAtStart] by concatenating the value
        /// of "prenom" from the PlayerPrefs with the existing value at the
        /// textList[indexPrenomAtStart] position.
        /// </summary>
        private void Awake()
        {
            prenom = PlayerPrefs.GetString("Prenom");
            textList[indexPrenomAtStart] = prenom + " " + textList[indexPrenomAtStart];
        }
        
        /// <summary>
        /// A coroutine that changes the text displayed on the scrollText.
        /// </summary>
        /// <returns></returns>
        public IEnumerator ChangeText()
        {
            yield return new WaitForSeconds(timeBetweenEachTextChange);
            if (_indexText < textList.Count)
            {
                if (_indexAnswers < indexsAnswerToShow.Count && indexsAnswerToShow[_indexAnswers] < UserScreen.Instance.Answers.Count && indexsTextToReplace.Contains(_indexText))
                {
                    scrollText.text = prefacesTextToReplace[_indexAnswers] + prenom + " : " + UserScreen.Instance.Answers[indexsAnswerToShow[_indexAnswers]];
                    scrollText.text = scrollText.text.Replace("\n", ", ");
                    _indexAnswers++;
                }
                else
                {
                    scrollText.text = textList[_indexText];
                }
                _indexText++;
            }
            else
            {
                _indexText = 0;
            }

            StartCoroutine(ChangeText());
        }
    }
}
