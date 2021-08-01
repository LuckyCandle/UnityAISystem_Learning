using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithInSight : Conditional
{
    // ��Ұ�Ƕ�
    public float fieldOfViewAngle;
    // Ŀ�������Tag
    public string targetTag;
    // ����Ŀ��ʱ����Ŀ��������õ�BahaviorTree�����������ȥ
    public SharedTransform target;
    public SharedTransform origin;
    public SharedVector3 targetPos;
    // ����ָ��Tag�����������
    private Transform[] possibleTargets;

    // ���غ�����Behavior Designerר�õ�Awake
    public override void OnAwake()
    {
        // ����Tag���ҵ��������壬ȫ����������
        var targets = GameObject.FindGameObjectsWithTag(targetTag);
        possibleTargets = new Transform[targets.Length];
        for (int i = 0; i < targets.Length; ++i)
        {
            possibleTargets[i] = targets[i].transform;
        }
    }
    // ���غ�����Behavior Designerר�õ�Update
    public override TaskStatus OnUpdate()
    {
        // �ж�Ŀ���Ƿ�����Ұ�ڣ��������ֵTaskStatus�ܹؼ�����Ӱ������ִ������
        for (int i = 0; i < possibleTargets.Length; ++i)
        {
            if (withinSight(possibleTargets[i], fieldOfViewAngle, 10))
            {
                // ��Ŀ����Ϣ��д������������棬��������Action�Ϳ��Է���������
                target.Value = possibleTargets[i];
                targetPos.Value = target.Value.position;
                // �ɹ��򷵻� TaskStatus.Success
                return TaskStatus.Success;
            }
        }
        // û�ҵ�Ŀ�������һ֡����ִ�д�����
        return TaskStatus.Running;
    }

    // �ж������Ƿ�����Ұ��Χ�ڵķ���
    public bool withinSight(Transform targetTransform, float fieldOfViewAngle, float distance)
    {
        Vector3 direction = targetTransform.position - transform.position;
        if (direction.magnitude > distance)
        {
            return false;
        }
        return Vector3.Angle(direction, transform.forward) < fieldOfViewAngle;
    }
}
