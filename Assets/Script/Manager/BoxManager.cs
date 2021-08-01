using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance;
    public GameObject box_prefab;
    public Dictionary<GameObject, int> boxes = new Dictionary<GameObject, int>();
    public List<GameObject> boxlist = new List<GameObject>();
    private int current_id = 1;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            BoxGenerate();
        }
        foreach (var box in boxlist)
        {
            SaveNewBox(box);
        }
    }

    private void BoxGenerate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,LayerMask.GetMask("Ground"))) {
            GameObject box = Instantiate(box_prefab, new Vector3(hit.point.x, 5, hit.point.z),Quaternion.identity);
            boxlist.Add(box);
        }
    }

    public Vector3 BoxCleanPos(int id) {
        int col = (id-1) % 6;
        int row = (id - 1) / 6;
        Vector3 v = new Vector3(-6.0f + col * 2.0f,0.5f,-6.0f + row*2.0f);
        return v;
    }

    private void SaveNewBox(GameObject box) { 
        if(box.transform.position.y > 0.51f || boxes.ContainsKey(box))
        {
            return;
        }
        boxes[box] = current_id;
        current_id++;
    }

    public bool LockBox(GameObject box,GameObject robot) {
        BoxData boxData = box.GetComponent<BoxData>();
        if (boxData == null) {
            return false;
        }
        if (boxData.working_robot == null) 
        {
            boxData.working_robot = robot;
        }
        if (boxData.working_robot != robot)
        {
            return false;
        }
        return true;
    }

    public bool FreeBoxLock(GameObject box, GameObject robot) {
        BoxData d = box.GetComponent<BoxData>();
        if (d == null)
        {
            return false;
        }
        if (d.working_robot == null)
        {
            return true;
        }
        if (d.working_robot != robot)
        {
            return false;
        }
        d.working_robot = null;
        return true;
    }
}
