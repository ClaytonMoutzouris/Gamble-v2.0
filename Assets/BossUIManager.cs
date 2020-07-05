using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIManager : MonoBehaviour
{
    public static BossUIManager instance;
    public BossHealthBar bossHealthbar;
    public Text bossName;
    BossEnemy currentBoss;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void SetBoss(BossEnemy boss)
    {
        gameObject.SetActive(true);
        currentBoss = boss;
        bossHealthbar.InitHealthbar(currentBoss);
        bossHealthbar.SetHealth(currentBoss.mHealth);
        bossName.text = currentBoss.entityName;
    }

    public void ClearBoss()
    {
        currentBoss = null;                               
        bossName.text = "";
        gameObject.SetActive(false);

    }


}
