using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyForNextLevel : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public BoxCollider2D boxCollider;
    public void Update()
    {
        if (Vector2.Distance(PlayerController.instance.Player.position, transform.position) < 3)
        { 
        SpriteRenderer.enabled = false;
            boxCollider.enabled = false;
            gameObject.transform.position = new Vector2(100, 100);
        }
    }
}
