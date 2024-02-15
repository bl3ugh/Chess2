using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : MonoBehaviour
{
    // all directions that the bishop can move in 
    public int[,] moves = { { 1, 1 }, { -1, -1 }, { -1, 1 }, { 1, -1 } };

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

            //checking to see if the coord is on the board
            //and if it is taking a piece since if it is
            //then we will have to change the move plate
            while (board.IsOnBoard(FX, FY) && board.GetPosition(FX, FY) == null)
            {
                Moves.Add(new Move(X, Y, FX, FY, 0, 0,0));

                //increment to the next square to form a line 
                FX += x_offset;
                FY += y_offset;

            }


            //colour of the piece at the square we are checking 
            if (board.IsOnBoard(FX, FY) && player != board.GetPosition(FX, FY).GetComponent<Chesspiece>().player)
            {
                Moves.Add(new Move(X, Y, FX, FY, 1, 0,0));
            }
        }
    }
}
