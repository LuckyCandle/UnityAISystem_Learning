using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarFromTarget : Conditional
{
    public SharedTransform target;
    public SharedTransform origin;

    public override void OnAwake() {
        
    }

    public override TaskStatus OnUpdate()
    {
        //目标距离过远或者离开初始位置过远，Task完成
        if (Vector3.Distance(target.Value.position, transform.position) > 15.0f) {
            return TaskStatus.Success;
        }
        if (Vector3.Distance(origin.Value.position, transform.position) > 10.0f) {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
