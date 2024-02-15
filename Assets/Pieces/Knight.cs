using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public int[,] moves = { { 1, 2 }, { 2, 1 }, { -1, 2 }, { -2, 1 }, { -1, -2 }, { -2, -1 }, { 1, -2 }, { 2, -1 } };

    public GameObject game;

    List<Move> Moves = new List<Move>();

    public void GenMovesGO(int X, int Y, string player)
    {
        List<Move> Moves = game.GetComponent<Game>().Moves;
        Game board = game.GetComponent<Game>();

        for (int y = 0; y < moves.GetLength(0); y++)
        {
            int x_offset = moves[y, 0];
            int y_offset = moves[y, 1];

            //Final x and y position
            int FX = X + x_offset;
            int FY = Y + y_offset;

            if (board.IsOnBoard(FX, FY))
            {
                // Get what is at that square
                GameObject CP = board.GetPosition(FX, FY);

                //if there is no piece on that square
                if (CP == null)
                {
                    Moves.Add(new Move(X, Y, FX, FY, 0, 0,0));
                }//check again if the piece is an enemy
                else if (CP.GetComponent<Chesspiece>().player != player)
                {
                    Moves.Add(new Move(X, Y, FX, FY, 1, 0,0));
                }
            }
        }
    }
}
