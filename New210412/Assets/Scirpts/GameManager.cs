using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int stage;
    [SerializeField]
    private string[] enemyObjs;
    [SerializeField]
    private Transform[] spawnPoints; //위치
    [SerializeField]
    private Transform playerPos;

    [SerializeField]
    private Animator stageAnim;
    [SerializeField]
    private GameObject stageStart;
    [SerializeField]
    private Animator clearAnim;
    [SerializeField]
    private Animator fadeAnim;

    //적 딜레이 변수
    [SerializeField]
    private float nextSpawnDelay;
    [SerializeField]
    private float curSpawnDelay;

    [SerializeField]
    private GameObject Player;

    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    [SerializeField]
    private GameObject FadeBlack;

    public ObjectManager objectManager;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    public AudioSource EnemyDeadSound;
    [SerializeField]
    private AudioSource ButtonSound;
    [SerializeField]
    private AudioSource GameOverSound;
    [SerializeField]
    private AudioSource GameReStartSound;
    [SerializeField]
    private AudioSource ClearSound;
    [SerializeField]
    private AudioSource BossSound;
    [SerializeField]
    private AudioSource BGMSound;

    void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "Enemy_a","Enemy_n", "Enemy_b", "Enemy_N1", "Enemy_N2",
        "Enemy_p","Enemy_r","Enemy_s","Enemy_s1","Enemy_w","Enemy_y","Enemy_z","Meteor",
            "Boss1","Boss2","Boss3"};

        StageStrat();
    }

    void Update()
    {
        //항상 흐르는 시간
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > nextSpawnDelay && !spawnEnd)
        {
            //생성후 딜레이 0으로 초기화
            SpawnEnemy();
            curSpawnDelay = 0;
        }

        //#.UI Score Update
        Player playerLogic = Player.GetComponent<Player>();

        //{0:n0} 3자리마다 쉼표
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    public void StageStrat()
    {
        FadeBlack.SetActive(true);
        stageStart.SetActive(true);
        //#.Stage UI Load
        ClearSound.Stop();
        GameReStartSound.Play();
        BGMSound.Play();
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<Text>().text = "Stage " + stage + "\nStart!";
        clearAnim.GetComponent<Text>().text = "Stage " + stage + "\nClear!!";

        //#.Fade In
        fadeAnim.SetTrigger("In");
        Invoke("StageStartfalse", 2.1f);
        //#.Enemy Spawn File Read
        ReadSpawnFile();
    }

    void StageStartfalse() 
    {
        FadeBlack.SetActive(false);
        stageStart.SetActive(false);
    }


    public void StageEnd()
    {
        //#.Clear UI Load
        clearAnim.SetTrigger("On");
        // GameReStartSound.Play();
        //#.Fade Out
        fadeAnim.SetTrigger("Out");
        
        //#.Stage Increament
        BossSound.Stop();
        ClearSound.Play();
        //#.Player Repos
        Player.transform.position = playerPos.position;
        stage++;
        if (stage > 3)
        {
            Invoke("GameOver", 4);
        }
        else
        {
            Invoke("StageStrat", 3);
        }
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;

        switch (spawnList[spawnIndex].type)
        {
            case "A":
                enemyIndex = 0;
                break;
            case "N":
                enemyIndex = 1;
                break;
            case "B":
                enemyIndex = 2;
                break;
            case "N1":
                enemyIndex = 3;
                break;
            case "N2":
                enemyIndex = 4;
                break;
            case "P":
                enemyIndex = 5;
                break;
            case "R":
                enemyIndex = 6;
                break;
            case "S":
                enemyIndex = 7;
                break;
            case "S1":
                enemyIndex = 8;
                break;
            case "W":
                enemyIndex = 9;
                break;
            case "Y":
                enemyIndex = 10;
                break;
            case "Z":
                enemyIndex = 11;
                break;
            case "M":
                enemyIndex = 12;
                break;
            case "B1":
                enemyIndex = 13;
                break;
            case "B2":
                enemyIndex = 14;
                break;
            case "B3":
                enemyIndex = 15;
                break;
        }

        int enemeyPoint = spawnList[spawnIndex].point;

        //생성
        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);

        if (enemyIndex > 12) 
        {
            BGMSound.Stop();
            BossSound.Play();
        }

        enemy.transform.position = spawnPoints[enemeyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();

        enemyLogic.gamemanager = this;
        enemyLogic.Player = Player;
        enemyLogic.objectManager = objectManager;

        if (enemeyPoint == 5 || enemeyPoint == 6)
        {
            //돌림
            enemy.transform.Rotate(Vector3.back * 90);

            //Right Spawn
            rigid.velocity = new Vector2(enemyLogic.Speed * (-1),
                -1);
        }
        else if (enemeyPoint == 7 || enemeyPoint == 8)
        {
            enemy.transform.Rotate(Vector3.forward * 90);

            //Left Spawn
            rigid.velocity = new Vector2(enemyLogic.Speed, -1);
        }
        else
        {
            //Front Spawn
            rigid.velocity = new Vector2(0, enemyLogic.Speed * (-1));
        }

        //#.리스폰 인덱스 증가
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        //#. 다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    void ReadSpawnFile()
    {
        //#1. 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //#2.리스폰 파일 읽기
        // as TextAsset 텍스트파일인이 검증
        TextAsset textFile = Resources.Load("Stage" + stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);
        
        while (stringReader != null)
        {
            string line = stringReader.ReadLine();

            if (line == null)
            {
                break;
            }

            //#.리스폰 데이터 생성
            string[] array = line.Split(',');
            Spawn spawnData = new Spawn();

            float.TryParse(array[0], out spawnData.delay);
            spawnData.type = array[1]; //B1

            int.TryParse(array[2], out spawnData.point);
            spawnList.Add(spawnData);
            // break;
        }
        //#.텍스트파일 닫기
        stringReader.Close();

        //#.첫번째 스폰 딜레이 적용
        nextSpawnDelay = spawnList[0].delay;
    }

    public void UpdateLifeIcon(int life)
    {
        // #.UI Life Init Disable
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        // #.UI Life Active
        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateboomIcon(int boom)
    {
        // #.UI boom Init Disable
        for (int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }

        // #.UI boom Active
        for (int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    //부활
    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
    }

    //부활
    void RespawnPlayerExe()
    {
        Player.transform.position = Vector3.down * 4.2f;
        Player.SetActive(true);

        Player playerLogic = Player.GetComponent<Player>();

        playerLogic.isDamaged = false;
    }

    public void GameRetry()
    {
        // GameReStartSound.Play();
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        GameOverSound.Play();
        gameOverSet.SetActive(true);
    }

    public void CallExplosion(Vector3 pos, string type)
    {

        GameObject explosion = objectManager.MakeObj("Explosion");

        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }

    public void buttonSound() 
    {
        ButtonSound.Play();
    }


}
