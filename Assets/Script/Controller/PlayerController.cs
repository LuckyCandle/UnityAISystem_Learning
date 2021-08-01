using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        FaceToMousePosition();
        AIManager.instance.VisualSimulation(false,transform);
    }

    private void FixedUpdate()
    {
        //¿ØÖÆplayerµÄÒÆ¶¯
        rb.velocity = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical")).normalized * 5.0f;
    }

    private void FaceToMousePosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }

}
