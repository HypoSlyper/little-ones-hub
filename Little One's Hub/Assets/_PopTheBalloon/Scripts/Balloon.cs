using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [HideInInspector] public float speed = 1;
    [HideInInspector] public float animationDuration;
    [HideInInspector] public Vector3 target;

    bool move;
    Animator animator;
    BalloonManager balloonManager;

    private void Start()
    {
        move = true;
        animator = GetComponent<Animator>();
        balloonManager = BalloonManager.instance;
    }

    void Update()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        if (transform.position == target)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (balloonManager.CheckBalloon(this))
        {
            StartCoroutine(Pop());
        }
        else
        {
            WrongBalloon();
        }
    }

    IEnumerator Pop()
    {
        balloonManager.score++;
        balloonManager.UpdateScore();
        Destroy(GetComponent<Collider2D>());
        move = false;
        animator.SetBool("Pop", true);
        GetComponent<AudioSource>().Play();
        Destroy(GetComponentInChildren<TMP_Text>());

        yield return new WaitForSeconds(animationDuration);

        Destroy(this.gameObject);
    }
    
    void WrongBalloon()
    {
        StartCoroutine(balloonManager.PlayAudio(balloonManager.GetComponent<AudioSource>(), balloonManager.tryAgain, balloonManager.sounds[balloonManager.correctLetterNum], 0, false, false));
    }
}