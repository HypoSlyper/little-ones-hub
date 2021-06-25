using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class BalloonManager : MonoBehaviour
{
    #region Public Variables

    [Header("Gift")]
    public GameObject giftBox;
    public GameObject giftPanel;

    [Header("Letter")]
    public TMP_Text scoreText;
    public TMP_Text letterText;
    public float goal = 10;

    [Header("Balloon")]
    public GameObject balloonPrefab;
    public Transform balloonParent;
    public float spawnDelay = 1;
    public float size = 1;
    public float targetY = 10;
    public float animationDuration = 1;

    [Header("Audio")]
    public AudioClip startClip;
    public AudioSource currentLetterSource;
    public GameObject letterHolder;
    public AudioClip tryAgain;

    [Header("Level Win")]
    public GameObject winPanel;

    [Space]
    public Sprite[] sprites;
    public RuntimeAnimatorController[] animators;
    public Vector2[] positions;

    [HideInInspector] public AudioClip[] sounds;
    [HideInInspector] public int score = 0;
    [HideInInspector] public int levelNum = 0;
    [HideInInspector] public int correctLetterNum = 0;
    [HideInInspector] public int prevCorrectLetterNum = 0;
    [HideInInspector] public string correctLetter;
    [HideInInspector] public bool spawnGift = false;
    [HideInInspector] public int roundNum = 0;
    [HideInInspector] public int roundNum4 = 0;
    [HideInInspector] public int numsNum = 0;

    #endregion

    #region Private Variables

    string[] letters;
    int balloonNum = 0;
    int positionNum = 0;
    int letterNum = 0;
    int prevPosNum = 0;
    int prevLetterNum = 0;
    bool addCollider = true;
    bool oneLetter = false;
    List<int> beforeShuffleNums = new List<int>();
    List<int> nums = new List<int>();
    Level level;

    #endregion

    #region Singleton

    [HideInInspector] public static BalloonManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    void Start()
    {
        level = LevelSelection.instance.selectedLevel;
        levelNum = 1;

        beforeShuffleNums.Add(0);
        beforeShuffleNums.Add(1);
        beforeShuffleNums.Add(2);
        beforeShuffleNums.Add(3);
        if (level.letters.Length > 4)
        {
            beforeShuffleNums.Add(4);
        }

        nums = beforeShuffleNums.OrderBy(x => Random.value).ToList();

        if (LevelSelection.instance.selectedLetter == "0")
        {
            oneLetter = false;
        }
        else
        {
            oneLetter = true;
        }

        RestartLevel();
    }

    public void SpawnBalloon()
    {
        balloonNum = Random.Range(0, sprites.Length);
        positionNum = Random.Range(0, positions.Length);
        letterNum = Random.Range(0, letters.Length);

        do
        {
            positionNum = Random.Range(0, positions.Length);
        }
        while (positionNum == prevPosNum);

        do
        {
            letterNum = Random.Range(0, letters.Length);
        }
        while (letterNum == correctLetterNum);

        if (prevLetterNum >= 4 && letterNum != correctLetterNum)
        {
            letterNum = correctLetterNum;
            prevLetterNum = 0;
        }

        prevPosNum = positionNum;
        GameObject go = Instantiate(balloonPrefab, positions[positionNum], Quaternion.identity, balloonParent);

        SetBalloon(go);
        prevLetterNum++;
    }

    void SetBalloon(GameObject balloonGO)
    {
        //balloonGO.GetComponent<Balloon>().speed = moveSpeed;
        balloonGO.transform.localScale = new Vector3(size, size, size);
        balloonGO.GetComponent<Balloon>().target = new Vector3(balloonGO.transform.localPosition.x, targetY);
        balloonGO.GetComponent<Balloon>().animationDuration = animationDuration;
        balloonGO.GetComponent<SpriteRenderer>().sprite = sprites[balloonNum];
        balloonGO.GetComponent<Animator>().runtimeAnimatorController = animators[balloonNum];
        balloonGO.GetComponentInChildren<TMP_Text>().text = letters[letterNum];
        if (addCollider)
        {
            balloonGO.AddComponent<PolygonCollider2D>();
        }
    }

    public bool CheckBalloon(Balloon balloon)
    {
        if (balloon.GetComponentInChildren<TMP_Text>().text == correctLetter)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator PlayAudio(AudioSource source, AudioClip preClip, AudioClip sound, float delay, bool invokeSpawn, bool letterHold)
    {
        if (!source.isPlaying || preClip == startClip)
        {
            yield return new WaitForSeconds(delay);

            source.clip = preClip;
            source.Play();

            yield return new WaitForSeconds(source.clip.length);

            if (letterHold)
            {
                letterHolder.SetActive(true);
            }
            source.clip = sound;
            source.Play();

            yield return new WaitForSeconds(source.clip.length);

            if (letterHold)
            {
                letterHolder.SetActive(false);
            }

            if (invokeSpawn)
            {
                InvokeRepeating("SpawnBalloon", 0, BalloonManager.instance.spawnDelay);
            }
        }
        else
        {
            yield return null;
        }
    }

    public IEnumerator PlaySoundIE(AudioSource source)
    {
        letterHolder.SetActive(true);
        currentLetterSource.Play();

        yield return new WaitForSeconds(currentLetterSource.clip.length);

        letterHolder.SetActive(false);
    }

    public void PlaySound(AudioSource source)
    {
        if (!GetComponent<AudioSource>().isPlaying && !source.isPlaying)
        {
            StartCoroutine(PlaySoundIE(source));
        }
    }

    public void UpdateScore()
    {
        if (score >= goal)
        {
            scoreText.text = goal.ToString();
            GameWon();
        }

        scoreText.text = score.ToString();
    }

    public void RestartLevel()
    {
        roundNum4 = nums[numsNum];

        if (roundNum4 == 0)
        {
            letters = level.order01;
        }
        else if (roundNum4 == 1)
        {
            letters = level.order02;
        }
        else if (roundNum4 == 2)
        {
            letters = level.order03;
        }
        else if (roundNum4 == 3)
        {
            letters = level.order04;
        }
        else if (roundNum4 == 4)
        {
            letters = level.order05;
        }

        sounds = new AudioClip[0];
        sounds = new AudioClip[letters.Length];

        int x = 0;
        for (int j = 0; j < level.sounds.Length; j++)
        {
            for (int i = 0; i < level.sounds.Length; i++)
            {
                if (letters[x] == level.sounds[i].name.ToLower())
                {
                    sounds[x] = level.sounds[i];
                }
            }
            x++;
        }

        if (levelNum > level.letters.Length - 1)
        {
            spawnGift = true;
            levelNum = 0;
        }
        else
        {
            spawnGift = false;
        }

        if (roundNum4 > level.letters.Length - 1)
        {
            roundNum4 = nums[0];
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Gift"))
        {
            Destroy(go);
        }

        giftPanel.SetActive(false);

        currentLetterSource.gameObject.SetActive(true);

        addCollider = true;

        if (GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Stop();
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Balloon"))
        {
            Destroy(go.gameObject);
        }

        StopAllCoroutines();
        CancelInvoke("SpawnBalloon");

        score = 0;
        UpdateScore();

        if (oneLetter)
        {
            for (int i = 0; i < letters.Length; i++)
            {
                if (letters[i] == LevelSelection.instance.selectedLetter)
                {
                    correctLetterNum = i;
                }
            }
        }
        else
        {
            correctLetterNum = nums[roundNum];
        }

        prevCorrectLetterNum = correctLetterNum;

        correctLetter = letters[correctLetterNum];
        letterText.text = correctLetter;

        currentLetterSource.clip = sounds[correctLetterNum];

        if (spawnGift)
        {
            GiftManager.instance.SetGiftNum();
        }

        StartCoroutine(PlayAudio(GetComponent<AudioSource>(), startClip, sounds[correctLetterNum], 0.5f, true, true));
        levelNum++;
        roundNum++;
    }

    public void GameWon()
    {
        if (spawnGift)
        {
            currentLetterSource.gameObject.SetActive(false);
        }

        foreach (var bln in GameObject.FindGameObjectsWithTag("Balloon"))
        {
            bln.GetComponent<Collider2D>().enabled = false;
        }

        addCollider = false;

        CancelInvoke("SpawnBalloon");

        if (spawnGift)
        {
            giftPanel.SetActive(true);
            giftPanel.GetComponent<Animator>().SetBool("FadeIn", true);
            StartCoroutine(SpawnGift(0.35f));
        }
        else
        {
            StartCoroutine(SetRestart());
        }
    }

    //public IEnumerator LevelOver()
    //{
    //    foreach (GameObject go in GameObject.FindGameObjectsWithTag("Gift"))
    //    {
    //        Destroy(go);
    //    }
    //    giftPanel.SetActive(false);

    //    winPanel.SetActive(true);
    //    winPanel.GetComponent<Animator>().SetBool("Zoom", true);
    //    winPanel.GetComponent<AudioSource>().Play();

    //    yield return new WaitForSeconds(7.5f);

    //    GetComponent<SceneSwitcher>().SwitchScene("MainMenu");
    //}

    public IEnumerator SetRestart()
    {
        yield return new WaitForSeconds(2);

        RestartLevel();
    }

    public IEnumerator SpawnGift(float delay)
    {
        yield return new WaitForSeconds(delay);

        Instantiate(giftBox);
    }
}
