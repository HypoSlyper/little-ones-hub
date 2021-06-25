using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public Sprite defaultSprite;

    public void SetAnim(bool open)
    {
        animator.SetBool("Open", open);
    }
}
