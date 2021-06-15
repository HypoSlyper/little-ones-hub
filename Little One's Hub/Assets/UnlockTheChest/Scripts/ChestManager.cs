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

    [Header("Score")]
    public int goal = 4;
    public TMP_Text scoreText;

    [Header("UI")]
    public TMP_Text correctWordText;
    public Image[] buttonImages;

    [HideInInspector] public int correctWordNum;
    [HideInInspector] public Sprite correctImage;

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

    void Start()
    {
        level = LevelSelection.instance.selectedLevel;
        SetWords();
        SetImages();
        RestartLevel();
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

        if (correctWordNum >= goal)
        {
            SetWords();
        }

        SetImages();
        correctWordText.text = words[correctWordNum];
        chest.animator.SetBool("Open", false);
        chest.GetComponent<SpriteRenderer>().sprite = chest.defaultSprite;
        chest.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void CheckClick(Image imageS)
    {
        if (imageS.sprite == correctImage)
        {
            StartCoroutine(LevelDone(3));
        }
    }

    IEnumerator LevelDone(float delay)
    {
        chest.PlayAnim();

        yield return new WaitForSeconds(delay);

        RestartLevel();
    }
}
