using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipCollider : MonoBehaviour
{

    bool onCollision = false;
    bool exitCollision = false;

    BoxCollider2D boxCollider2D;
    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onCollision = true;
        exitCollision = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onCollision = false;
        exitCollision = true;
    }

    public bool GetOnCollision() { return onCollision;  }

    public bool GetOnExitCollision() { return exitCollision; }

    public BoxCollider2D GetBoxCollider2D() { return boxCollider2D; }
}
