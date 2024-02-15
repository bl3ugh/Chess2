using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chesspiece : MonoBehaviour
{
    public Pawn pawn;
    public Queen queen;
    public Bishop bishop;
    public Rook rook;
    public King king;
    public Knight knight;

    public GameObject game;//referes back to our main camera
    public GameObject move_plate;//to show valid moves

    //colour of the piece
    public string colour;
    public string piece;


    //Every different piece
    public Sprite black_pawn, black_queen, black_knight, black_king, black_rook, black_bishop;
    public Sprite white_pawn, white_queen, white_knight, white_king, white_rook, white_bishop;

    //variable to keep track of which the colour the piece belongs to 
    public string player;

    //To store possible moves for pieces for placement of Moveplates


    //initial positions for placement so that it isn't on the screen
    public int XBoard = -1;
    public int YBoard = -1;


    //keeps track to see if the piece has moved
    public bool Moved = false;

    public void Activate()//when chesspiece is made this is called
    {
        //Finds GameController from unity
        game = GameObject.FindGameObjectWithTag("GameController");

        pawn = game.GetComponent<Pawn>();
        queen = game.GetComponent<Queen>();
        bishop = game.GetComponent<Bishop>();
        rook = game.GetComponent<Rook>();
        king = game.GetComponent<King>();
        knight = game.GetComponent<Knight>();
        


        //used to change the position of the object when created 
        SetCoords();

        //gets the piece from a string
        switch (this.name)
        {
            //black pieces
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;

            //white pieces
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
        }
    }
    public void SetCoords()
    {
        //'this' referes to the thing that this code is attached to 
        this.transform.position = new Vector3(XBoard, YBoard, -4);
    }

    public int GetXBoard()
    {
        return XBoard;
    }

    public int GetYBoard()
    {
        return YBoard;
    }

    public void SetXBoard(int x)
    {
        XBoard = x;
    }

    public void SetYBoard(int y)
    {
        YBoard = y;
    }

    private void OnMouseUp()
    {

        if (game.GetComponent<Game>().CurrentPlayer == colour)
        {
            DestroyMovePlates();

            //if you select another piece without moving you still want move plates for that piece

            InitiateMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        foreach (GameObject mp in movePlates)
        {
            Destroy(mp);
        }
    }



    public void InitiateMovePlates()
    {

        //depending on the name the moves that it can make are different
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                queen.GenMovesGO(XBoard, YBoard, player);
                break;

            case "black_knight":
            case "white_knight":
                knight.GenMovesGO(XBoard,YBoard, player);
                break;

            case "black_rook":
            case "white_rook":
                rook.GenMovesGO(XBoard, YBoard, player);
                break;

            case "black_king":
            case "white_king":
                king.GenMovesGO(XBoard, YBoard, player, Moved);
                break;

            case "black_bishop":
            case "white_bishop":
                bishop.GenMovesGO(XBoard, YBoard, player);
                break;

            case "black_pawn":
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
        List<Move> Moves = game.GetComponent<Game>().Moves;
        for (int i = 0; i < Moves.Count; i++)
        {
            bool Take = (Moves[i].Take == 1);
            bool Castle = (Moves[i].Castle == 1);
            SpawnMovePlate(Moves[i].FinalX, Moves[i].FinalY, Take, Castle);
        }
        Moves.Clear();
    }


    public void SpawnMovePlate(int x, int y, bool attack, bool CastlingPlate)
    {
        if (attack)
        {
            //spawn in the base move plate as MP
            GameObject MP = Instantiate(move_plate, new Vector3(x, y, -6), Quaternion.identity);
            MovePlate MPScript = MP.GetComponent<MovePlate>();

            //Make it the attack Move plate
            MPScript.takes = attack;

            //Set the move plates reference to this piece
            //gameObject referes to the current piece
            MPScript.SetReference(gameObject);

            //save its coords to the move plate
            MPScript.SetCoords(x, y);

        }
        else
        {
            //spawn in the base move plate as MP
            GameObject MP = Instantiate(move_plate, new Vector3(x, y, -6), Quaternion.identity);
            MovePlate MPScript = MP.GetComponent<MovePlate>();

            //Set the move plates reference to this piece
            //gameObject referes to the current piece
            MPScript.SetReference(gameObject);

            //save its coords to the move plate
            MPScript.SetCoords(x, y);

            //save if this move plate is a castle move
            if (CastlingPlate == true)
            {
                MPScript.Castle = true;
            }

        }
    }

}

