using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseMotion : MonoBehaviour
{
    [SerializeField] private float xWidth;
    [SerializeField] private float yHeight;
    [SerializeField] private float zDepth;
    [SerializeField] private float speed;

    private float seed;

    private void Start()
    {
        seed = Random.Range(0f, 1f);
    }

    void Update()
    {
        Vector3 targetPosition;
        targetPosition.x = (Mathf.PerlinNoise(Time.time * speed, seed) * xWidth) - xWidth/2;
        targetPosition.y = (Mathf.PerlinNoise(Time.time * speed, seed) * yHeight) - yHeight/2;
        targetPosition.z = (Mathf.PerlinNoise(Time.time * speed, seed) * zDepth) -zDepth/2;
        transform.localPosition = targetPosition;
    }
}
