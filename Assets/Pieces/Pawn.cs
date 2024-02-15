using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public GameObject game;

    List<Move> Moves = new List<Move>();

    //Generates move for a board of GameObjects
    public void GenMovesGO(int X, int Y, int Direction, bool Moved)
    {
        List<Move> Moves = game.GetComponent<Game>().Moves;
        Game board = game.GetComponent<Game>();
        Chesspiece CPS = board.GetPosition(X,Y).GetComponent<Chesspiece>();

        int SingleMove = Y + Direction;
        int DoubleMove = SingleMove + Direction;

        if (board.IsOnBoard(X, SingleMove))
        {
            //check one place ahead
            if (board.GetPosition(X, SingleMove) == null)
            {
                Moves.Add(new Move(X, Y, X, SingleMove, 0, 0,0));

                //check 2 places ahead
                //can only move 2spaces if the first isn't blocked
                if (!(Moved) && board.GetPosition(X, DoubleMove) == null)
                {
                    Moves.Add(new Move(X, Y, X, DoubleMove, 0, 0,0));
                }
            }

            //check its sides so one to the right "x + 1"

            if (board.IsOnBoard(X + 1, SingleMove) && board.GetPosition(X + 1, SingleMove) != null && board.GetPosition(X + 1, SingleMove).GetComponent<Chesspiece>().player != CPS.player)
            {
                Moves.Add(new Move(X, Y, X + 1, SingleMove, 1, 0,0));
            }

            //check left side so one to the left "x - 1"

            if (board.IsOnBoard(X - 1, SingleMove) && board.GetPosition(X - 1, SingleMove) != null && board.GetPosition(X - 1, SingleMove).GetComponent<Chesspiece>().player != CPS.player)
            {
                Moves.Add(new Move(X, Y, X - 1, SingleMove, 1, 0,0));
            }
        }
    }
}
