using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField]
    private GameObject idle_box;
    [SerializeField]
    private GameObject destory_box;
    Animator anim;
    [SerializeField]
    private GameObject[] items;

    void Awake() 
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(box_event());
        }
    }

    IEnumerator box_event() 
    {
        idle_box.SetActive(false);
        destory_box.SetActive(true);
        anim.SetTrigger("touch");
        yield return new WaitForSeconds(1.3f);
        int ranCoin = Random.Range(0, items.Length);
        Instantiate(items[ranCoin], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


}
