using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SetButtonLetter : MonoBehaviour
{
    void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponentInChildren<TMP_Text>().text = LevelSelection.instance.selectedLevel.letters[i];
        }
    }
}
