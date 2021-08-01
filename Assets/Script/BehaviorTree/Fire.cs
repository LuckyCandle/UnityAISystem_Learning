using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Action
{
    public void onAwake()
    {
        Debug.Log(gameObject);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}
