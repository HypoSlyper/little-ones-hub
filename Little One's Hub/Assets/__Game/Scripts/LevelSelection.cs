using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    public Level selectedLevel;
    public string selectedLetter = "0";
    public AudioClip selectedSound;

    #region Singleton

    [HideInInspector] public static LevelSelection instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public void SetLevel(Level level)
    {
        selectedLevel = level;
    }

    public void SetLetterAndSound(GameObject button)
    {
        selectedLetter = button.GetComponentInChildren<TMP_Text>().text;

        for (int i = 0; i < selectedLevel.sounds.Length; i++)
        {
            if (selectedLevel.letters[i] == selectedLetter)
            {
                selectedSound = selectedLevel.sounds[i];
                return;
            }
        }
    }
}
