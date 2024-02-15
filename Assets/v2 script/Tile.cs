using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color BaseColour, OffsetColour;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private GameObject Highlight;



    public void Init(bool IsOffset)
    {
        render.color = IsOffset ? OffsetColour : BaseColour;
    }

    void OnMouseEnter()
    {
        Highlight.SetActive(true);
    }
    void OnMouseExit()
    {
        Highlight.SetActive(false);
    }
}
