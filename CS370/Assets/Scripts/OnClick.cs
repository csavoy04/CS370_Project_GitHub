using UnityEngine;
using System.Collections.Generic;

/*  Script made by: Coleman
        Script that:
            - Allows the player to click a symbol input
                to enter the code for the symbol puzzle
            - Note that this script is only for changing
                button image not to check if puzzle is complete
    Date: 11/16/2025
    Made in: C# VsCode
*/

public class OnClick : MonoBehaviour
{
    [Header("Variables")]
    public SpriteRenderer spriteRenderer;
    public List<Sprite> images;
    public int i = 0;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = images[0];
    }

    void ChangeSprite()
    {
        if (i < 2) 
        {
            i++;
        }
        else
        {
            i = 0;
        }
        spriteRenderer.sprite = images[i];
    }

    void OnMouseDown()
    {
        Debug.Log("clicked");
        ChangeSprite();
    }
}
