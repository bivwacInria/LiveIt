using System;
using UnityEngine;

namespace Code.Script
{
    public class SpacialAnchorManager : MonoBehaviour
    {
        public static SpacialAnchorManager Instance;
        [SerializeField] private OVRSceneAnchor experienceScreen;
        [SerializeField] private OVRSceneAnchor userScreen;
        
        private Vector3 _experienceScreenPositionInit;
        private Quaternion _experienceScreenRotationInit;
        private Vector3 _experienceScreenScaleInit;
        
        private Vector3 _userScreenPositionInit;
        private Quaternion _userScreenRotationInit;
        private Vector3 _userScreenScaleInit; 

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// Initializes and saves the initial anchor data for the experience screen and user screen.
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

            var experienceScreenTransform = experienceScreen.transform;
            _experienceScreenPositionInit = experienceScreenTransform.position;
            _experienceScreenRotationInit = experienceScreenTransform.rotation;
            _experienceScreenScaleInit = experienceScreenTransform.localScale;
            
            var userScreenTransform = userScreen.transform;
            _userScreenPositionInit = userScreenTransform.position;
            _userScreenRotationInit = userScreenTransform.rotation;
            _userScreenScaleInit = userScreenTransform.localScale;
            
            LoadAnchors();
        }
        
        /// <summary>
        /// Saves the anchor data for the experience screen and user screen in PlayerPrefs.
        /// </summary>
        public void SaveAnchors()
        {
            var experienceScreenTransform = experienceScreen.transform;
            var experienceScreenPosition = experienceScreenTransform.position;
            var experienceScreenRotation = experienceScreenTransform.rotation;
            var experienceScreenScale = experienceScreenTransform.localScale;
            
            var userScreenTransform = userScreen.transform;
            var userScreenPosition = userScreenTransform.position;
            var userScreenRotation = userScreenTransform.rotation;
            var userScreenScale = userScreenTransform.localScale;

            var experienceScreenSaveData = $"{experienceScreen.Uuid}/x:{experienceScreenPosition.x}/y:{experienceScreenPosition.y}/z:{experienceScreenPosition.z}/rx:{experienceScreenRotation.x}/ry:{experienceScreenRotation.y}/rz:{experienceScreenRotation.z}/rw:{experienceScreenRotation.w}/sx:{experienceScreenScale.x}/sy:{experienceScreenScale.y}/sz:{experienceScreenScale.z}";
            PlayerPrefs.SetString("uuidExperienceScreenSave", experienceScreenSaveData);

            var userScreenSaveData = $"{userScreen.Uuid}/x:{userScreenPosition.x}/y:{userScreenPosition.y}/z:{userScreenPosition.z}/rx:{userScreenRotation.x}/ry:{userScreenRotation.y}/rz:{userScreenRotation.z}/rw:{userScreenRotation.w}/sx:{userScreenScale.x}/sy:{userScreenScale.y}/sz:{userScreenScale.z}";
            PlayerPrefs.SetString("uuidUserScreenSave", userScreenSaveData);
        }

        /// <summary>
        /// LoadAnchors loads the anchors for the experience screen and user screen from PlayerPrefs.
        /// </summary>
        private void LoadAnchors()
        {
            if (!LoadAnchor("uuidExperienceScreenSave",out var experienceScreenPosition, out var experienceScreenRotation, out var experienceScreenScale)) return;
            if (!LoadAnchor("uuidUserScreenSave",out var userScreenPosition, out var userScreenRotation, out var userScreenScale)) return;

            var transform1 = experienceScreen.transform;
            transform1.position = experienceScreenPosition;
            transform1.rotation = experienceScreenRotation;
            transform1.localScale = experienceScreenScale;
            
            var transform2 = userScreen.transform;
            transform2.position = userScreenPosition;
            transform2.rotation = userScreenRotation;
            transform2.localScale = userScreenScale;
        }
        
        /// <summary>
        /// Loads an anchor using the saved anchor key and retrieves the position, rotation, and scale.
        /// </summary>
        /// <param name="savedAnchorKey">The key used to save the anchor.</param>
        /// <param name="position">The position of the anchor.</param>
        /// <param name="rotation">The rotation of the anchor.</param>
        /// <param name="scale">The scale of the anchor.</param>
        /// <returns>
        /// True if the anchor was successfully loaded, otherwise false.
        /// </returns>
        private static bool LoadAnchor(string savedAnchorKey,out Vector3 position, out Quaternion rotation, out Vector3 scale)
        {
            var playerPrefdata = PlayerPrefs.GetString(savedAnchorKey).Split('/');//gets string version of the uuid, position and rotation saved all separated 
            var myUuid = playerPrefdata[0]; 

            if (!Guid.TryParse(myUuid, out var parsedGuid))
            {
                position = default;
                rotation = default;
                scale = default;
                return false;
            }

            //Position is stocked in a string splited previously has 3 string playerPrefData[1],[2] and [3] corresponding to x,y,z 
            //PlayerPrefData[1] look's like x:VALUE so we split the string again using : as separator and keeping the second part: [1]
            //Same thing for [2] and [3]
            position = new Vector3(float.Parse(playerPrefdata[1].Split(':')[1]),
                float.Parse(playerPrefdata[2].Split(':')[1]), float.Parse(playerPrefdata[3].Split(':')[1]));
            //Same method than for position but using playerPrefData[4], [5], [6] and [7] corresponding to x,y,z,w
            rotation = new Quaternion(float.Parse(playerPrefdata[4].Split(':')[1]),
                float.Parse(playerPrefdata[5].Split(':')[1]), float.Parse(playerPrefdata[6].Split(':')[1]),
                float.Parse(playerPrefdata[7].Split(':')[1]));
            //Same method than for position but using playerPrefData[8], [9] and [10]
            scale = new Vector3(float.Parse(playerPrefdata[8].Split(':')[1]),
                float.Parse(playerPrefdata[9].Split(':')[1]), float.Parse(playerPrefdata[10].Split(':')[1]));
            return true;
        }
        
        /// <summary>
        /// Resets the anchors of the experience screen and the user screen to their initial positions, rotations, and scales.
        /// </summary>
        public void ResetAnchors()
        {
            var experienceScreenTransform = experienceScreen.transform;
            experienceScreenTransform.position = _experienceScreenPositionInit;
            experienceScreenTransform.rotation = _experienceScreenRotationInit;
            experienceScreenTransform.localScale = _experienceScreenScaleInit;
            
            var userScreenTransform = userScreen.transform;
            userScreenTransform.position = _userScreenPositionInit;
            userScreenTransform.rotation = _userScreenRotationInit;
            userScreenTransform.localScale = _userScreenScaleInit;
        }

        /// <summary>
        /// Removes all saved anchors by deleting all player prefs.
        /// </summary>
        public void RemoveSavedAnchors()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
