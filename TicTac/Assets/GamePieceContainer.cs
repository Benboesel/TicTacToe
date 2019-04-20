using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePieceContainer : MonoBehaviour
{
    public List<GamePiece> gamePieces = new List<GamePiece>();
    public GamePiece.Type containerType;
    public int MinPieceCount = 5;
    public int refillAmount = 5;
    public float SpawnWaitTime = 1f;
    public Transform pieceSpawnPoint;
    public GamePiece gamePiecePrefab;
    private Coroutine spawnCoroutine;

    public void OnTriggerEnter(Collider other)
    {
        GamePiece gamePiece = other.GetComponent<GamePiece>();
        if (gamePiece != null && gamePiece.type == containerType && !gamePieces.Contains(gamePiece))
        {
            gamePieces.Add(gamePiece);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        GamePiece gamePiece = other.GetComponent<GamePiece>();
        if (gamePiece != null && gamePiece.type == containerType && gamePieces.Contains(gamePiece))
        {
            gamePieces.Remove(gamePiece);
        }
        CheckAmount();
    }

    private void CheckAmount()
    {
        if(gamePieces.Count < MinPieceCount)
        {
            if(spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(AddMorePieces());
            }
        }
    }

    private IEnumerator AddMorePieces()
    {
        while(gamePieces.Count < MinPieceCount + refillAmount)
        {
            GamePiece gamePiece = Instantiate(gamePiecePrefab) as GamePiece;
            gamePiece.transform.parent = this.transform;
            gamePiece.transform.position = pieceSpawnPoint.position;
            gamePiece.transform.rotation = Random.rotation;
            yield return new WaitForSeconds(SpawnWaitTime);
        }
        spawnCoroutine = null;
    }

}
