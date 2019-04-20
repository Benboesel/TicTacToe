using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{
    [SerializeField] private GamePieceContainer gamePieceContainer;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform head;
    [SerializeField] private Transform rightHandStatic;

    public Cell GetBestMove()
    {
        List<Cell> emptyCells = new List<Cell>();
        foreach(Cell cell in TicTacToe.Instance.cells)
        {
            if(cell.IsEmpty())
            {
                emptyCells.Add(cell);
            }
        }
        int randomCellInt = Random.Range(0, emptyCells.Count - 1);
        return emptyCells[randomCellInt];
    }
    
    public GamePiece GetAGamePiece()
    {
        GamePiece highestPiece = null;
        float highestPosition = 0;
        foreach(GamePiece gamePiece in gamePieceContainer.gamePieces)
        {
            if(gamePiece.transform.position.y > highestPosition)
            {
                highestPosition = gamePiece.transform.position.y;
                highestPiece = gamePiece;
            }
        }
        return highestPiece;
    }

    public void GrabShape()
    {

    }

    public void PlacePieceOnBoard()
    {

    }

    private IEnumerator MakeMove()
    {
        Cell targetCell = GetBestMove();
        GamePiece gamePiece = GetAGamePiece();
        yield return StartCoroutine(MoveTo(rightHand, gamePiece.transform.position, gamePiece.transform.rotation, 1f, AnimationCurve.EaseInOut(0, 0, 1, 1)));
        gamePiece.SetIsKinematic(true);
        gamePiece.transform.SetParent(rightHand);
        yield return StartCoroutine(MoveTo(rightHand, targetCell.transform.position + (Vector3.up * .05f) , targetCell.transform.rotation, 1f, AnimationCurve.EaseInOut(0, 0, 1, 1)));
        gamePiece.transform.SetParent(null);
        targetCell.AddPieceToCell(gamePiece);
        yield return StartCoroutine(MoveTo(rightHand, rightHandStatic.transform.position, rightHandStatic.transform.rotation, 1f, AnimationCurve.EaseInOut(0, 0, 1, 1)));
    }

    private IEnumerator MoveTo(Transform obj, Vector3 targetPosition, Quaternion targetRotation, float animationTime, AnimationCurve motionCurve)
    {
        float startTime = Time.time;
        Vector3 initialPosition = obj.transform.position;
        Quaternion initialRotation = obj.transform.rotation;

        while (true)
        {
            float percentageDone = (Time.time - startTime) / animationTime;
            if (percentageDone > 1)
            {
                percentageDone = 1;
            }
            obj.transform.position = Vector3.Lerp(initialPosition, targetPosition, motionCurve.Evaluate(percentageDone));
            obj.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, motionCurve.Evaluate(percentageDone));
            if (percentageDone == 1)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
    }

    public override void StartTurn()
    {
        base.StartTurn();
        StartCoroutine(MakeMove());

    }
}
