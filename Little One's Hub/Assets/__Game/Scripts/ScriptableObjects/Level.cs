using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public int levelNumber;
    public string levelColor;
    public string[] letters;
    public AudioClip[] sounds;
    public Sprite[] images;

    [Header("Orders")]
    public string[] order01;
    public string[] order02;
    public string[] order03;
    public string[] order04;
    public string[] order05;

    [Header("Words")]
    public string[] words;
    public Sprite[] wordImages;
}
