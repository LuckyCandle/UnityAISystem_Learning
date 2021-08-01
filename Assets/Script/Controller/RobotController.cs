using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public GameObject working_box;
    public bool going_back = true;

    // Update is called once per frame
    void Update()
    {
        UpdateStateMachine();
    }

    private bool CleanBox(GameObject box) {
        Vector3 clean_pos = BoxManager.instance.BoxCleanPos(BoxManager.instance.boxes[box]);
        if (Vector3.Distance(box.transform.position, clean_pos) > 0.05f)
        {
            //MoveTowards(current: Vector3, target: Vector3, maxDistanceDelta: float) : Vector3
            Vector3 to = clean_pos - box.transform.position;
            box.transform.position += to.normalized * Mathf.Min(0.05f, Vector3.Distance(box.transform.position, clean_pos));
            transform.position = box.transform.position + to.normalized * -0.5f;
            return false;
        }
        return true;
    }

    private void UpdateStateMachine() {
        //如果当前没有正在搬运的货物，则从box中查找需要搬运的货物
        if (working_box == null) {
            foreach (var item in BoxManager.instance.boxes)
            {
                if (BoxManager.instance.LockBox(item.Key,gameObject) == false)
                {
                    continue;
                }
                Vector3 clean_pos = BoxManager.instance.BoxCleanPos(item.Value);
                if (Vector3.Distance(item.Key.transform.position, clean_pos) > 0.05f)
                {
                    // 找到一个需要搬运的货物，设置为当前正在搬的
                    working_box = item.Key;
                    break;
                }
            }
        }

        //如果当前正在搬运货物
        if (working_box != null) {
            if (going_back == false)
            {
                if (CleanBox(working_box))
                {
                    BoxManager.instance.FreeBoxLock(working_box, gameObject);
                    working_box = null;
                    going_back = true;
                }
            }
            else {
                //正在跑向货物的状态
                if (Vector3.Distance(working_box.transform.position, transform.position) > 0.05f)
                {
                    //MoveTowards(current: Vector3, target: Vector3, maxDistanceDelta: float) : Vector3
                    Vector3 to = working_box.transform.position - transform.position;
                    float f = to.magnitude / 0.05f;
                    to /= f;
                    transform.position += to;
                }
                else
                {
                    going_back = false;
                }
            }
        }
    }
}
