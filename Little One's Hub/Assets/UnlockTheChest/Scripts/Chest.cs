using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public Sprite defaultSprite;

    public void PlayAnim()
    {
        animator.enabled = true;
        animator.SetBool("Open", true);
    }
}
