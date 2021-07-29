using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed;

    //경계에 닿 은지 체크
    bool isTouchTop;
    bool isTocuhBottom;
    bool isTouchRight;
    bool isTouchLeft;

    public int life;
    public int score;
    public float Speed;
    [SerializeField]
    private int power;
    [SerializeField]
    private int maxpower;
    public int boom;
    public int maxboom;

    [SerializeField]
    private GameObject bulletObjA;
    [SerializeField]
    private GameObject bulletObjB;
    [SerializeField]
    private GameObject boomEffect;

    [SerializeField]
    private float maxShotDelay;
    [SerializeField]
    private float curShotDelay;

    public bool isDamaged;
    public bool isBoomTime;
    public bool isRespawnTime;

    public ObjectManager objectManager;
    public GameManager gameManager;

    public GameObject[] followers;

    Animator anim;

    AudioSource sound;
    [SerializeField]
    private AudioSource DeadSound;
    [SerializeField]
    private AudioSource BoomSound;
    [SerializeField]
    private AudioSource ItemSound;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        Boom();
        Reload();
    }

    void Move() 
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
        {
            h = 0;
        }

        if ((isTouchTop && v == 1) || (isTocuhBottom && v == -1))
        {
            v = 0;
        }

        Vector3 curPos = transform.position;
        //물리적 이동이라 델타 타입 넣어준다
        Vector3 NextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + NextPos;
    }

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

        switch (power)
        {
            case 1:
                //Power One
                //매개변수 오브젝트를 생성하는 함수Instantiate
                
                GameObject bullet = objectManager.MakeObj("Player_Bullet_A");
                bullet.transform.position = transform.position;
                /*
                GameObject bullet = Instantiate(bulletObjA, transform.position,
                transform.rotation);
                */

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 20, ForceMode2D.Impulse);

                break;
            case 2:
                //Power two
                //매개변수 오브젝트를 생성하는 함수Instantiate
                
                GameObject bulletR = objectManager.MakeObj("Player_Bullet_A");
                bulletR.transform.position = transform.position + Vector3.right * 0.2f;
                
                /*
                GameObject bulletR = Instantiate(bulletObjA, transform.position + 
                Vector3.right * 0.1f,transform.rotation);
                */
                
                GameObject bulletL = objectManager.MakeObj("Player_Bullet_A");
                bulletL.transform.position = transform.position + Vector3.left * 0.2f;

                /*
                GameObject bulletL = Instantiate(bulletObjA, transform.position +
                Vector3.left * 0.1f, transform.rotation);
                */

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                break;

            default:
                //Power One
                //매개변수 오브젝트를 생성하는 함수Instantiate
                
                GameObject bulletRR = objectManager.MakeObj("Player_Bullet_A");
                bulletRR.transform.position = transform.position + Vector3.right * 0.35f;
                
                /*
                GameObject bulletRR = Instantiate(bulletObjA, transform.position +
                Vector3.right * 0.4f, transform.rotation);
                */
                
                GameObject bulletCC = objectManager.MakeObj("Player_Bullet_B");
                bulletCC.transform.position = transform.position;

                /*
                GameObject bulletCC = Instantiate(bulletObjB, transform.position
                , transform.rotation);
                */
                
                GameObject bulletLL = objectManager.MakeObj("Player_Bullet_A");
                bulletLL.transform.position = transform.position + Vector3.left * 0.35f;
                /*
                GameObject bulletLL = Instantiate(bulletObjA, transform.position +
                Vector3.left * 0.4f, transform.rotation);
                */

                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                break;
        }
        sound.Play();

        curShotDelay = 0;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTocuhBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Enemybullet")
        {
            if (isRespawnTime)
            {
                return;
            }
            
            if (isDamaged)
            {
                return;
            }
            
            isDamaged = true;
            life--;
            gameManager.UpdateLifeIcon(life);

            StartCoroutine(DeadEffect());
            DeadSound.Play();

            if (life == 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.RespawnPlayer();
            }

            StartCoroutine(PlayoffEffect());

            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            ItemSound.Play();
            switch (item.type)
            {
                case "Power":
                    if (power == maxpower)
                    {
                        score += 500;
                    }
                    else
                    {
                        power++;
                        AddFollower();
                    }
                    break;

                case "Boom":
                    if (boom == maxboom)
                    {
                        score += 500;
                    }
                    else
                    {
                        boom++;
                        gameManager.UpdateboomIcon(boom);
                    }
                    break;
            }
            // Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }

    IEnumerator DeadEffect()
    {
        anim.SetTrigger("Dead");
        yield return new WaitForSeconds(0.9f);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTocuhBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void Boom()
    {
        if (!Input.GetButton("Fire2")) 
        {
            return;
        }

        if (isBoomTime)
        {
            return;
        }

        if (boom == 0)
        {
            return;
        }

        boom--;
        isBoomTime = true;
        gameManager.UpdateboomIcon(boom);

        // #1.Effect visible
        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 0.5f);
        BoomSound.Play();
        //#2.Remove Enemy
        //정해놓은 테그에 설정된 애들만 가져옴
        GameObject[] enemiesA = objectManager.GetPool("Enemy_a");
        GameObject[] enemiesN = objectManager.GetPool("Enemy_n");
        GameObject[] enemiesN1 = objectManager.GetPool("Enemy_N1");
        GameObject[] enemiesN2 = objectManager.GetPool("Enemy_N2");
        GameObject[] enemiesB = objectManager.GetPool("Enemy_b");
        GameObject[] enemiesS = objectManager.GetPool("Enemy_s");
        GameObject[] enemiesS1 = objectManager.GetPool("Enemy_s1");
        GameObject[] enemiesP = objectManager.GetPool("Enemy_p");
        GameObject[] enemiesR = objectManager.GetPool("Enemy_r");
        GameObject[] enemiesW = objectManager.GetPool("Enemy_w");
        GameObject[] enemiesZ = objectManager.GetPool("Enemy_z");
        GameObject[] enemiesY = objectManager.GetPool("Enemy_y");
        GameObject[] enemiesB1 = objectManager.GetPool("Boss1");

        for (int index = 0; index < enemiesA.Length; index++)
        {
            //활성화만
            if (enemiesA[index].activeSelf)
            {
                Enemy enemyLogic = enemiesA[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesN.Length; index++)
        {
            //활성화만
            if (enemiesN[index].activeSelf)
            {
                Enemy enemyLogic = enemiesN[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesN1.Length; index++)
        {
            //활성화만
            if (enemiesN1[index].activeSelf)
            {
                Enemy enemyLogic = enemiesN1[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesN2.Length; index++)
        {
            //활성화만
            if (enemiesN2[index].activeSelf)
            {
                Enemy enemyLogic = enemiesN2[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesB.Length; index++)
        {
            //활성화만
            if (enemiesB[index].activeSelf)
            {
                Enemy enemyLogic = enemiesB[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesS.Length; index++)
        {
            //활성화만
            if (enemiesS[index].activeSelf)
            {
                Enemy enemyLogic = enemiesS[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesS1.Length; index++)
        {
            //활성화만
            if (enemiesS1[index].activeSelf)
            {
                Enemy enemyLogic = enemiesS1[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesP.Length; index++)
        {
            //활성화만
            if (enemiesP[index].activeSelf)
            {
                Enemy enemyLogic = enemiesP[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesR.Length; index++)
        {
            //활성화만
            if (enemiesR[index].activeSelf)
            {
                Enemy enemyLogic = enemiesR[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesW.Length; index++)
        {
            //활성화만
            if (enemiesW[index].activeSelf)
            {
                Enemy enemyLogic = enemiesW[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesZ.Length; index++)
        {
            //활성화만
            if (enemiesZ[index].activeSelf)
            {
                Enemy enemyLogic = enemiesZ[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesY.Length; index++)
        {
            //활성화만
            if (enemiesY[index].activeSelf)
            {
                Enemy enemyLogic = enemiesY[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesB1.Length; index++)
        {
            //활성화만
            if (enemiesB1[index].activeSelf)
            {
                Enemy enemyLogic = enemiesB1[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }



        //#3.
        GameObject[] bulletsn = objectManager.GetPool("Enemybullet_n");
        GameObject[] bulletsn2 = objectManager.GetPool("Enemybullet_n2");
        GameObject[] bulletsn3 = objectManager.GetPool("EnemyBullet_n3");
        GameObject[] bulletsn4 = objectManager.GetPool("EnemyBullet_n4");
        GameObject[] bulletsn5 = objectManager.GetPool("Enemybullet_n5");
        GameObject[] bulletsn6 = objectManager.GetPool("EnemyBullet_n6");

        // GameObject[] bullets = GameObject.FindGameObjectsWithTag("Enemybullet");

        for (int index = 0; index < bulletsn.Length; index++)
        {
            //    Destroy(bullets[index]);
            bulletsn[index].SetActive(false);
        }

        for (int index = 0; index < bulletsn2.Length; index++)
        {
            //    Destroy(bullets[index]);
            bulletsn2[index].SetActive(false);
        }

        for (int index = 0; index < bulletsn3.Length; index++)
        {
            //    Destroy(bullets[index]);
            bulletsn3[index].SetActive(false);
        }

        for (int index = 0; index < bulletsn4.Length; index++)
        {
            //    Destroy(bullets[index]);
            bulletsn4[index].SetActive(false);
        }

        for (int index = 0; index < bulletsn5.Length; index++)
        {
            //    Destroy(bullets[index]);
            bulletsn5[index].SetActive(false);
        }

        for (int index = 0; index < bulletsn6.Length; index++)
        {
            //    Destroy(bullets[index]);
            bulletsn6[index].SetActive(false);
        }
    }


    void AddFollower()
    {
        if (power == 4)
        {
            followers[0].SetActive(true);
        }
        else if (power == 5)
        {
            followers[1].SetActive(true);
        }
    }

    void OffBoomEffect() 
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    IEnumerator PlayoffEffect() 
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(true);
        anim.SetTrigger("Revival");
    }


}
