using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyAPrefab;
    public GameObject enemyNPrefab;
    public GameObject enemyN1Prefab;
    public GameObject enemyN2Prefab;
    public GameObject enemyBPrefab;
    public GameObject enemySPrefab;
    public GameObject enemyS1Prefab;
    public GameObject enemyPPrefab;
    public GameObject enemyRPrefab;
    public GameObject enemyWPrefab;
    public GameObject enemyZPrefab;
    public GameObject enemyYPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemyBoss1Prefab;
    public GameObject enemyBoss2Prefab;
    public GameObject enemyBoss3Prefab;

    public GameObject ItemPowerPrefab;
    public GameObject ItemBoomPrefab;
    public GameObject bulletPlayerAPrefab;
    public GameObject bulletPlayerBPrefab;
    public GameObject bulletFollowerPrefab;
    public GameObject bulletEnemyAPrefab;
    public GameObject bulletEnemyBPrefab;
    public GameObject bulletEnemyCPrefab;
    public GameObject bulletBossAPrefab;
    public GameObject bulletBossBPrefab;
    public GameObject bulletBossCPrefab;
    public GameObject explosionPerfabe;

    GameObject[] enemyA;
    GameObject[] enemyN;
    GameObject[] enemyN1;
    GameObject[] enemyN2;
    GameObject[] enemyB;
    GameObject[] enemyS;
    GameObject[] enemyS1;
    GameObject[] enemyP;
    GameObject[] enemyR;
    GameObject[] enemyW;
    GameObject[] enemyZ;
    GameObject[] enemyY;
    GameObject[] enemyM;
    GameObject[] enemyBoss1;
    GameObject[] enemyBoss2;
    GameObject[] enemyBoss3;

    GameObject[] ItemPower;
    GameObject[] ItemBoom;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletFollower;

    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;
    GameObject[] bulletEnemyC;

    GameObject[] bulletBossA;
    GameObject[] bulletBossB;
    GameObject[] bulletBossC;

    GameObject[] targetPool;

    GameObject[] explosions;


    void Awake()
    {
        enemyA = new GameObject[10];
        enemyN = new GameObject[6];
        enemyN1 = new GameObject[7];
        enemyN2 = new GameObject[4];
        enemyB = new GameObject[6];
        enemyS = new GameObject[5];
        enemyS1 = new GameObject[2];
        enemyP = new GameObject[1];
        enemyR = new GameObject[6];
        enemyW = new GameObject[7];
        enemyZ = new GameObject[9];
        enemyY = new GameObject[2];
        enemyM = new GameObject[1];
        enemyBoss1 = new GameObject[1];
        enemyBoss2 = new GameObject[1];
        enemyBoss3 = new GameObject[1];

        ItemPower = new GameObject[10];
        ItemBoom = new GameObject[10];

        bulletPlayerA = new GameObject[100];
        bulletPlayerB = new GameObject[100];
        bulletFollower = new GameObject[100];
        bulletEnemyA = new GameObject[100];
        bulletEnemyB = new GameObject[100];
        bulletEnemyC = new GameObject[100];
        bulletBossA = new GameObject[50];
        bulletBossB = new GameObject[1000];
        bulletBossC = new GameObject[1000];
        explosions = new GameObject[20];

        Generate();
    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "Enemy_a":
                targetPool = enemyA;
                break;

            case "Enemy_n":
                targetPool = enemyN;
                break;

            case "Enemy_b":
                targetPool = enemyB;
                break;

            case "Enemy_N1":
                targetPool = enemyN1;
                break;

            case "Enemy_N2":
                targetPool = enemyN2;
                break;

            case "Enemy_p":
                targetPool = enemyP;
                break;

            case "Enemy_r":
                targetPool = enemyR;
                break;

            case "Enemy_s":
                targetPool = enemyS;
                break;

            case "Enemy_s1":
                targetPool = enemyS1;
                break;

            case "Enemy_w":
                targetPool = enemyW;
                break;

            case "Enemy_y":
                targetPool = enemyY;
                break;

            case "Enemy_z":
                targetPool = enemyZ;
                break;

            case "Meteor":
                targetPool = enemyM;
                break;

            case "Boss1":
                targetPool = enemyBoss1;
                break;

            case "Boss2":
                targetPool = enemyBoss2;
                break;

            case "Boss3":
                targetPool = enemyBoss3;
                break;

            case "Item_Power":
                targetPool = ItemPower;
                break;

            case "Item_Boom":
                targetPool = ItemBoom;
                break;

            case "Player_Bullet_A":
                targetPool = bulletPlayerA;
                break;

            case "Player_Bullet_B":
                targetPool = bulletPlayerB;
                break;

            case "Follower_Bullet":
                targetPool = bulletFollower;
                break;

            case "Enemybullet_n":
                targetPool = bulletEnemyA;
                break;

            case "Enemybullet_n2":
                targetPool = bulletEnemyB;
                break;

            case "Enemybullet_n3":
                targetPool = bulletEnemyC;
                break;

            case "EnemyBullet_n4":
                targetPool = bulletBossA;
                break;

            case "EnemyBullet_n5":
                targetPool = bulletBossB;
                break;

            case "EnemyBullet_n6":
                targetPool = bulletBossC;
                break;

            case "Explosion":
                targetPool = explosions;
                break;
        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            //비활성화된 오브젝트에 접근하여 활성화 후,반환
            if (!targetPool[index].activeSelf)
            {
                /*
                try
                {
                    targetPool[index].SetActive(true);
                    return targetPool[index];
                }
                catch (NullReferenceException ex)
                {
                    Debug.Log(ex);
                }
                */
                
                    targetPool[index].SetActive(true);
                    return targetPool[index];
            }
        }
        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "Enemy_a":
                targetPool = enemyA;
                break;

            case "Enemy_n":
                targetPool = enemyN;
                break;

            case "Enemy_N1":
                targetPool = enemyN1;
                break;

            case "Enemy_N2":
                targetPool = enemyN2;
                break;

            case "Enemy_b":
                targetPool = enemyB;
                break;

            case "Enemy_s":
                targetPool = enemyS;
                break;

            case "Enemy_s1":
                targetPool = enemyS1;
                break;

            case "Enemy_p":
                targetPool = enemyP;
                break;

            case "Enemy_r":
                targetPool = enemyR;
                break;

            case "Enemy_w":
                targetPool = enemyW;
                break;

            case "Enemy_z":
                targetPool = enemyZ;
                break;

            case "Enemy_y":
                targetPool = enemyY;
                break;

            case "Meteor":
                targetPool = enemyM;
                break;

            case "Boss1":
                targetPool = enemyBoss1;
                break;

            case "Boss2":
                targetPool = enemyBoss2;
                break;

            case "Boss3":
                targetPool = enemyBoss3;
                break;

            case "Item_Power":
                targetPool = ItemPower;
                break;

            case "Item_Boom":
                targetPool = ItemBoom;
                break;

            case "Player_Bullet_A":
                targetPool = bulletPlayerA;
                break;

            case "Player_Bullet_B":
                targetPool = bulletPlayerB;
                break;

            case "Follower_Bullet":
                targetPool = bulletFollower;
                break;

            case "Enemybullet_n":
                targetPool = bulletEnemyA;
                break;

            case "Enemybullet_n2":
                targetPool = bulletEnemyB;
                break;
                
            case "EnemyBullet_n3":
                targetPool = bulletEnemyC;
                break;

            case "EnemyBullet_n4":
                targetPool = bulletBossA;
                break;

            case "Enemybullet_n5":
                targetPool = bulletBossB;
                break;

            case "EnemyBullet_n6":
                targetPool = bulletBossC;
                break;

            case "Explosion":
                targetPool = explosions;
                break;
        }
        return targetPool;
    }

    void Generate()
    {

        //Enemy
        for (int index = 0; index < enemyA.Length; index++)
        {
            enemyA[index] = Instantiate(enemyAPrefab);
            enemyA[index].SetActive(false);
        }

        for (int index = 0; index < enemyN.Length; index++)
        {
            enemyN[index] = Instantiate(enemyNPrefab);
            enemyN[index].SetActive(false);
        }

        for (int index = 0; index < enemyN1.Length; index++)
        {
            enemyN1[index] = Instantiate(enemyN1Prefab);
            enemyN1[index].SetActive(false);
        }

         for (int index = 0; index < enemyN2.Length; index++)
        {
            enemyN2[index] = Instantiate(enemyN2Prefab);
            enemyN2[index].SetActive(false);
        }

        for (int index = 0; index < enemyB.Length; index++)
        {
            enemyB[index] = Instantiate(enemyBPrefab);
            enemyB[index].SetActive(false);
        }

        for (int index = 0; index < enemyS.Length; index++)
        {
            enemyS[index] = Instantiate(enemySPrefab);
            enemyS[index].SetActive(false);
        }

        for (int index = 0; index < enemyS1.Length; index++)
        {
            enemyS1[index] = Instantiate(enemyS1Prefab);
            enemyS1[index].SetActive(false);
        }

        for (int index = 0; index < enemyP.Length; index++)
        {
            enemyP[index] = Instantiate(enemyPPrefab);
            enemyP[index].SetActive(false);
        }

        for (int index = 0; index < enemyR.Length; index++)
        {
            enemyR[index] = Instantiate(enemyRPrefab);
            enemyR[index].SetActive(false);
        }

        for (int index = 0; index < enemyW.Length; index++)
        {
            enemyW[index] = Instantiate(enemyWPrefab);
            enemyW[index].SetActive(false);
        }

        for (int index = 0; index < enemyZ.Length; index++)
        {
            enemyZ[index] = Instantiate(enemyZPrefab);
            enemyZ[index].SetActive(false);
        }

        for (int index = 0; index < enemyY.Length; index++)
        {
            enemyY[index] = Instantiate(enemyYPrefab);
            enemyY[index].SetActive(false);
        }

        for (int index = 0; index < enemyS.Length; index++)
        {
            enemyS[index] = Instantiate(enemySPrefab);
            enemyS[index].SetActive(false);
        }

        for (int index = 0; index < enemyM.Length; index++)
        {
            enemyM[index] = Instantiate(enemyMPrefab);
            enemyM[index].SetActive(false);
        }

        for (int index = 0; index < enemyBoss1.Length; index++)
        {
            enemyBoss1[index] = Instantiate(enemyBoss1Prefab);
            enemyBoss1[index].SetActive(false);
        }

        for (int index = 0; index < enemyBoss2.Length; index++)
        {
            enemyBoss2[index] = Instantiate(enemyBoss2Prefab);
            enemyBoss2[index].SetActive(false);
        }

         for (int index = 0; index < enemyBoss3.Length; index++)
        {
            enemyBoss3[index] = Instantiate(enemyBoss3Prefab);
            enemyBoss3[index].SetActive(false);
        }

        for (int index = 0; index < ItemPower.Length; index++)
        {
            ItemPower[index] = Instantiate(ItemPowerPrefab);
            ItemPower[index].SetActive(false);
        }

        for (int index = 0; index < ItemBoom.Length; index++)
        {
            ItemBoom[index] = Instantiate(ItemBoomPrefab);
            ItemBoom[index].SetActive(false);
        }

        //Bullet
        for (int index = 0; index < bulletPlayerA.Length; index++)
        {
            bulletPlayerA[index] = Instantiate(bulletPlayerAPrefab);
            bulletPlayerA[index].SetActive(false);
        }

        for (int index = 0; index < bulletPlayerB.Length; index++)
        {
            bulletPlayerB[index] = Instantiate(bulletPlayerBPrefab);
            bulletPlayerB[index].SetActive(false);
        }

        for (int index = 0; index < bulletFollower.Length; index++)
        {
            bulletFollower[index] = Instantiate(bulletFollowerPrefab);
            bulletFollower[index].SetActive(false);
        }

        for (int index = 0; index < bulletEnemyA.Length; index++)
        {
            bulletEnemyA[index] = Instantiate(bulletEnemyAPrefab);
            bulletEnemyA[index].SetActive(false);
        }

        for (int index = 0; index < bulletEnemyB.Length; index++)
        {
            bulletEnemyB[index] = Instantiate(bulletEnemyBPrefab);
            bulletEnemyB[index].SetActive(false);
        }

        for (int index = 0; index < bulletEnemyC.Length; index++)
        {
            bulletEnemyC[index] = Instantiate(bulletEnemyCPrefab);
            bulletEnemyC[index].SetActive(false);
        }
        
        for (int index = 0; index < bulletBossA.Length; index++)
        {
            bulletBossA[index] = Instantiate(bulletBossAPrefab);
            bulletBossA[index].SetActive(false);
        }

        for (int index = 0; index < bulletBossB.Length; index++)
        {
            bulletBossB[index] = Instantiate(bulletBossBPrefab);
            bulletBossB[index].SetActive(false);
        }

        for (int index = 0; index < bulletBossC.Length; index++)
        {
            bulletBossC[index] = Instantiate(bulletBossCPrefab);
            bulletBossC[index].SetActive(false);
        }
        
        for (int index = 0; index < explosions.Length; index++)
        {
            explosions[index] = Instantiate(explosionPerfabe);
            explosions[index].SetActive(false);
        }
    }

}
