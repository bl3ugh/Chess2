using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class Bot : MonoBehaviour
{
    public Pawn pawn;
    public Queen queen;
    public Bishop bishop;
    public Rook rook;
    public King king;
    public Knight knight;

    //To allow fo GenMoves to only have to do one operation
    private int[] Multiplier = {1, -1};

    private int[,] InternalBoard = new int[8, 8];

    private GameObject[,] GameBoard;
    private GameObject[,] BotBoard;

    void Start()
    {
        Game game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();

        pawn = game.GetComponent<Pawn>();
        queen = game.GetComponent<Queen>();
        bishop = game.GetComponent<Bishop>();
        rook = game.GetComponent<Rook>();
        king = game.GetComponent<King>();
        knight = game.GetComponent<Knight>();
    }


    private int PieceToInt(GameObject piece)
    {
        //returns a value for a piece
        Chesspiece Piece = piece.GetComponent<Chesspiece>();
        int output = 0;
        switch (Piece.piece)
        {
            case null: output = 0; break;
            case "pawn":output = 1; break;
            case "knight":output = 3; break;
            case "bishop": output = 3; break;
            case "rook": output = 5; break;
            case "queen": output = 9; break;
            case "king": output = 10000; break;
        }
        bool IsBlack = (Piece.colour == "black");
        //black pieces are stored as negative numbers
        output *= IsBlack ? -1 : 1;
        return output;
    }


    private int[,] BoardToInt(GameObject[,] board)
    {
        for (int y = 0; y < 8; y++)
        {
            for(int x = 0; x < 8; x ++)
            {
                InternalBoard[y,x] = PieceToInt(board[y,x]);
            }
        }
        return InternalBoard;
    }

    private void FindAllMoves(GameObject[,] Board,string BotColour)
    {
        GameObject game = GameObject.FindGameObjectWithTag("GameController");
        //loop through each piece and get possible moves
        foreach (GameObject piece in Board)
        {
            if (piece != null)
            {
                if (piece.GetComponent<Chesspiece>().player == BotColour)
                {
                    Chesspiece CP = piece.GetComponent<Chesspiece>();
                    int XBoard = CP.XBoard;
                    int YBoard = CP.YBoard;
                    string player = CP.player;
                    bool Moved = false;
                    switch (piece.name)
                    {
                        case "black_queen":
                        case "white_queen":
                            queen.GenMovesGO(XBoard, YBoard, player);
                            break;

                        case "black_knight":
                        case "white_knight":
                            knight.GenMovesGO(XBoard, YBoard, player);
                            break;

                        case "black_rook":
                        case "white_rook":
                            rook.GenMovesGO(XBoard, YBoard, player);
                            break;

                        case "black_king":
                        case "white_king":
                            Moved = CP.Moved;
                            king.GenMovesGO(XBoard, YBoard, player, Moved);
                            break;

                        case "black_bishop":
                        case "white_bishop":
                            bishop.GenMovesGO(XBoard, YBoard, player);
                            break;

                        case "black_pawn":
                            Moved = CP.Moved;
                            if (game.GetComponent<Game>().player == "white")
                            {
                                pawn.GenMovesGO(XBoard, YBoard, -1, Moved);
                            }
                            else
                            {
                                pawn.GenMovesGO(XBoard, YBoard, +1, Moved);
                            }
                            break;


                        case "white_pawn":
                            Moved = CP.Moved;
                            if (game.GetComponent<Game>().player == "white")
                            {
                                pawn.GenMovesGO(XBoard, YBoard, +1, Moved);
                            }
                            else
                            {
                                pawn.GenMovesGO(XBoard, YBoard, -1, Moved);
                            }
                            break;
                    }
                }
            }
        }
    }

    //Moves a piece and updates its stored location
    private void MovePiece(int StartX, int StartY,int FinalX, int FinalY, int direction, GameObject reference, int Take)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");

        if (Take == 1)
        {
            if (reference.GetComponent<Chesspiece>().colour == "white")
            {
                //as taking we remove the piece being taken, which is on the opposite team
                controller.GetComponent<Game>().black_pieces[FinalY, FinalX] = null;
            }
            else
            {
                controller.GetComponent<Game>().white_pieces[FinalY, FinalX] = null;
            }

            GameObject CP = controller.GetComponent<Game>().GetPosition(FinalX, FinalY);//gets the chesspiece that is being taken 
            Destroy(CP);//and destroys it
        }
        
        //set the positons of where our piece was as empty
        if (direction != 0)
        {
            GameObject rook = controller.GetComponent<Game>().GetPosition(StartX, StartY);
            controller.GetComponent<Game>().SetPositionEmpty(StartX, StartY);
            rook.GetComponent<Chesspiece>().SetXBoard(FinalX + direction);
            rook.GetComponent<Chesspiece>().SetYBoard(FinalY);

            if (reference.GetComponent<Chesspiece>().colour == "white")
            {
                //as taking we remove the piece being taken, which is on the opposite team
                controller.GetComponent<Game>().white_pieces[FinalY, FinalX + direction] = reference;
                controller.GetComponent<Game>().white_pieces[StartY, StartX] = null;
            }
            else
            {
                controller.GetComponent<Game>().black_pieces[FinalY, FinalX + direction] = reference;
                controller.GetComponent<Game>().black_pieces[StartY, StartX] = null;
            }

            //Actually move the piece using the intended positions
            rook.GetComponent<Chesspiece>().SetCoords();

            //update the array 
            controller.GetComponent<Game>().SetPosition(rook);

            //remove the moveplates
            rook.GetComponent<Chesspiece>().DestroyMovePlates();

            //change its moved value to true
            rook.GetComponent<Chesspiece>().Moved = true;

        }
        else
        {
            controller.GetComponent<Game>().SetPositionEmpty(StartX, StartY);
            reference.GetComponent<Chesspiece>().SetXBoard(FinalX);
            reference.GetComponent<Chesspiece>().SetYBoard(FinalY);

            if (reference.GetComponent<Chesspiece>().colour == "white")
            {
                //as taking we remove the piece being taken, which is on the opposite team
                controller.GetComponent<Game>().white_pieces[FinalY, FinalX] = reference;
                controller.GetComponent<Game>().white_pieces[StartY, StartX] = null;
            }
            else
            {
                controller.GetComponent<Game>().black_pieces[FinalY, FinalX] = reference;
                controller.GetComponent<Game>().black_pieces[StartY, StartX] = null;
            }

            //Actually move the piece using the intended positions
            reference.GetComponent<Chesspiece>().SetCoords();

            //update the array 
            controller.GetComponent<Game>().SetPosition(reference);

            //remove the moveplates
            reference.GetComponent<Chesspiece>().DestroyMovePlates();

            //change its moved value to true
            reference.GetComponent<Chesspiece>().Moved = true;
        }
        controller.GetComponent<Game>().SwitchPlayer();
    }


    private int EvalBoard(GameObject[,] Board)
    {
        // returns the value of a board
        int BoardValues = 0;
        for (int y = 0; y < 8; y++) 
        {
            for (int x = 0; x < 8; x++)
            {
                BoardValues += PieceToInt(Board[y,x]);
            }
        }
        return BoardValues;
    }


    private int[] EvalMoves(int NumMoves)
    {
        // returns value of the moves based on the value of the baord after he move has been made
        int[] MoveValues = new int[NumMoves];
        return MoveValues;
    }

    private GameObject FindReference(int X, int Y)
    {
        Game game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        return game.positions[Y, X];
    }


    private GameObject[,] FindBotPieces()
    {
        Game game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        GameObject[,] Board;
        if (game.player == "black")
        {
            Board = game.white_pieces;
        }
        else
        {
            Board = game.black_pieces;
        }
        return Board;
    }

    private void TempMove(int StartX, int StartY, int FinalX, int FinalY, int direction, GameObject[,] Board)
    { 

        //set the positons of where our piece was as empty
        if (direction != 0)
        {
            GameObject rook = Board[StartY, StartX];
            Board[StartY, StartX] = null;
            Board[FinalY, FinalX + direction] = rook;

            //change its moved value to true
            rook.GetComponent<Chesspiece>().Moved = true;

        }
        else
        {
            GameObject reference = Board[StartY, StartX];
            Board[StartY, StartX] = null;
            Board[FinalY, FinalX] = reference;

            //change its moved value to true
            reference.GetComponent<Chesspiece>().Moved = true;
        }
    }

    private void UnmakeMove(int StartX, int StartY, int FinalX, int FinalY, int direction, GameObject[,] Board)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");

        //set the positons of where our piece was as empty
        if (direction != 0)
        {
            GameObject rook = Board[StartY, FinalX];
            Board[FinalY, FinalX] = null;
            Board[StartY, StartX + direction] = rook;

            //change its moved value to true
            rook.GetComponent<Chesspiece>().Moved = true;

        }
        else
        {
            GameObject reference = Board[StartY, FinalX];
            Board[FinalY, FinalX] = null;
            Board[StartY, StartX] = reference;

            //change its moved value to true
            reference.GetComponent<Chesspiece>().Moved = true;
        }
    }
    private int EvaluateMove(Move move, GameObject[,] Board)
    {
        int Value = 0;
        foreach(GameObject piece in Board)
        {
            if (piece != null) 
            {
                Value += PieceToInt(piece);
            }
        }
        return Value;
    }

    private Move FindBestMove(List<Move> Moves)
    {
        int TopMove = 0;
        Move BestMove = Moves[0];
        int CurrentMove = 0;
        for(int i = 0; i < Moves.Count; i++)
        {
            Move _move = Moves[i];

            //moves the piece on the botboard
            TempMove(_move.InitialX, _move.InitialY, _move.FinalX, _move.FinalY, _move.Direction, BotBoard);

            //find value of current move
            CurrentMove = EvaluateMove(_move, BotBoard);    

            //updates best move
            bool change = (CurrentMove > TopMove) ? true : false;
            BestMove = (change) ? _move : BestMove;

            UnmakeMove(_move.InitialX, _move.InitialY, _move.FinalX, _move.FinalY, _move.Direction, BotBoard);
        }
        return BestMove;
    }

    public void MakeRandomMove()
    {
        Game game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        GameObject[,] Board  = FindBotPieces();
        BotBoard = (GameObject[,])game.positions.Clone();

        string BotPlayer = game.players[game.player == game.players[0] ? 1 : 0];

        //have all moves saved to the Moves in Game
        FindAllMoves(Board,BotPlayer);

        //save the reference to Bot so less accesses 
        //Looks nicer
        List<Move> Moves = game.Moves;

        Move move_ = FindBestMove(Moves);

        MovePiece(move_.InitialX, move_.InitialY, move_.FinalX, move_.FinalY, move_.Direction,FindReference(move_.InitialX, move_.InitialY),move_.Take);

        //this clears the main Moves lsit as well as they are linked by reference
        Moves.Clear();
    }


}
