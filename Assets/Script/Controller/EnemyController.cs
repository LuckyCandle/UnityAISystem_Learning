using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum State
    {
        Idle,   // 待命状态
        Attack, // 进攻敌方
        Back,   // 回归原位
        Dead,   // 死亡
    }
    public State state;    // AI当前状态
    GameObject invader = null;      // 入侵者GameObject

    public float moveSpeed = 1.0f;          // 移动速度
    public float turnSpeed = 3.0f;          // 转身速度
    public float maxChaseDist = 11.0f;      // 最大追击距离
    public float maxLeaveDist = 2.0f;      // 最大离开原位距离

    [SerializeField]
    private Vector3 basePosition;       // 原始位置
    private Quaternion baseDirection;   // 原始方向
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

    #region //转向移动函数
    // 是否正在面对入侵者，即已经正确瞄准
    bool IsFacingInvader()
    {
        if (invader == null)
        {
            return false;
        }
        Vector3 v1 = invader.transform.position - transform.position;
        v1.y = 0;
        // Vector3.Angle获得的是一个0~180度的角度，和参数两个向量顺序无关
        if (Vector3.Angle(transform.forward, v1) < 1)
        {
            return true;
        }
        return false;
    }

    // 转向入侵者方向，每次只转一点，速度受turnSpeed控制
    void RotateToInvader()
    {
        if (invader == null)
        {
            return;
        }
        Vector3 v1 = invader.transform.position - transform.position;
        v1.y = 0;
        // 结合叉积和Rotate函数进行旋转，很简洁很好用，建议掌握
        // 使用Mathf.Min(turnSpeed, Mathf.Abs(angle))是为了严谨，避免旋转过度导致的抖动
        Vector3 cross = Vector3.Cross(transform.forward, v1);
        float angle = Vector3.Angle(transform.forward, v1);
        transform.Rotate(cross, Mathf.Min(turnSpeed, Mathf.Abs(angle)));
    }

    // 转向参数指定的方向，每次只转一点，速度受turnSpeed控制。这里有点不够严谨，参考上面的方法
    void RotateToDirection(Quaternion rot)
    {
        Quaternion.RotateTowards(transform.rotation, rot, turnSpeed);
    }

    // 是否正位于某个点， 注意float比较时绝不能采用 == 判断
    bool IsInPosition(Vector3 pos)
    {
        Vector3 v = pos - transform.position;
        v.y = 0;
        return v.magnitude < 0.05f;
    }

    // 移动到某个点，每次只移动一点。也不严谨，有可能超过目标一点点
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
            // 方向不对的话，转一下
            transform.rotation = Quaternion.RotateTowards(transform.rotation, baseDirection, turnSpeed);
        }
        else if (state == State.Attack)
        {
            if (invader != null)
            {
                if (Vector3.Distance(invader.transform.position, transform.position) > maxChaseDist)
                {
                    // 与敌人距离过大，追丢的情况
                    state = State.Back;
                    return;
                }
                if (Vector3.Distance(basePosition, transform.position) > maxLeaveDist)
                {
                    // 离开原始位置过远的情况
                    state = State.Back;
                    return;
                }
                if (Vector3.Distance(invader.transform.position, transform.position) > maxChaseDist / 2)
                {
                    // 追击敌人
                    MoveToPosition(invader.transform.position);
                }
                // 转向敌人
                if (!IsFacingInvader())
                {
                    RotateToInvader();
                }
                else
                {// 开火
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
