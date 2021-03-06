﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetAnimator(RuntimeAnimatorController animation)
    {

        animator.runtimeAnimatorController = animation;
    }

    public void Clear()
    {
        animator.runtimeAnimatorController = null;
        spriteRenderer.sprite = null;

    }

}
