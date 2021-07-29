using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg;
    public bool isRotate;
    Animator anim;

    // Update is called once per frame
    void Update()
    {
        if (isRotate)
        {
            transform.Rotate(Vector3.forward * 10);
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
        {
            gameObject.SetActive(false);
        }
    }

    /*
    public void Onbullet()
    {
        StartCoroutine(delayTime());
        // Destroy(gameObject);
    }

    IEnumerator delayTime() 
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(4f);
    }
    */
}
