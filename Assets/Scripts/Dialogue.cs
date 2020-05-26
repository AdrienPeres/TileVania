using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue
{
    public bool happenedOnce = false;

    public string name;

    public Sentence[] sentences;
}
