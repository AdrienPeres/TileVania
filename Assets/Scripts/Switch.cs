using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] Sprite activated;
    [SerializeField] Sprite deactivated;
    SpriteRenderer spriteRenderer;
    bool isActivated = false;
    CapsuleCollider2D myCapsuleCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        isActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        Interaction();
        if(isActivated)
        {
            spriteRenderer.sprite = activated;
            spriteRenderer.color = new Color(0, 1f, .2f, 1);
        }
        else
        {
            spriteRenderer.sprite = deactivated;
            spriteRenderer.color = Color.white;
        }
    }

    private void Interaction()
    {
        if(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            Player player = FindObjectOfType<Player>();
            if(player.Interact())
            {
                Activation();
            }
        }
    }
    public void Activation()
    {
        isActivated = !isActivated;
    }


    public bool IsActivated() { return isActivated; }
}
