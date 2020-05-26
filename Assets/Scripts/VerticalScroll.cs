using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    [Tooltip ("Game units per seconds")]
    [SerializeField] float scrollRate = 1f;
    

    // Update is called once per frame
    void Update()
    {
        float yMove = scrollRate * Time.deltaTime;
        transform.Translate(new Vector2(0, yMove));
    }
}
