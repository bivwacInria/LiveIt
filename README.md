# Live-It
Unity version 2022.3.20

Using Meta SDK for XR version 62.0.0

Made to be built on MetaQuest 3
# Scenes
Main scene is the initialisation stage of the experiment where the participant would type their names and select their preferred pronouns.

Scene1 is the first scene of the experiment where the experimenter can place and save the position of the TVs, so they can fit the room where the experiment takes place.

Once the setup is done, the experimenter can go back to the Main scene where the experiment proper can start.
# Control
The participant needs to use a Bluetooth keyboard linked to the headset to type, the right text box is preselected. The code automatically transforms QWERTY keyboard commands into AZERTY ones if you need to deactivate this feature it's in the InputManager script.

The experimenter uses the controller and hands to interact with the TVs for the setup:
- A: Lock/unlock the interactability of the TVs.
- B: Save the TVs positions.
- X: Reset the TVs positions.
- Y: Destroy the saved positions.
- Start: Go back to main screen.

To move, rotate or rescale the TVs the experimenter need to use their hands. One hand to just move and rotate and two hands to also rescale the TVs.
# Change language
For the Main scene all the texts can be founds in the Hierarchy, under FlatUnityCanvas/UnityCanvas/Content.

For Scene1, static text are either part of the background image that you can change to fit a more familiar environnement for the participant or in additional text has a Unity Gameobject. All of that is in ExperienceUserScreen/Canvas/Background. The dynamic texts are in the script UserScreen which is attached to ExperienceUserScreen.
