using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public bool isEnabled = true;

    private Transform playerPos;

    private void Start()
    {
        playerPos = FindFirstObjectByType<Camera>().transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            transform.LookAt(playerPos);
        }
    }
}
