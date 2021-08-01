using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager instance;
    [Header("===== ģ���Ӿ������߲�����=====")]
    public float viewRadius = 8.0f;
    public float viewNumber = 30;

    public GameObject Player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            if (instance != this) {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
    public void VisualSimulation(bool isAi,Transform _transform)
    {
        Vector3 forward_left = Quaternion.Euler(0, -45, 0) * _transform.forward * viewRadius;
        for (int i = 0; i <= viewNumber; i++)
        {
            //30������90����ƽ�֣�ÿ������ת3��
            Vector3 v = Quaternion.Euler(0, (90.0f / viewNumber) * i, 0) * forward_left;
            Vector3 pos = _transform.position + v;
            //�������ߣ���������屾���յ��ǸղŶ���Vecotr3λ�ã�
            Ray ray = new Ray(_transform.position, v);
            RaycastHit hit;
            if (!isAi)
            {
                LayerMask mask = LayerMask.GetMask("Wall", "Enemy");
                if (Physics.Raycast(ray, out hit, viewRadius, mask))
                {
                    pos = hit.point;
                }
                Debug.DrawLine(_transform.position, pos, Color.blue);
            }
            else {
                LayerMask mask = LayerMask.GetMask("Wall", "Player");
                if (Physics.Raycast(ray, out hit, viewRadius, mask))
                {
                    pos = hit.point;
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
                        Player = hit.transform.gameObject;
                    }
                }
                Debug.DrawLine(_transform.position, pos, Color.red);
            }

        }
    }
}
