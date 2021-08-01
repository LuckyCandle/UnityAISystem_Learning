using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePosition :Action
{
    public SharedTransform target;

    void MoveToPosition(Vector3 pos)
    {
        Vector3 v = pos - transform.position;
        v.y = 0;
        transform.position += v.normalized * 2.0f * Time.deltaTime;
    }

    bool IsInPosition(Vector3 pos)
    {
        Vector3 v = pos - transform.position;
        v.y = 0;
        return v.magnitude < 0.05f;
    }

    public override TaskStatus OnUpdate()
    {
        if (IsInPosition(target.Value.position)) {
            return TaskStatus.Success;
        }
        MoveToPosition(target.Value.position);
        return TaskStatus.Running;
    }
}
