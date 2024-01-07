using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    private Rigidbody2D pedestBody;
    public float pedestrianSpeed;
    public float changeDirInterval;
    private float timeCounter = 0.0f;
    private bool leftTrueRightFalse;

    private void Start()
    {
        pedestBody = GetComponent<Rigidbody2D>();
        leftTrueRightFalse = true;
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter > changeDirInterval)
        {
            timeCounter = 0.0f;

            if (leftTrueRightFalse) {
                pedestBody.velocity = new Vector2(pedestrianSpeed, 3.0f);
                leftTrueRightFalse = false;
            }
            else
            {
                pedestBody.velocity = new Vector2(-pedestrianSpeed, 3.0f);
                leftTrueRightFalse = true;
            }
        }
    }
}
