using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GiftManager : MonoBehaviour
{
    //public GameObject restartButton;
    public GameObject gift;

    int giftNum;
    int prevGiftNum = 0;

    List<Sprite> currentGiftList = new List<Sprite>();
    Sprite[] currentGifts;
    string[] letters;

    #region Singleton

    [HideInInspector] public static GiftManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public void SetGiftNum()
    {
        foreach (Sprite giftGo in LevelSelection.instance.selectedLevel.images)
        {
            if (giftGo.name.ToLower().StartsWith(BalloonManager.instance.correctLetter))
            {
                currentGiftList.Add(giftGo);
            }
            else if (BalloonManager.instance.correctLetter == "x" && giftGo.name.EndsWith("x"))
            {
                currentGiftList.Add(giftGo);
            }
        }

        currentGifts = new Sprite[0];
        currentGifts = currentGiftList.ToArray();

        giftNum = Random.Range(0, currentGifts.Length);
        prevGiftNum = giftNum;
    }

    public void GiftPlayer()
    {
        GameObject instGift = Instantiate(gift);
        SetGift(instGift);
    }

    void SetGift(GameObject giftGo)
    {
        giftGo.GetComponent<SpriteRenderer>().sprite = currentGifts[giftNum];
        giftGo.GetComponentInChildren<TMP_Text>().text = currentGifts[giftNum].name;
        
        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        //restartButton.SetActive(true);
        BalloonManager.instance.numsNum++;
        BalloonManager.instance.roundNum = 0;

        yield return new WaitForSeconds(5f);

        BalloonManager.instance.RestartLevel();
    }
}
