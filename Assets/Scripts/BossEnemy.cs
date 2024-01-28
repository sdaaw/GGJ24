using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : ShootingEnemy
{
    [SerializeField]
    private float _teleportTimerMax;

    private float _teleportTimer;

    [SerializeField]
    private List<Transform> _teleportPositions = new List<Transform>();


    public void Teleport()
    {
        var randomSpot = _teleportPositions[Random.Range(0, _teleportPositions.Count - 1)];
        transform.position = randomSpot.position;
    }

    protected override void Update()
    {
        _teleportTimer += Time.deltaTime;

        if (_teleportTimer >= _teleportTimerMax)
        {
            Teleport();
            _teleportTimer = 0;
        }

        base.Update();
    }

}
