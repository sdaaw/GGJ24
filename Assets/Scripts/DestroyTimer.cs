using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{

    private float _destroyTimer = 0;

    [SerializeField]
    private float _destroyTimerMax = 1;


    // Update is called once per frame
    void Update()
    {
        _destroyTimer += Time.deltaTime;

        if (_destroyTimer >= _destroyTimerMax)
        {
            _destroyTimer = 0;
            Destroy(this.gameObject);
        }

    }
}
