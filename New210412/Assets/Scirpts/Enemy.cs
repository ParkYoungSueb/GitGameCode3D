using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed;
    [SerializeField]
    private int health;
    public string enmey_name;
    public int enemySocre;
    Rigidbody2D rigid;

    [SerializeField]
    private GameObject bulletObjA;
    [SerializeField]
    private GameObject bulletObjB;

    public GameObject Player;
    public ObjectManager objectManager;

    [SerializeField]
    private GameObject ItemBoom;
    [SerializeField]
    private GameObject ItemPower;

    public GameManager gamemanager;

    [SerializeField]
    private float maxShotDelay;
    [SerializeField]
    private float curShotDelay;

    Animator anim;

    [SerializeField]
    private int patternIndex;
    [SerializeField]
    private int curPatternCount;
    [SerializeField]
    private int[] maxPatternCount;

    float m_fireRate;
    float currentFireRate;
    float timer;
    int waitingTime;

    void Awake()
    {
        anim = GetComponent<Animator>();
        objectManager = GetComponent<ObjectManager>();
        currentFireRate = m_fireRate;
    }

    void Update()
    {
        Fire();
        Reload();
    }

    // 컴포넌트가 활성화 될 때 호출되는 생명주기 함수
    void OnEnable()
    {
        switch (enmey_name)
        {
            case "B1":
                health = 1700;
                Invoke("Stop", 2);
                break;

            case "B2":
                health = 3200;
                Invoke("Stop", 2);
                break;

            case "B3":
                health = 4900;
                Invoke("Stop", 2);
                break;

            case "A":
                health = 20;
                break;

            case "N":
                health = 15;
                break;

            case "N1":
                health = 10;
                break;

            case "N2":
                health = 15;
                break;

            case "B":
                health = 30;
                break;

            case "P":
                health = 20;
                break;

            case "R":
                health = 5;
                break;

            case "S":
                health = 10;
                break;

            case "S1":
                health = 15;
                break;

            case "W":
                health = 20;
                break;

            case "Y":
                health = 20;
                break;

            case "Z":
                health = 40;
                break;
        }
    }

    void Stop()
    {
        //내자신이 활성화 되어 있을때
        if (!gameObject.activeSelf)
        {
            return;
        }
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        if (health <= 0)
        {
            return;
        }

        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireFoward()
    {
        //#.Fire 4 Bullet Foward
        GameObject bulletR = objectManager.MakeObj("Enemybullet_n2");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;

        GameObject bulletRR = objectManager.MakeObj("Enemybullet_n");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;

        GameObject bulleLL = objectManager.MakeObj("Enemybullet_n");
        bulleLL.transform.position = transform.position + Vector3.left * 0.3f;

        GameObject bulleLLL = objectManager.MakeObj("EnemyBullet_n");
        bulleLLL.transform.position = transform.position + Vector3.left * 0.45f;

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulleLL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulleLLL.GetComponent<Rigidbody2D>();

        //normalized 단일 백터
        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

        //#.Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireFoward", 2);
        }
        else
        {
            Invoke("Think", 3);
        }
    }

    void FireShot()
    {
        for (int index = 0; index < 5; index++)
        {
            //매개변수 오브젝트를 생성하는 함수Instantiate
            GameObject bullet = objectManager.MakeObj("Enemybullet_n3");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVce = Player.transform.position - transform.position;
            Vector2 randVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVce += randVec;

            rigid.AddForce(dirVce.normalized * 3, ForceMode2D.Impulse);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireShot", 3.5f);
        }
        else
        {
            Invoke("Think", 3);
        }
    }

    void FireArc()
    {
        //#.Fire Arc Continue Fire
        //매개변수 오브젝트를 생성하는 함수Instantiate
        GameObject bullet = objectManager.MakeObj("Enemybullet_n");
        bullet.transform.position = transform.position;
        //회전 초기화 Quaternion하면 0이됨
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVce = new Vector2(Mathf.Cos(Mathf.PI * 8 * curPatternCount / maxPatternCount[patternIndex]), -1);

        rigid.AddForce(dirVce.normalized * 5, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireArc", 0.15f);
        }
        else
        {
            Invoke("Think", 3);
        }
    }

    void FireAround()
    {
        //#. Fire Around
        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int index = 0; index < roundNum; index++)
        {
            //#.Fire Around
            GameObject bullet = objectManager.MakeObj("EnemyBullet_n6");
            bullet.transform.position = transform.position;
            //회전 초기화 Quaternion하면 0이됨
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVce = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum),
               Mathf.Sin(Mathf.PI * 2 * index / roundNum));

            rigid.AddForce(dirVce.normalized * 2, ForceMode2D.Impulse);

            //회전 로직 추가 회전은 z축이니 forward
            Vector3 rotVec = Vector3.forward * 360 * index / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireAround", 0.7f);
        }
        else
        {
            Invoke("Think", 3);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //외각에 닿으면 사라짐
        if (collision.gameObject.tag == "BorderBullet" && enmey_name != "B1" && enmey_name != "B2" && enmey_name != "B3")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        else if (collision.gameObject.tag == "Player_Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            // bullet.Onbullet();
            OnHit(bullet.dmg);
            collision.gameObject.SetActive(false);
        }
    }

    public void OnHit(int dmg)
    {
        if (health <= 0)
        {
            return;
        }

        anim.SetTrigger("Hit");
        health -= dmg;

        if (health <= 0)
        {
            Player playerLogic = Player.GetComponent<Player>();
            playerLogic.score += enemySocre;

            // Invoke("explosion",2);
            anim.SetTrigger("Dead");


            gamemanager.EnemyDeadSound.Play();

            //#.Random Ratio Item Drop\
            int ran = enmey_name == "B1" ? 0 : Random.Range(0, 10);

            if (ran < 8) //Power
            {
                GameObject itemPower = objectManager.MakeObj("Item_Power");
                itemPower.transform.position = transform.position;
                //아이템 생성
                //Instantiate(ItemPower, transform.position, ItemPower.transform.rotation);
            }
            else if (ran < 10) //Boom
            {
                GameObject itemBoom = objectManager.MakeObj("Item_Boom");
                itemBoom.transform.position = transform.position;
                //아이템 생성
                //Instantiate(ItemBoom, transform.position, ItemBoom.transform.rotation);
            }
            
            gameObject.SetActive(false);
            // Destroy(gameObject);

            //회전값 기본으로
            transform.rotation = Quaternion.identity;
            // gamemanager.CallExplosion(transform.position, enmey_name);

            
            //#.Boss Kill
            if (enmey_name == "B1" || enmey_name == "B2" || enmey_name == "B3")
            {
                gamemanager.StageEnd();
            }
            
        }
       
    }


    //총알 만듬
    void Fire()
    {
        if (curShotDelay < maxShotDelay)
        {
            return;
        }

        if (enmey_name == "N" || enmey_name == "S" || enmey_name == "W" || enmey_name == "P" || enmey_name == "N1" || enmey_name == "S1" || enmey_name == "N2" || enmey_name == "A" || enmey_name == "B")
        {
            //Power One
            //매개변수 오브젝트를 생성하는 함수Instantiate
            GameObject bullet = objectManager.MakeObj("Enemybullet_n2");
            bullet.transform.position = transform.position;

            // GameObject bullet = Instantiate(bulletObjA, transform.position,
            // transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            //플레이어 방향으로 쏘기
            Vector3 dirVce = Player.transform.position - transform.position;
            rigid.AddForce(dirVce.normalized * 3, ForceMode2D.Impulse);

            AudioSource sound = GetComponent<AudioSource>();
            sound.Play();
        }
        else if (enmey_name == "Y" || enmey_name == "R" || enmey_name == "Z")
        {
            GameObject bulletR = objectManager.MakeObj("Enemybullet_n3");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            // GameObject bulletR = Instantiate(bulletObjB, transform.position + Vector3.right*0.3f,
            // transform.rotation);

            GameObject bulleLt = objectManager.MakeObj("EnemyBullet_n5");
            bulleLt.transform.position = transform.position + Vector3.left * 0.3f;

            // GameObject bulleLt = Instantiate(bulletObjB, transform.position + Vector3.left * 0.3f,
            // transform.rotation);

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulleLt.GetComponent<Rigidbody2D>();

            //플레이어 방향으로 쏘기
            Vector3 dirVceR = Player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVceL = Player.transform.position - (transform.position + Vector3.left * 0.3f);

            AudioSource sound = GetComponent<AudioSource>();
            sound.Play();

            //normalized 단일 백터
            rigidR.AddForce(dirVceR.normalized * 10, ForceMode2D.Impulse);
            rigidL.AddForce(dirVceL.normalized * 10, ForceMode2D.Impulse);
        }
        curShotDelay = 0;
    }


    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}
