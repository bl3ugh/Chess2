using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;//To handle which piece created the plate

    //to store if the move plate is the king castling
    public bool Castle = false;

    // Board positions for the plate
    int x_ord;
    int y_ord;


    //To check if the piece can take an enemy piece on a location
    public bool takes = false;

    public void Start()
    {
        if (takes)
        {
            //make the circle invisible and make square red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);//make circle invisible
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1f);//to make square red
            transform.GetChild(0).gameObject.transform.position = new Vector3(x_ord, y_ord, -3);
        }
        transform.GetChild(0).gameObject.transform.position = new Vector3(x_ord, y_ord, -3);
    }

   //Moves a piece and updates its stored location
   public void Move(int StartX, int StartY, int direction)
   {
        controller = GameObject.FindGameObjectWithTag("GameController");
        //set the positons of where our piece was as empty
        if (direction != 0)
        {
            GameObject rook = controller.GetComponent<Game>().GetPosition(StartX, StartY);
            controller.GetComponent<Game>().SetPositionEmpty(StartX, StartY);
            rook.GetComponent<Chesspiece>().SetXBoard(x_ord + direction);
            rook.GetComponent<Chesspiece>().SetYBoard(y_ord);

            if (reference.GetComponent<Chesspiece>().colour == "white")
            {
                //as taking we remove the piece being taken, which is on the opposite team
                controller.GetComponent<Game>().white_pieces[y_ord, x_ord + direction] = reference;
                controller.GetComponent<Game>().white_pieces[StartY, StartX] = null;
            }
            else
            {
                controller.GetComponent<Game>().black_pieces[y_ord, x_ord + direction] = reference;
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
            reference.GetComponent<Chesspiece>().SetXBoard(x_ord);
            reference.GetComponent<Chesspiece>().SetYBoard(y_ord);

            if (reference.GetComponent<Chesspiece>().colour == "white")
            {
                //as taking we remove the piece being taken, which is on the opposite team
                controller.GetComponent<Game>().white_pieces[y_ord, x_ord] = reference;
                controller.GetComponent<Game>().white_pieces[StartY, StartX] = null;
            }
            else
            {
                controller.GetComponent<Game>().black_pieces[y_ord, x_ord] = reference;
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
   }


    public void OnMouseUp()//if mouse is clicked on the move plate
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        //chess piece script
        Chesspiece CPS = reference.GetComponent<Chesspiece>();

        if (takes)
        {
            if (reference.GetComponent<Chesspiece>().colour == "white")
            {
                //as taking we remove the piece being taken, which is on the opposite team
                controller.GetComponent<Game>().black_pieces[y_ord, x_ord] = null;
            }
            else
            {
                controller.GetComponent<Game>().white_pieces[y_ord, x_ord] = null;
            }

            GameObject CP = controller.GetComponent<Game>().GetPosition(x_ord, y_ord);//gets the chesspiece that is being taken 
            Destroy(CP);//and destroys it
        }

        //temporary x and y coordinates that store the position of our piece
        int xt = reference.GetComponent<Chesspiece>().GetXBoard();
        int yt = reference.GetComponent<Chesspiece>().GetYBoard();

        if (!Castle)
        {
            //move the piece
            Move(xt,yt,0);
        }
        else
        {
            //move the piece
            Move(xt,yt,0);
            //if the king is 1 from the rook 
            if (7 - CPS.XBoard == 1)
            {
                Move(xt + 3, yt, -1);
            }
            else
            {
                Move(xt-4, yt, 1);
            }
        }
        controller.GetComponent<Game>().SwitchPlayer();

    }

    //Set coordinates of the move plate
    //different to SetCoords of Game.cs
    public void SetCoords(int x, int y)
    {
        x_ord = x;
        y_ord = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }
    public GameObject GetReference()
    {
        return reference;
    }

     

}
