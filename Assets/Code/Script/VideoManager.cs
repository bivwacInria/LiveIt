using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Code.Script
{
    public class VideoManager : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private GameObject panel;
        [SerializeField] private Animation animation;
        [SerializeField] private ScrollTextManager scrollTextManager;
        [SerializeField] private List<VideoClip> listVideos;
        private int _indexListVideos = 0;

        private void Start()
        {
            StartCoroutine(ChangeVideoWhenLastVideoEnd());
        }
        
        /// <summary>
        ///  Coroutine to change the video when the last video ends.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChangeVideoWhenLastVideoEnd()
        {
            yield return new WaitForSeconds((float)videoPlayer.clip.length);
            _indexListVideos++;
            if (_indexListVideos < listVideos.Count)
            {
                panel.SetActive(!panel.activeSelf);
                animation.Play();
                StartCoroutine(scrollTextManager.ChangeText());
                videoPlayer.clip = listVideos[_indexListVideos];
                videoPlayer.Play();
                StartCoroutine(ChangeVideoWhenLastVideoEnd());
            }
            else
            {
                _indexListVideos = 0;
                videoPlayer.clip = listVideos[_indexListVideos];
                videoPlayer.Play();
                StartCoroutine(ChangeVideoWhenLastVideoEnd());
            }
        }
    }
}
