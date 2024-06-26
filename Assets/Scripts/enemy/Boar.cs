using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : enemy
{

    protected override void Awake()
    {
        base.Awake();
        prtalState = new BoarPrtalState();
        chaseState = new BoarChaseState();
    }


}
