using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int Stage_number;
    [SerializeField]
    private Player player;
    [SerializeField]
    private AudioSource BGM_Sound;

    [SerializeField]
    public Animator Change_ani;

    float playTime;
    bool isBattle;
    bool MeunButton;

    int Enemy_C_count = 0;
    int Enemy_D_count = 0;
    int Enemy_S_count = 0;

    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private List<int> enemyList;
    [SerializeField]
    private Transform[] enemyZones;

    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private GameObject gamePanel;
    [SerializeField]
    private GameObject GameoverPanel;
    [SerializeField]
    private GameObject GameStartPanel;
    [SerializeField]
    private GameObject ChangePanel;
    [SerializeField]
    private GameObject StatusPanel;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text best_scoreText;
    [SerializeField]
    private Text stageText;
    [SerializeField]
    private Text playTimeText;
    [SerializeField]
    private Text playerHealthText;

    [SerializeField]
    private RectTransform bossHealthGroup;
    /*
    [SerializeField]
    private RectTransform bossHealthBar;
    */
    [SerializeField]
    Image HpHar;
    [SerializeField]
    Text Hptext;

    [SerializeField]
    private Text enemyCText;
    [SerializeField]
    private Text enemyDText;
    [SerializeField]
    private Text enemySText;

    int mouse_number = 0;
    Enemy boss;
    private bool game_clear = false;

    void Awake()
    {
        isBattle = true;
        ChangePanel.SetActive(false);

        if (PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", 0);
        }

        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(true);
        }
    }

    public int GetStage_number
    {
        get { return Stage_number; }
        set { Stage_number = value; }
    }

    public bool Get_isBattle
    {
        get { return isBattle; }
        set { isBattle = value; }
    }

    public bool Get_game_clear
    {
        get { return game_clear; }
        set { game_clear = value; }
    }

    public void GameStart()
    {
        GameoverPanel.SetActive(false);
        GameStartPanel.SetActive(false);
        gamePanel.SetActive(true);
        player.gameObject.SetActive(true);

        if (player.Get_isDead)
        {
            SceneManager.LoadScene("Stage " + Stage_number);
        }
    }

    public void GameOver()
    {
        gamePanel.SetActive(false);
        GameoverPanel.SetActive(true);

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if (player.Get_score > maxScore)
        {
            best_scoreText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.Get_score);
        }
    }

    void Update()
    {
        GetInput();
        
        playTime += Time.deltaTime;

        if (mouse_number < 7) 
        {
            mouse_number++;
            StartCoroutine(InBattle());
        }

        if (Stage_number == 4) 
        {
            if (!Enemy.isBoss) 
            {
                Debug.Log("Game Clear");
            }
        }
    }

    public void OnSilderEvent(float value) 
    {
        BGM_Sound.volume = value;
    }

    public void Game_clear() 
    {
        ChangePanel.SetActive(true);
        Change_ani.SetBool("Picture_Change",true);
    }

    void LateUpdate()
    {
        //up UI
        scoreText.text = string.Format("{0:n0}", player.Get_score);
        stageText.text = "STAGE " + Stage_number;

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);

        playTimeText.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min)
            + ":" + string.Format("{0:00}", second);

        //player UI
        playerHealthText.text = player.Get_health + " / " + player.Get_MaxHealth;
        
        //boss UI
        if (Stage_number == 4 && !game_clear)
        {
            boss = GameObject.FindGameObjectWithTag("Boss").transform.GetComponentInChildren<Enemy>();
            bossHealthGroup.anchoredPosition = Vector3.down * 120;

            float HP = (float)boss.Get_curHealth;
            HpHar.fillAmount = HP / boss.Get_maxHealth;
            Hptext.text = string.Format("HP {0}/{1}",HP, boss.Get_maxHealth);
            //bossHealthBar.localScale = new Vector3((float)boss.Get_curHealth / boss.Get_maxHealth, 1, 1);
        }
        else
        {
            enemyCText.text = Enemy_C_count.ToString();
            enemyDText.text = Enemy_D_count.ToString();
            enemySText.text = Enemy_S_count.ToString();
            bossHealthGroup.anchoredPosition = Vector3.up * 200;
        }
    }

    void GetInput() 
    {
        MeunButton = Input.GetButtonUp("Cancel");
    }

    public void GameMenu_On() 
    {
        gamePanel.SetActive(false);
        Time.timeScale = 0;
        menuPanel.SetActive(true);
    }

    public void GameMenu_Off()
    {
        gamePanel.SetActive(true);
        Time.timeScale = 1;
        menuPanel.SetActive(false);
    }

    public void States_On()
    {
        StatusPanel.SetActive(true);
    }

    void GameStageStart() 
    {
        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(true);
        }
        isBattle = true;
    }

    public void StageEnd()
    {
        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(false);
        }

        isBattle = false;
    }

    IEnumerator InBattle()
    {
        if (Stage_number != 4)
        {
            int ran = Random.Range(0, 3);
            enemyList.Add(ran);

            switch (ran)
            {
                case 0:
                    Enemy_C_count++;
                    break;

                case 1:
                    Enemy_D_count++;
                    break;

                case 2:
                    Enemy_S_count++;
                    break;
            }

            int ranZone = Random.Range(0, 3);

            if (enemyZones.Length > 0) 
            {
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]],
                enemyZones[ranZone].position, enemyZones[ranZone].rotation);

                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.Get_target = player.transform;
                enemy.E_manager = this;
                enemyList.RemoveAt(0);
            }
            yield return new WaitForSeconds(4f);
        }

        while (Enemy_C_count + Enemy_D_count + Enemy_S_count > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(false);
        }
    }


    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit() 
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
            Application.OpenURL("http://google.com");
        #else
             Application.Quit();
        #endif
    }


}
