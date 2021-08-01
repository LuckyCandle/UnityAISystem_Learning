using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToOrigin : Action
{
    public SharedTransform origin;

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
        if (IsInPosition(origin.Value.position))
        {
            if (transform.rotation != origin.Value.rotation) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, origin.Value.rotation, 3.0f);
                return TaskStatus.Running;
            }
            return TaskStatus.Success;
        }
        MoveToPosition(origin.Value.position);
        return TaskStatus.Running;
    }
}
