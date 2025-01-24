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
    private string currentPlayer = "X";
    private static readonly Random random = new();
    private readonly Dictionary<string, int> scores = new Dictionary<string, int>(){
        {"X", 1},
        {"O", -1},
        {"tie", 0}
    };

    
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
                quads[3*x+y].text = board[x,y];
                currentPlayer = ai;
                BestMove();
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
            if(Equals3(board[i,0], board[i,1], board[i,2])){
                winner = board[i,0];
            }
        }

        // Check for line in vertical
        for (int i = 0; i < 3; i++){
            if(Equals3(board[0,i], board[1,i], board[2,i])){
                winner = board[0,i];
            }
        }

        // Check for line in diagonals
        if(Equals3(board[0,0], board[1,1], board[2,2])){
            winner = board[0,0];
        }
        if(Equals3(board[2,0], board[1,1], board[0,2])){
            winner = board[2,0];
        }

        int openSpots = 0;
        for (int i = 0; i < 3; i++){
            for (int j = 0; j < 3; j++){
                if(board[i,j] == ""){
                    openSpots++;
                }
            }
        }

        if(winner == "" && openSpots == 0){
            return "tie";
        }else{
           return winner;
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
        if(currentPlayer == ai) BestMove();
    }

    void NextMove(){
        if(currentPlayer == ai) {

            // Dumb
            List<(int, int)> avaiablePositions = new();
            for (int i = 0; i < 3; i++){
                for (int j = 0; j < 3; j++){
                    if(board[i,j] == "") avaiablePositions.Add((i,j));
                }
            }
            int r = random.Next(avaiablePositions.Count);
            (int, int) move = avaiablePositions[r];

            board[move.Item1, move.Item2] = ai;

            currentPlayer = human;
        }
    }

    void BestMove(){
        if(currentPlayer == ai) {

            int bestScore = int.MinValue;
            var move = (-1, -1);
            for (int i = 0; i < 3; i++){
                for (int j = 0; j < 3; j++){
                    if(board[i,j] == "") {
                        board[i,j] = ai;
                        int score = Minmax(board, 0, false);
                        board[i,j] = "";
                        if(score > bestScore){
                            bestScore = score;
                            move = (i,j);
                        }
                    }
                }
            }

            board[move.Item1, move.Item2] = ai;
            quads[3*move.Item1+move.Item2].text = board[move.Item1,move.Item2];

            currentPlayer = human;
        }
    }

    int Minmax(string[,] board, int depth, bool isMaximizing){
        string result = CheckWinner();
        if(result != ""){
            return scores[result];
        }

        if(isMaximizing){
            int bestScore = int.MinValue;
            for (int i = 0; i < 3; i++){
                for (int j = 0; j < 3; j++){
                    if(board[i,j] == ""){
                        board[i,j] = ai;
                        int score = Minmax(board, depth++, false);
                        board[i,j] = "";
                        bestScore = Math.Max(score, bestScore);
                    }
                }
            }
            return bestScore;
        }else{
            int bestScore = int.MaxValue;
            for (int i = 0; i < 3; i++){
                for (int j = 0; j < 3; j++){
                    if(board[i,j] == ""){
                        board[i,j] = human;
                        int score = Minmax(board, depth++, true);
                        board[i,j] = "";
                        bestScore = Math.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }
}
