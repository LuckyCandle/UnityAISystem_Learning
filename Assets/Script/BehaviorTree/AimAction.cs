using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAction : Action {

    public SharedTransform target;

    // �Ƿ�������������ߣ����Ѿ���ȷ��׼
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

    // ת�������߷���ÿ��ֻתһ�㣬�ٶ���turnSpeed����
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
            // ����ֵ��ͬ��״̬��������޴�Ӱ�죬���ԶԱȲ���
            return TaskStatus.Running;
        }
        RotateToTarget();
        return TaskStatus.Running;
    }

}
