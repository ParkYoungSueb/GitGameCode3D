using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Vector3 Offset;

    float speed_move = 3.0f;
    // float speed_rota = 2.0f;

    void Update()
    {
        transform.position = playerTransform.position + Offset;
        // moveObjectFunc();
    }

    void moveObjectFunc() 
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        hAxis = hAxis * speed_move * Time.deltaTime;
        vAxis = vAxis * speed_move * Time.deltaTime;

        transform.Translate(Vector3.right  * hAxis);
        transform.Translate(Vector3.forward * vAxis);
    }

}
