using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonateCircle : MonoBehaviour
{
    private bool _canDealDamage;


    // Update is called once per frame
    void Update()
    {
        
    }

    protected void Explode()
    {
        // spawn explosion particle
        // deal damage
        _canDealDamage = true;
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
