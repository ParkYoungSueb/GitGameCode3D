using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Player player;
    Vector3 p_moveVec;
    float timer;

    void Awake()
    {
        timer = 0;
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        transform.Rotate(Vector3.up * 30 * Time.deltaTime);
        Move();

        if (timer > 11)
        {
            Destroy(gameObject);
        }
    }

    void Move() 
    {
        p_moveVec = new Vector3(0, 0, -Time.deltaTime).normalized;

        transform.position += p_moveVec * 3 * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.Get_health -= 30;
            Destroy(gameObject);
        }
    }


}
