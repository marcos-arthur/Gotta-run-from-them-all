using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

public class TicTacToe : MonoBehaviour
{
    private readonly string[,] board = new string[3,3];
    private string ai = "X";
    private string human = "O";
    private int openSpots = 9;
    private string currentPlayer;
    private static readonly Random random = new();

    
    [SerializeField]
    private List<TMP_Text> quads;
    [SerializeField]
    private TMP_Text playerText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 3; i++){
            for (int j = 0; j < 3; j++){
                board[i,j] = "";
            }
        }

        Toss();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (TMP_Text t in quads){
            int x = int.Parse(t.name[0].ToString());
            int y = int.Parse(t.name[1].ToString());
            t.text = board[x,y];
        }

        string winner = CheckWinner();
        if(winner != ""){
            Debug.Log(winner + " wins");
        }
    }

    public void HandleClick(GameObject quad){
        if(currentPlayer == human){
            TMP_Text textComponent = quad.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            int x = int.Parse(textComponent.name[0].ToString());
            int y = int.Parse(textComponent.name[1].ToString());
            if(textComponent.text == ""){
                board[x,y] = human;
                openSpots--;
                currentPlayer = ai;
                NextMove();
            }
        }
    }

    bool Equals3(string a, string b, string c){
        return a == b && b == c && a != "";
    }

    string CheckWinner(){
        string winner = "";

        // Check for line in horizontal
        for (int i = 0; i < 3; i++){
            if(Equals3(board[0,i], board[1,i], board[2,i])){
                winner = board[0,i];
            }
        }

        // Check for line in vertical
        for (int i = 0; i < 3; i++){
            if(Equals3(board[i,0], board[i,1], board[i,2])){
                winner = board[i,0];
            }
        }

        // Check for line in diagonals
        if(Equals3(board[0,0], board[1,1], board[2,2])){
            winner = board[0,2];
        }
        if(Equals3(board[0,2], board[1,1], board[2,0])){
            winner = board[0,2];
        }

        if(winner == "" && openSpots == 0){
            return "tie";
        }else{
            if(ai == winner) {
                // ai wins
                return "ai";
            }else if(human == winner){
                // human wins
                return "human";
            }else{
                return "";
            }
        }
    }

    void Toss(){
        var n = random.Next(0, 2);
        if(n == 0) {
            human = "X";
            ai = "O";
            currentPlayer = human;
        }
        else {
            human = "O";
            ai = "X";
            currentPlayer = ai;
        }
        
        playerText.text = "Jogador: " + human;
        if(currentPlayer == ai) NextMove();
    }

    void NextMove(){
        if(currentPlayer == ai) {
            List<(int, int)> avaiablePositions = new();
            for (int i = 0; i < 3; i++){
                for (int j = 0; j < 3; j++){
                    if(board[i,j] == "") avaiablePositions.Add((i,j));
                }
            }
            int r = random.Next(avaiablePositions.Count);
            (int, int) move = avaiablePositions[r];

            board[move.Item1, move.Item2] = ai;
            openSpots--;

            currentPlayer = human;
        }
    }
}
