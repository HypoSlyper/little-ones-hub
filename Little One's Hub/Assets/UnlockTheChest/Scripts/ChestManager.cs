using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestManager : MonoBehaviour
{
    #region Public Variables

    [Header("Chest")]
    public Chest chest;
    public float coinSpeed = 10;

    [Header("Score")]
    public int goal = 4;
    public TMP_Text scoreText;

    [Header("UI")]
    public TMP_Text correctWordText;
    public Image[] buttonImages;

    [HideInInspector] public int correctWordNum;
    [HideInInspector] public Sprite correctImage;
    [HideInInspector] public int score4To1 = 0;
    [HideInInspector] public int score = 0;

    #endregion

    #region Private Variables

    List<int> shuffledNums = new List<int>();
    List<int> beforeShuffleNums = new List<int>();

    List<Sprite> shuffledImages = new List<Sprite>();
    List<Sprite> beforeShuffleImages = new List<Sprite>();

    List<string> words = new List<string>();
    List<Sprite> images = new List<Sprite>();

    Level level;

    #endregion

    #region Singleton

    [HideInInspector] public static ChestManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    void Start()
    {
        level = LevelSelection.instance.selectedLevel;
        SetWords();
        SetImages();
        RestartLevel();
    }

    public void UpdateScore()
    {
        if (score4To1 >= 4)
        {
            score++;
            score4To1 = 0;
        }

        scoreText.text = score.ToString();
    }

    void SetWords()
    {
        beforeShuffleNums = new List<int>(0);
        shuffledNums = new List<int>(0);
        words = new List<string>(0);

        correctWordNum = 0;

        for (int i = 0; i < level.words.Length; i++)
        {
            beforeShuffleNums.Add(i);
        }

        shuffledNums = beforeShuffleNums.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < 4; i++)
        {
            words.Add(level.words[shuffledNums[i]]);
        }
    }

    void SetImages()
    {
        images = new List<Sprite>(0);

        beforeShuffleImages = new List<Sprite>(level.wordImages);
        shuffledImages = beforeShuffleImages.OrderBy(x => Random.value).ToList();

        int y = Random.Range(0, 3);

        for (int i = 0; i < 3; i++)
        {
            images.Add(shuffledImages[i]);  
        }

        foreach (Sprite image in level.wordImages)
        {
            if (image.name.ToLower() == words[correctWordNum].ToLower())
            {
                correctImage = image;
            }
        }

        if (!images.Contains(correctImage))
        {
            images[y] = correctImage;
        }

        SetButtonSprite();
    }

    void SetButtonSprite()
    {
        for (int i = 0; i < buttonImages.Length; i++)
        {
            buttonImages[i].sprite = images[i];
        }
    }

    public void RestartLevel()
    {
        correctWordNum++;

        foreach (Image img in buttonImages)
        {
            img.GetComponentInParent<Button>().interactable = true;
        }

        if (correctWordNum >= goal)
        {
            SetWords();
        }

        SetImages();
        correctWordText.text = words[correctWordNum];
    }

    public void CheckClick(Image imageS)
    {
        if (imageS.sprite == correctImage)
        {
            StartCoroutine(LevelDone(3));

            foreach (Image img in buttonImages)
            {
                img.GetComponentInParent<Button>().interactable = false;
            }
        }
    }

    IEnumerator LevelDone(float delay)
    {
        chest.PlayAnim();

        yield return new WaitForSeconds(delay);

        RestartLevel();
    }
}
