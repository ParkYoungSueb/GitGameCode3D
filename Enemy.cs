using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject[] obj;
    public enum Type { Zipper, Spider, Ranger, Dragon, car, Groucho, Boss }; 
    public Type enemyType;

    [SerializeField]
    GameObject healthBarBackground;

    [SerializeField]
    Image healthBarFiled;

    Transform target;
    int maxHealth;
    int curHealth;
    float Power;
    int Exp;
    float targetRadius;
    float targetRange;
    float[] delay;
    bool isRecover = false;
    bool isChase;
    bool isDead = false;

    public static bool isBoss = true;

    [SerializeField]
    private BoxCollider meleeArea;
    [SerializeField]
    private BoxCollider SpecialArea;
    [SerializeField]
    private bool isAttack;
    [SerializeField]
    private GameObject box;
    [SerializeField]
    private AudioSource hit_Sound;
    [SerializeField]
    private AudioSource down_Sound;
    [SerializeField]
    private AudioSource attack_Sound;

    Rigidbody rigid;
    BoxCollider boxCollider;
    NavMeshAgent nav;
    Player player;
    GameManager manager;

    //0 idle,1 attack, 2 special,3 deth, 4 hit, 5.run
    [SerializeField]
    private string[] Action_names;

    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        player = target.GetComponent<Player>();
        manager = GameObject.Find("GameManager").transform.GetComponent<GameManager>();
        // healthBarFiled.fillAmount = 1f;

        if (enemyType == Type.Zipper)
        {
            obj = GameObject.FindGameObjectsWithTag("Enemy_z");
            targetRadius = 0.4f;
            targetRange = 0.3f;
            delay = new float[6] { 0.4f, 0.1f, 1.2f, 0.5f, 0.2f, 1.3f };
            maxHealth = 160;
            Exp = 100;
        }
        else if (enemyType == Type.Dragon)
        {
            obj = GameObject.FindGameObjectsWithTag("Drange");
            targetRadius = 0.5f;
            targetRange = 0.7f;
            delay = new float[6] { 0.4f, 0.15f, 1.3f, 0.3f, 0.2f , 1f};
            maxHealth = 250;
            Exp = 150;
        }
        else if (enemyType == Type.Spider) 
        {
            obj = GameObject.FindGameObjectsWithTag("spider");
            targetRadius = 1.2f;
            targetRange = 1.4f;
            delay = new float[6] { 0.1f, 0.5f, 1.3f, 0.3f, 0.2f, 1f };
            maxHealth = 150;
            Exp = 90;
        }
        else if (enemyType == Type.Boss)
        {
            obj = GameObject.FindGameObjectsWithTag("Boss");
            targetRadius = 1.0f;
            targetRange = 1.2f;
            delay = new float[6] { 0.4f, 0.3f, 1.3f, 0.3f, 0.1f, 1f };
            maxHealth = 2000;
            isRecover = true;
            Exp = 0;
        }
        nav = GetComponent<NavMeshAgent>();
        curHealth = maxHealth;

        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        Invoke("ChaseStart", 0.5f);

        
    }

    public Transform Get_target
    {
        get { return target; }
        set { target = value; }
    }

    public int Get_maxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public int Get_curHealth
    {
        get { return curHealth; }
        set { curHealth = value; }
    }

    public GameManager E_manager
    {
        get { return manager; }
        set { manager = value; }
    }

    void ChaseStart()
    {
        isChase = true;
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (nav.enabled) 
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }

        if (curHealth < 0 && enemyType == Type.Boss)
        {
            StartCoroutine(dead_recovery());
        }
    }

    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void Targeting()
    {
        if (!isDead)
        {
            RaycastHit[] rayHits =
               Physics.SphereCastAll(transform.position,targetRadius, 
               transform.forward * 1.7f, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        int ranAction = Random.Range(0, 5);

        if (!player.Get_isDead)
        {
            switch (ranAction)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    // Power = 10 + manager.get_Stage_number() % 10;
                    Power = 12;
                    foreach (GameObject anims in obj)
                    {
                        if (enemyType == Type.Boss)
                        {
                            anims.GetComponentInChildren<Animation>().Play(Action_names[1]);
                        }
                        else
                        {
                            null_check(anims, 1);
                        }
                    }
                    yield return new WaitForSeconds(delay[0]);
                    meleeArea.enabled = true;
                    attack_Sound.Play();
                    player.Get_health -= Mathf.Ceil(player.get_defend() / player.get_defend() + 130 / Power);
                    yield return new WaitForSeconds(delay[1]);
                    meleeArea.enabled = false;
                    yield return new WaitForSeconds(delay[2]);
                    break;
                case 4:
                    Power = 22;
                    foreach (GameObject anims in obj)
                    {
                        if (enemyType == Type.Boss)
                        {
                            anims.GetComponentInChildren<Animation>().Play(Action_names[2]);
                        }
                        else
                        {
                            null_check(anims, 2);
                        }
                    }
                    // obj.GetComponent<Animation>().Play(Action_names[2]);
                    yield return new WaitForSeconds(delay[3]);
                    SpecialArea.enabled = true;
                    attack_Sound.Play();
                    player.Get_health -= Mathf.Ceil(player.get_defend() / player.get_defend() + 130 / Power);
                    yield return new WaitForSeconds(delay[4]);
                    SpecialArea.enabled = false;
                    yield return new WaitForSeconds(delay[5]);
                    // test_v = Vector3.zero;
                    break;
            }
            isChase = true;
            isAttack = false;

            
            foreach (GameObject anims in obj) 
            {
                if (enemyType == Type.Boss)
                {
                    anims.GetComponentInChildren<Animation>().Play(Action_names[0]);
                }
                else
                {
                    null_check(anims, 0);
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDead)
        {
            if (other.transform.tag == "attack_leg")
            {
                curHealth -= player.Get_power / 2;
                hit_Sound.Play();
                //hit
                foreach (GameObject anims in obj)
                {
                    if (enemyType == Type.Boss)
                    {
                        anims.GetComponentInChildren<Animation>().Play(Action_names[4]);
                    }
                    else
                    {
                        null_check(anims, 4);
                        healthBarBackground.SetActive(true);
                        healthBarFiled.fillAmount = (float)curHealth / maxHealth;
                    }
                }
            }
            else if (curHealth <= 0 && enemyType != Type.Boss)
            {
                foreach (GameObject anims in obj)
                {
                    null_check(anims, 3);
                }
                healthBarBackground.SetActive(false);
                isDead = true;
                StartCoroutine(OnDamage());
            }
        }

        //Dead
        
    }

    IEnumerator OnDamage() 
    {
        nav.enabled = false;
        isDead = true;
        player.Get_score += Exp;
        player.Get_p_Exp += Exp;
        yield return new WaitForSeconds(7f);
        Instantiate(box, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    IEnumerator dead_recovery()
    {
        if (isRecover)
        {
            nav.enabled = false;
            curHealth = maxHealth;
            isRecover = false;
            down_Sound.Play();

            foreach (GameObject anims in obj)
            {
                if (enemyType == Type.Boss)
                {
                    anims.GetComponentInChildren<Animation>().Play(Action_names[5]);
                }
                else
                {
                    null_check(anims, 5);
                }
            }

            yield return new WaitForSeconds(6f);

            foreach (GameObject anims in obj)
            {
                if (enemyType == Type.Boss)
                {
                    anims.GetComponentInChildren<Animation>().Play(Action_names[6]);
                }
                else 
                {
                    null_check(anims, 6);
                }
            }
            nav.enabled = true;
            yield return new WaitForSeconds(4f);
        }
        else if (!isRecover)
        {
            nav.enabled = false;
            player.Get_score += Exp;
            player.Get_p_Exp += Exp;
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);

            manager.Get_game_clear = true;

            isBoss = false;
        }
    }

    public bool Boss_extant() 
    {
        return isBoss;
    }

    void null_check(GameObject check_oj,int index)
    {
        if (check_oj != null && !isDead)
        {
            check_oj.GetComponent<Animation>().Play(Action_names[index]);
        }
    }

}

