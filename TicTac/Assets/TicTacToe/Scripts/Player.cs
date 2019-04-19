using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Score;

    public void AddToScore()
    {
        Score++;
    }

    public void ResetScore()
    {
        Score = 0;
    }
}
