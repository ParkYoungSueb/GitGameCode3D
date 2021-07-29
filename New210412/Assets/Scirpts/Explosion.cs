using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator animl;

    void Awake()
    {
        animl = GetComponent<Animator>();
    }

    void OnEnable()
    {
        Invoke("Disable", 2f);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    public void StartExplosion(string target)
    {
        animl.SetTrigger("OnExplosion");

        switch (target)
        {
            case "A":
                transform.localScale = Vector3.one * 1f;
                break;

            case "N1":
                transform.localScale = Vector3.one * 1f;
                break;

            case "N2":
                transform.localScale = Vector3.one * 0.7f;
                break;

            case "N3":
                transform.localScale = Vector3.one * 0.7f;
                break;

            case "B":
                transform.localScale = Vector3.one * 0.7f;
                break;

            case "P":
                transform.localScale = Vector3.one * 1f;
                break;

            case "R":
                transform.localScale = Vector3.one * 0.7f;
                break;

            case "S":
                transform.localScale = Vector3.one * 0.7f;
                break;

            case "S1":
                transform.localScale = Vector3.one * 0.7f;
                break;

            case "W":
                transform.localScale = Vector3.one * 2f;
                break;

            case "Y":
                transform.localScale = Vector3.one * 2f;
                break;

            case "Z":
                transform.localScale = Vector3.one * 2f;
                break;

            case "M":
                transform.localScale = Vector3.one * 0.7f;
                break;
            
            case "B1":
                transform.localScale = Vector3.one * 3f;
                break;

            case "B2":
                transform.localScale = Vector3.one * 3f;
                break;

            case "B3":
                transform.localScale = Vector3.one * 3f;
                break;
            
        }
    }
}
