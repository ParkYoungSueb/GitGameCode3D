using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    float hAxis;
    float vAxis;
    float speed;
    bool isJump;
    bool wDown;
    bool jDown;
    bool attck01;
    bool isAttack;
    bool attck02;
    bool attck03;
    bool isDamage;
    bool angry = false;
    bool isMenu = false;
    bool isstatus;
    bool isPlay;
    bool isDead;

    float health;
    float MaxHealth;
    int power;
    int defend;
    int Lev;
    int p_Exp;
    int score;

    [SerializeField]
    private AudioSource runSound;
    [SerializeField]
    private AudioSource jumpSound;
    [SerializeField]
    private AudioSource DeadSound;
    [SerializeField]
    private AudioSource Attack01sound;
    [SerializeField]
    private AudioSource Attack02sound;
    [SerializeField]
    private AudioSource Attack03sound;
    [SerializeField]
    private AudioSource damagesound;
    [SerializeField]
    private AudioSource BossSound;
    [SerializeField]
    private AudioSource Health;
    [SerializeField]
    private AudioSource LevelUp;
    [SerializeField]
    private AudioSource Clear;
    [SerializeField]
    private AudioSource Start;
    [SerializeField]
    private AudioSource dead_floor_Sound;
    [SerializeField]
    private AudioSource item_Sound;
    [SerializeField]
    private AudioSource Hp_Sound;
    [SerializeField]
    private AudioSource poison_Sound;

    [SerializeField]
    private GameManager manager;
    [SerializeField]
    private BoxCollider attack_bullet;

    Rigidbody rigid;
    Animator anim;
    Vector3 p_moveVec;

    void Awake()
    {
        speed = 7;
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        manager = GameObject.FindGameObjectWithTag("GameController").transform.GetComponent<GameManager>();
        Start.Play();
        // isPlay = true;

        if (manager.GetStage_number == 1)
        {
            health = 500;
            MaxHealth = 500;
            power = 20;
            defend = 2;
            Lev = 1;
            p_Exp = 0;
            score = 0;
        }
        else
        {
            health = PlayerPrefs.GetFloat("s_health");
            MaxHealth = PlayerPrefs.GetFloat("s_MaxHealth");
            power = PlayerPrefs.GetInt("s_power");
            defend = PlayerPrefs.GetInt("s_defend");
            Lev = PlayerPrefs.GetInt("s_Lev");
            p_Exp = PlayerPrefs.GetInt("s_p_Exp");
            score = PlayerPrefs.GetInt("s_score");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        P_Move();
        Turn();
        Jump();
        StartCoroutine(attack_s());
        ability();
        escape();
        m_status();
        StartCoroutine(OnDie());
    }

    public float Get_health
    {
        get { return health; }
        set { health = value; }
    }

    public int Get_score
    {
        get { return score; }
        set { score = value; }
    }

    public int Get_power
    {
        get { return power; }
        set { power = value; }
    }

    public int Get_defend
    {
        get { return defend; }
        set { defend = value; }
    }


    public int Get_Lev
    {
        get { return Lev; }
        set { Lev = value; }
    }

    public int Get_p_Exp
    {
        get { return p_Exp; }
        set { p_Exp = value; }
    }

    public float Get_MaxHealth
    {
        get { return MaxHealth; }
        set { MaxHealth = value; }
    }

    public bool Get_isDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    void FixedUpdate()
    {
        FreezeRoation();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButtonDown("Jump");
        attck01 = Input.GetButtonUp("Attack01");
        attck02 = Input.GetButtonUp("Attack02");
        attck03 = Input.GetButtonUp("Attack03");
        isMenu = Input.GetButtonDown("escape");
        isstatus = Input.GetButtonDown("status");
    }

    void escape()
    {
        if (isMenu == true && isPlay == true)
        {
            isPlay = false;
            manager.GameMenu_On();
            return;
        }

        /*一時止まる*/
        if (isMenu == true && isPlay == false)
        {
            isPlay = true;
            manager.GameMenu_Off();
            return;
        }
    }

    void m_status()
    {
        if (isstatus) 
        {
            isstatus = false;
            manager.States_On();
        }
    }


    void P_Move()
    {
        if (!isDead)
        {
            p_moveVec = new Vector3(hAxis, 0, vAxis).normalized;

            if (isDead)
            {
                p_moveVec = Vector3.zero;
            }

            //삼합연산자
            transform.position += p_moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
            runSound.Play();
            anim.SetBool("p_run", p_moveVec != Vector3.zero);
            anim.SetBool("p_walk", wDown);
        }
    }

    void Turn()
    {
        transform.LookAt(transform.position + p_moveVec);
    }

    void Jump()
    {
        if (jDown && !isJump && p_moveVec == Vector3.zero)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("p_jump", true);
            jumpSound.Play();
            isJump = true;
        }
    }

    IEnumerator attack_s()
    {
        yield return new WaitForSeconds(1f);

        if (!isAttack && attck01 && !isJump && !(p_moveVec != Vector3.zero))
        {
            Attack01sound.Play();
            attack_bullet.enabled = true;
            anim.SetBool("p_attack01", true);
            isAttack = true;
            yield return new WaitForSeconds(0.4f);
            attack_bullet.enabled = false;
            yield return new WaitForSeconds(0.2f);
            anim.SetBool("p_attack01", false);

            isAttack = false;
        }

        if (!isAttack && attck02 && !isJump && !(p_moveVec != Vector3.zero))
        {
            Attack02sound.Play();
            anim.SetBool("p_attack02", true);
            attack_bullet.enabled = true;

            isAttack = true;
            yield return new WaitForSeconds(1.4f);
            attack_bullet.enabled = false;
            anim.SetBool("p_attack02", false);
            isAttack = false;
        }

        if (!isAttack && attck03 && !isJump && !(p_moveVec != Vector3.zero))
        {
            Attack03sound.Play();
            attack_bullet.enabled = true;
            anim.SetBool("p_attack03", true);
            isAttack = true;
            yield return new WaitForSeconds(1.8f);
            attack_bullet.enabled = false;
            anim.SetBool("p_attack03", false);
            isAttack = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.layer == 10)
        {
            isJump = false;
            anim.SetBool("p_jump", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "item") 
        {
            Item item = other.GetComponent<Item>();

            switch (item.type) 
            {
                case Item.Type.Hp:
                    Hp_Sound.Play();
                    if (health+60 >= MaxHealth) 
                    {
                        health = MaxHealth;
                    }
                    else 
                    {
                        health += 60;
                    }
                    break;

                case Item.Type.MaxHp:
                    Hp_Sound.Play();
                    health = MaxHealth;
                    break;

                case Item.Type.poison:
                    poison_Sound.Play();
                    health -= 90;
                    break;

                case Item.Type.Coin:
                    item_Sound.Play();
                    score += 100;
                    break;
            }
        }

        if (other.tag == "dead_floor")
        {
            StartCoroutine(dead_floor_Time());
        }
        else if (other.tag == "EnemyBullet" && !isDead || other.tag == "Missile" && !isDead)
        {
            StartCoroutine(OnDamage());
        }
        else if (other.tag == "end_floor")
        {
            StartCoroutine(Next_Stage());
        }
    }

    void FreezeRoation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    IEnumerator OnDamage()
    {
        isDamage = true;

        if (isDamage)
        {
            anim.SetBool("p_damage", true);
            damagesound.Play();
        }

        yield return new WaitForSeconds(0.5f);
        isDamage = false;
        anim.SetBool("p_damage", false);

        if (!angry && health < MaxHealth / 2)
        {
            Health.Play();
            angry = true;
        }
    }

    IEnumerator OnDie()
    {
        if (health <= 0 && !isDead)
        {
            DeadSound.Play();
            anim.SetBool("p_dead", true);
            isDead = true;
            // PlayerPrefs.SetInt("MaxScore", score);
            yield return new WaitForSeconds(4f);
            manager.GameOver();
        }
    }

    void ability()
    {
        if (p_Exp >= 350 + (Lev * 10))
        {
            LevelUp.Play();
            p_Exp -= 350;
            power += 2;
            defend++;
            Lev++;
            MaxHealth += 15;
            health += 500;
            angry = false;
        }
    }

    public float get_defend()
    {
        return defend;
    }

    IEnumerator dead_floor_Time()
    {
        dead_floor_Sound.Play();
        ability_save();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("Stage " + manager.GetStage_number);
    }

    IEnumerator Next_Stage()
    {
        anim.SetBool("p_win", true);

        Clear.Play();

        manager.Game_clear();

        if (manager.GetStage_number == 4)
        {
            //Game End
            manager.Quit();
        }
        else if (manager.GetStage_number == 3)
        {
            BossSound.Play();
        }

        yield return new WaitForSeconds(3.5f);
        anim.SetBool("p_win", false);
        ability_save();

        //next Stage
        int next_number = manager.GetStage_number + 1;

        SceneManager.LoadScene("Stage " + next_number);
    }

    void ability_save() 
    {
        PlayerPrefs.SetInt("s_power", power);
        PlayerPrefs.SetInt("s_defend", defend);
        PlayerPrefs.SetInt("s_Lev", Lev);
        PlayerPrefs.SetInt("s_p_Exp", p_Exp);
        PlayerPrefs.SetFloat("s_MaxHealth", MaxHealth);
        PlayerPrefs.SetFloat("s_health", health);
        PlayerPrefs.SetInt("s_score", score);
    }

}
