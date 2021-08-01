using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAction : Action {

    public SharedTransform target;

    // 是否正在面对入侵者，即已经正确瞄准
    bool IsFacingTarget()
    {
        if (target.Value == null)
        {
            return false;
        }
        Vector3 v1 = target.Value.position - transform.position;
        v1.y = 0;
        if (Vector3.Angle(transform.forward, v1) < 1)
        {
            return true;
        }
        return false;
    }

    // 转向入侵者方向，每次只转一点，速度受turnSpeed控制
    void RotateToTarget()
    {
        if (target.Value == null)
        {
            return;
        }
        Vector3 v1 = target.Value.position - transform.position;
        v1.y = 0;
        Vector3 cross = Vector3.Cross(transform.forward, v1);
        float angle = Vector3.Angle(transform.forward, v1);
        transform.Rotate(cross, Mathf.Min(2, Mathf.Abs(angle)));
    }

    public override void OnAwake()
    {
    }

    public override TaskStatus OnUpdate()
    {
        if (IsFacingTarget())
        {
            // 返回值不同对状态树会产生巨大影响，可以对比测试
            return TaskStatus.Running;
        }
        RotateToTarget();
        return TaskStatus.Running;
    }

}
