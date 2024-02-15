using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    //buttons
    public GameObject PlayButton;

    //start positions of the camera
    public float CamXPos = -10;
    public float CamYPos = 3.5f;

    public List<Move> Moves = new List<Move>();

    public GameObject[,] positions = new GameObject[8, 8];//2d array 8x8 for positions
    public GameObject[,] black_pieces = new GameObject[8,8]; //To store pos of black pieces
    public GameObject[,] white_pieces = new GameObject[8,8]; //To store pos of white pieces

    private int x_ord;//to store x position for positions
    private int y_ord;//to store y position for positions

    private bool place_piece = true;//used to skip rows when placing pieces

    //board orientation, the player will always be at the bottom
    public string player = "black";


    //current player that can move
    //will switch per move 
    //always starts with white
    public string CurrentPlayer = "white";

    private int num_spaces;//used for when there aren't ay pieces being placed

    private string piece_name;

    //int to switch between 1 and 0 
    public int iterator = 0;

    //array to store the 2 players to be switched between using the iterator
    public string[] players = { "white", "black" };

    //the obj
    public GameObject Chesspiece;   

    public string startFEN_black = "rnbqkbnr/ppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";//FEN string for when black starts
    public string startFEN_white = "RNBQKBNR/PPPPPPPP/8/8/8/8/pppppppp/rnbqkbnr";//FEn string for when white starts

    //to check later what colour to assign to each piece
    public string[] BlackPieces = { "black_pawn", "black_queen", "black_knight", "black_king", "black_rook", "black_bishop" } ;

    [SerializeField] private Transform cam_pos;//used to set the camera such that 0,0 is 0,0


    void cam_Transform()
    {
        cam_pos.transform.position = new Vector3(CamXPos, CamYPos, -100);//centres the camera about the board, which starts at 0,0
    }

    public void SwitchPlayer()
    {
        //binary
        iterator = 2147483647 - (2147483646 | iterator);
        CurrentPlayer = players[iterator];
        if(CurrentPlayer != player)
        {
            GameObject.FindGameObjectWithTag("Bot").GetComponent<Bot>().MakeRandomMove();
        }

    }

    public void chr_to_int(char chr)//function to translate a character to its number form since this language sucks
    {
        switch (chr)
        {
            case '1': num_spaces = 1; break;
            case '2': num_spaces = 2; break;
            case '3': num_spaces = 3; break;
            case '4': num_spaces = 4; break;
            case '5': num_spaces = 5; break;
            case '6': num_spaces = 6; break;
            case '7': num_spaces = 7; break;
            case '8': num_spaces = 8; break;
        }
    }

    private void letter_to_piece(char letter)// takes a letter and returns the piece name
    {
        switch (letter)
        {
            case 'p': piece_name = "black_pawn"; place_piece = true; break;
            case 'P': piece_name = "white_pawn"; place_piece = true; break;

            case 'n': piece_name = "black_knight"; place_piece = true; break;
            case 'N': piece_name = "white_knight"; place_piece = true; break;

            case 'r': piece_name = "black_rook"; place_piece = true; break;
            case 'R': piece_name = "white_rook"; place_piece = true; break;

            case 'b': piece_name = "black_bishop"; place_piece = true; break;
            case 'B': piece_name = "white_bishop"; place_piece = true; break;

            case 'q': piece_name = "black_queen"; place_piece = true; break;
            case 'Q': piece_name = "white_queen"; place_piece = true; break;

            case 'k': piece_name = "black_king"; place_piece = true; break;
            case 'K': piece_name = "white_king"; place_piece = true; break;

            case '/':
                y_ord++;
                x_ord = 0;
                place_piece = false;
                break;

            default:
                chr_to_int(letter);
                x_ord += num_spaces;
                place_piece = false;
                break;
        }
    }

    private bool Contains(string[] arr, string name)
    {
        for(int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == name)
            {
                return true;
            }
        }
        return false;
    }

    private void place_pieces()
    {
        GameObject obj;
        //if player == black, fen = black fen, else white
        bool IsBlack = (player == "black");
        string _FEN = IsBlack ? startFEN_black : startFEN_white;
        foreach(char letter in _FEN)
        {
            letter_to_piece(letter);
            IsBlack = Contains(BlackPieces, piece_name);
            string Colour = IsBlack ? "black" : "white";
            if (place_piece)
            {
                obj = Create(piece_name, x_ord, y_ord, Colour);
                positions[y_ord, x_ord] = obj;
                //if the piece is black then add to black
                if (Contains(BlackPieces, piece_name))
                {
                    black_pieces[y_ord,x_ord] = obj;
                }
                //else to white
                else
                {
                    white_pieces[y_ord,x_ord] = obj;
                }
                x_ord++;

            }
        }
        
    }
    public GameObject Create(string name, int x, int y, string colour)
    {
        GameObject obj = Instantiate(Chesspiece,new Vector3(0,0,-1),Quaternion.identity);
        Chesspiece CP = obj.GetComponent<Chesspiece>();
        CP.piece = name.Substring(name.IndexOf('_') + 1, name.Length - (name.IndexOf('_') + 1));
        CP.name = name;
        CP.colour = colour;
        CP.SetXBoard(x); 
        CP.SetYBoard(y);
        CP.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chesspiece CP = obj.GetComponent<Chesspiece>();
        positions[CP.GetYBoard(), CP.GetXBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[y, x] = null;
    }

    public GameObject GetPosition(int x , int y)
    {
        return positions[y, x];
    } 
    public bool IsOnBoard(int x, int y) // checks to see if a position is on the board
    {
        //change back to 0 and 7
        if(x < 0 || y < 0 || x > 7 || y > 7) return false;
        return true;
    }

    public void Menu()
    {
        Instantiate(PlayButton, new Vector3(CamXPos + 3, CamYPos + 1, -3), Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        Menu();
        cam_Transform();
        place_pieces();
        if(CurrentPlayer != player)
        {
            GameObject.FindGameObjectWithTag("Bot").GetComponent<Bot>().MakeRandomMove();
        }
    }

}
