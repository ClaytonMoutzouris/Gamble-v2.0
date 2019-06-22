using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatElement : MonoBehaviour
{
    public StatType type;
    public Text statName;
    public Text statValue;
    public Button button;

    void Start()
    {
        statName.text = StatTypeMethods.GetShortName(type);
    }

    public void SetStat(Stat stat)
    {
        statValue.text = stat.GetValue().ToString();
    }
}
