using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum State
    {
        Idle,   // ����״̬
        Attack, // �����з�
        Back,   // �ع�ԭλ
        Dead,   // ����
    }
    public State state;    // AI��ǰ״̬
    GameObject invader = null;      // ������GameObject

    public float moveSpeed = 1.0f;          // �ƶ��ٶ�
    public float turnSpeed = 3.0f;          // ת���ٶ�
    public float maxChaseDist = 11.0f;      // ���׷������
    public float maxLeaveDist = 2.0f;      // ����뿪ԭλ����

    [SerializeField]
    private Vector3 basePosition;       // ԭʼλ��
    private Quaternion baseDirection;   // ԭʼ����
    private WeaponController weaponController;


    private void Start()
    {
        basePosition = transform.position;
        baseDirection = transform.rotation;
        state = State.Idle;
        weaponController = GetComponentInChildren<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.Back) {
            AIManager.instance.VisualSimulation(true, transform);
            if (AIManager.instance.Player != null)
            {
                invader = AIManager.instance.Player;
                state = State.Attack;
            }
        }
        UpdateStateMachine();
    }

    #region //ת���ƶ�����
    // �Ƿ�������������ߣ����Ѿ���ȷ��׼
    bool IsFacingInvader()
    {
        if (invader == null)
        {
            return false;
        }
        Vector3 v1 = invader.transform.position - transform.position;
        v1.y = 0;
        // Vector3.Angle��õ���һ��0~180�ȵĽǶȣ��Ͳ�����������˳���޹�
        if (Vector3.Angle(transform.forward, v1) < 1)
        {
            return true;
        }
        return false;
    }

    // ת�������߷���ÿ��ֻתһ�㣬�ٶ���turnSpeed����
    void RotateToInvader()
    {
        if (invader == null)
        {
            return;
        }
        Vector3 v1 = invader.transform.position - transform.position;
        v1.y = 0;
        // ��ϲ����Rotate����������ת���ܼ��ܺ��ã���������
        // ʹ��Mathf.Min(turnSpeed, Mathf.Abs(angle))��Ϊ���Ͻ���������ת���ȵ��µĶ���
        Vector3 cross = Vector3.Cross(transform.forward, v1);
        float angle = Vector3.Angle(transform.forward, v1);
        transform.Rotate(cross, Mathf.Min(turnSpeed, Mathf.Abs(angle)));
    }

    // ת�����ָ���ķ���ÿ��ֻתһ�㣬�ٶ���turnSpeed���ơ������е㲻���Ͻ����ο�����ķ���
    void RotateToDirection(Quaternion rot)
    {
        Quaternion.RotateTowards(transform.rotation, rot, turnSpeed);
    }

    // �Ƿ���λ��ĳ���㣬 ע��float�Ƚ�ʱ�����ܲ��� == �ж�
    bool IsInPosition(Vector3 pos)
    {
        Vector3 v = pos - transform.position;
        v.y = 0;
        return v.magnitude < 0.05f;
    }

    // �ƶ���ĳ���㣬ÿ��ֻ�ƶ�һ�㡣Ҳ���Ͻ����п��ܳ���Ŀ��һ���
    void MoveToPosition(Vector3 pos)
    {
        Vector3 v = pos - transform.position;
        v.y = 0;
        transform.position += v.normalized * moveSpeed * Time.deltaTime;
    }
    #endregion

    private void UpdateStateMachine() {
        if (state == State.Dead)
        {
            return;
        }

        if (state == State.Idle)
        {
            // ���򲻶ԵĻ���תһ��
            transform.rotation = Quaternion.RotateTowards(transform.rotation, baseDirection, turnSpeed);
        }
        else if (state == State.Attack)
        {
            if (invader != null)
            {
                if (Vector3.Distance(invader.transform.position, transform.position) > maxChaseDist)
                {
                    // ����˾������׷�������
                    state = State.Back;
                    return;
                }
                if (Vector3.Distance(basePosition, transform.position) > maxLeaveDist)
                {
                    // �뿪ԭʼλ�ù�Զ�����
                    state = State.Back;
                    return;
                }
                if (Vector3.Distance(invader.transform.position, transform.position) > maxChaseDist / 2)
                {
                    // ׷������
                    MoveToPosition(invader.transform.position);
                }
                // ת�����
                if (!IsFacingInvader())
                {
                    RotateToInvader();
                }
                else
                {// ����
                    weaponController.Fire();
                }
            }
        }
        else if (state == State.Back)
        {
            invader = null;
            AIManager.instance.Player = null;
            if (IsInPosition(basePosition))
            {
                state = State.Idle;
                return;
            }
            else {
                MoveToPosition(basePosition);
            }
        }
    }
}
