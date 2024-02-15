using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMove : MonoBehaviour
{
    public int InitialX { get; set; }
    public int InitialY { get; set; }
    public int FinalX { get; set; }
    public int FinalY { get; set; }
    public int Take { get; set; }
    public int Castle { get; set; }

    public SetMove(int initialX, int initialY, int finalX, int finalY, int take, int castle)
    {
        InitialX = initialX;
        InitialY = initialY;
        FinalX = finalX;
        FinalY = finalY;
        Take = take;
        Castle = castle;
    }
}
