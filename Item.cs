using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { MaxHp, poison, Hp, Coin };
    // [SerializeField]
    public Type type;
    Rigidbody rigid;
    // SphereCollider sphereColider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        // sphereColider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 10 * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

}
