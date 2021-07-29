using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxShotDelay;
    public float curShotDelay;
    public ObjectManager objectManager;
    // public GameObject bulletObj;

    //따라오기
    public Vector3 followPos;
    public int followDelay;
    public Transform Parent;
    public Queue<Vector3> parentPos;

    void Awake()
    {
        parentPos = new Queue<Vector3>();
    }

    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    void Watch()
    {
        //#.Input Pos
        //플레이어가 가만히 있으면 집어넣지 않음
        if (!parentPos.Contains(Parent.position)) //값을 포함하고 있는지 ㅊ크
        {
            //Queue = FIFO (First Input First Out) 먼저들어가면 먼저나옴
            parentPos.Enqueue(Parent.position);
        }

        //#.Output Pos
        //바로 주면 위치 겹치니 딜레이줌
        if (parentPos.Count > followDelay)
        {
            //followPos 갱신
            followPos = parentPos.Dequeue();
        } //parentPos의 queue이 안차면
        else if (parentPos.Count < followDelay)
        {
            followPos = Parent.position;
        }
    }

    void Follow()
    {
        //해당 오브젝트 위치값으로 넣어줌
        transform.position = followPos;
    }

    //총알 만듬
    void Fire()
    {
        if (!Input.GetButton("Fire1"))
        {
            return;
        }

        if (curShotDelay < maxShotDelay)
        {
            return;
        }

        //Power One
        //매개변수 오브젝트를 생성하는 함수Instantiate
        GameObject bullet = objectManager.MakeObj("Follower_Bullet");
        bullet.transform.position = transform.position;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}
