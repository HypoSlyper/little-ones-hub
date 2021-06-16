﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    public bool move = false;
    public Vector3 defaultPosition;

    Transform target;

    ChestManager chestManager;

    void OnEnable()
    {
        chestManager = ChestManager.instance;
        target = chestManager.scoreText.transform;
    }

    void Update()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, (chestManager.coinSpeed * 10) * Time.deltaTime);
        }

        if (transform.localPosition == target.position)
        {
            move = false;
            chestManager.score4To1++;
            chestManager.chest.animator.SetBool("Open", false);
            chestManager.UpdateScore();
        }
    }
}
