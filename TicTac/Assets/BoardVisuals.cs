using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardVisuals : MonoBehaviour
{

    [SerializeField] private MeshRenderer tableMesh;
    [SerializeField] private Color playerColor;
    [SerializeField] private Color aiColor;

    public void Start()
    {
        TicTacToe.Instance.OnPlayerWin += OnPlayerWin;
        TicTacToe.Instance.OnAIWin += OnAIWin;
        TicTacToe.Instance.OnTurnChange += OnTurnChange;
    }

    public void OnPlayerWin()
    {

    }

    public void OnAIWin()
    {

    }

    public void OnTurnChange(Player player)
    {
        if(player.GamePieceType == GamePiece.Type.O)
        {
            PlayersTurn();
        }
        else
        {
            AIsTurn();
        }
    }

    private void PlayersTurn()
    {
        tableMesh.material.color = playerColor;
    }
    
    private void AIsTurn()
    {
        tableMesh.material.color = aiColor;
    }

}
