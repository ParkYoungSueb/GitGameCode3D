using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    [SerializeField]
    [TextArea(3, 5)]
    string format = "Player Lv{0} \nHP:{1}/{2} \nPower:{3} \nDef:{4} \nExp:{5}";

    Player player;

    void Awake()
    {
        // player = GetComponent<Player>();
        player = GameObject.Find("Player").transform.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        GetComponent<Text>().text = string.Format(
            format,
            player.Get_Lev,
            player.Get_health,
            player.Get_MaxHealth,
            player.Get_power,
            player.Get_defend,
            player.Get_p_Exp);
    }
}
