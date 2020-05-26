using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator myAnimator;


    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
}
