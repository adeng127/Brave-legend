using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected enemy currentEnemy;
    public abstract void OnEnter(enemy enemy);
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
    public abstract void OnExit();

}
