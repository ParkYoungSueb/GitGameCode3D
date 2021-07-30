using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tower : MonoBehaviour
{
    [SerializeField]
    private float m_range;
    [SerializeField]
    private float m_spinSpeed = 0f;
    [SerializeField]
    private Transform GunBody;
    [SerializeField]
    private GameObject Missile;
    [SerializeField]
    private Transform target;

    float timer;
    int waitingTime;
    [SerializeField]
    private LayerMask m_layerMask;
    [SerializeField]
    private GameManager gamemanger;

    [SerializeField]
    private float m_fireRate;
    float currentFireRate;


    void SearchEnemy()
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_range, m_layerMask);
        Transform t_shortestTarget = null;

        if (t_cols.Length > 0) 
        {
            float t_shortestDistance = Mathf.Infinity;

            foreach (Collider t_collTarget in t_cols)
            {
                float t_distance = Vector3.SqrMagnitude(transform.position - t_collTarget.transform.position);

                if (t_shortestDistance > t_distance) 
                {
                    t_shortestDistance = t_distance;
                    t_shortestTarget = t_collTarget.transform;
                }
            }
        }
        target = t_shortestTarget;
    }

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        currentFireRate = m_fireRate;
        timer = 0;
        waitingTime = 1;
        InvokeRepeating("SearchEnemy", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) 
        {
            GunBody.Rotate(new Vector3(0,45,0) * Time.deltaTime);
        }
        else 
        {
            Quaternion t_lookHotation = Quaternion.LookRotation(target.position);

            Vector3 t_euler = Quaternion.RotateTowards(GunBody.rotation
                , t_lookHotation,m_spinSpeed * Time.deltaTime).eulerAngles;

            GunBody.rotation = Quaternion.Euler(0,t_euler.y ,0);

            //조준해야 될 최종방향
            Quaternion t_fireRotation = Quaternion.Euler(0, t_lookHotation.eulerAngles.y,0);

            //각도 조절
            if (Quaternion.Angle(GunBody.rotation, t_fireRotation) < 5f) 
            {
                currentFireRate -= Time.deltaTime;

                if (currentFireRate <= 0) 
                {
                    currentFireRate = m_fireRate;
                    timer += Time.deltaTime;

                    if (timer > waitingTime)
                    {
                        //Action
                        Fire();
                        timer = 0;
                    }
                }
            }

        }
    }

    void Fire()
    {
        if (gamemanger.Get_isBattle) 
        {
            StartCoroutine(Shot());
        }
    }


    IEnumerator Shot() 
    {
        //1.총알 발사
        //Prefes 생성
        GameObject IntantBullet = Instantiate(Missile, GunBody.position,
            GunBody.rotation);
        Rigidbody bulletRigid = IntantBullet.GetComponent<Rigidbody>();
        //속도 조절
        bulletRigid.velocity = GunBody.forward;

        yield return new WaitForSeconds(1f);
    }

}
