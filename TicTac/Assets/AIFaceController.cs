using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFaceController : MonoBehaviour
{
    public Animator faceAnim;
    public float minBlinkTime;
    public float maxBlinkTime;
    int blinkHash = Animator.StringToHash("Blink");
    int happyHash = Animator.StringToHash("Happy");
    int bigEyesHash = Animator.StringToHash("BigEyes");
    int madHash = Animator.StringToHash("Mad");
    int dizzyHash = Animator.StringToHash("Dizzy");

    public enum FaceStates
    {
        Idle,
        Happy,
        BigEyes,
        Mad,
        Dizzy
    }

    public void Start()
    {
        StartCoroutine(Blink());
        TicTacToe.Instance.OnPlayerWin += Mad;
        TicTacToe.Instance.OnAIWin += BigEyed;
        TicTacToe.Instance.OnNewGame += Idle;
    }

    private void Idle()
    {
        SetState(FaceStates.Idle);
    }

    private void BigEyed()
    {
        SetState(FaceStates.BigEyes);
    }

    private void Mad()
    {
        SetState(FaceStates.Mad);
    }

    public void SetState(FaceStates state)
    {
        bool happyState, bigEyedState, madState, dizzyState;
        happyState = bigEyedState = madState = dizzyState = false;

        switch (state)
        {
            case FaceStates.Happy:
                happyState = true;
                break;

            case FaceStates.BigEyes:
                bigEyedState = true;
                break;

            case FaceStates.Mad:
                madState = true;
                break;

            case FaceStates.Dizzy:
                dizzyState = true;
                break;
        }
        faceAnim.SetBool(happyHash, happyState);
        faceAnim.SetBool(bigEyesHash, bigEyedState);
        faceAnim.SetBool(madHash, madState);
        faceAnim.SetBool(dizzyHash, dizzyState);
    }

    IEnumerator Blink()
    {
        while (true)
        {
            float waitTime = Random.Range(minBlinkTime, maxBlinkTime);
            faceAnim.SetBool(blinkHash, true);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        GamePiece gamePiece = collision.transform.GetComponent<GamePiece>();
        if(gamePiece != null)
        {
            SetState(FaceStates.Dizzy);
            gamePiece.Explode(Quaternion.Euler(90,0,0));
        }
    }
}
