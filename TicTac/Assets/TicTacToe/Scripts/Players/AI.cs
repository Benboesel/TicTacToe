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
    [SerializeField] private Transform bezierPoint_ToCup;
    [SerializeField] private Transform bezierPoint_ToTable;
    [SerializeField] private Transform bezierPoint_ToBody;
    [SerializeField] private AnimationCurve moveHandMotionCurve;

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
            if(gamePiece != null && gamePiece.transform.position.y > highestPosition)
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
        yield return StartCoroutine(MoveHandTo(rightHand, bezierPoint_ToCup, gamePiece.transform.position, gamePiece.transform.rotation, 1f));
        gamePiece.SetIsKinematic(true);
        gamePiece.transform.SetParent(rightHand);
        yield return StartCoroutine(MoveHandTo(rightHand, bezierPoint_ToTable, targetCell.transform.position + (Vector3.up * .05f) , targetCell.transform.rotation, 1f));
        gamePiece.transform.SetParent(null);
        targetCell.AddPieceToCell(gamePiece);
        yield return StartCoroutine(MoveHandTo(rightHand, bezierPoint_ToBody, rightHandStatic.transform.position, rightHandStatic.transform.rotation, 1f));
    }

    private IEnumerator MoveHandTo(Transform obj, Transform controlPoint, Vector3 targetPosition, Quaternion targetRotation, float animationTime)
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

            obj.transform.position = GetQuadraticBezierPoint(moveHandMotionCurve.Evaluate(percentageDone), initialPosition, controlPoint.position, targetPosition);
            obj.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, moveHandMotionCurve.Evaluate(percentageDone));
            if (percentageDone == 1)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private Vector3 GetQuadraticBezierPoint(float percentDone, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        //http://www.theappguruz.com/blog/bezier-curve-in-games//
        //Equation above website: (1-t)2P0 + 2(1-t)tP1 + t2P2 
        Vector3 vector;
        vector = Mathf.Pow(1 - percentDone, 2) * p0;
        vector += 2 * (1 - percentDone) * percentDone * p1;
        vector += Mathf.Pow(percentDone, 2) * p2;
        return vector;
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
