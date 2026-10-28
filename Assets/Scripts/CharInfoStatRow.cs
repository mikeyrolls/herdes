/**
 * Single stat in char info
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CharInfoStatRow : MonoBehaviour {

    [SerializeField] Image icon;
    [SerializeField] TMP_Text statName;
    [SerializeField] TMP_Text statValue;
    [SerializeField] TMP_Text statInc;

    public void SetValuesCurrBase(int currStat, int stat) {
        SetValuesCurrBase(currStat, stat, stat);
    }

    public void SetValuesCurrBase(int currStat, int stat, int stat2) {

        statValue.text = stat + "";
        if (stat2 != stat)
            statValue.text += "-" + stat2;
        
        int valueDiff = currStat - stat;

        if (valueDiff == 0) {
            statInc.text = "";
        } else if (valueDiff > 0) {     //inc
            statInc.text = "+" + valueDiff;
            statInc.color = new Color(0.21f, 0.61f, 0.08f, 1f); //roughly the same as inc/dec in icons
        } else {    //dec
            statInc.text = "" + valueDiff;
            statInc.color = new Color(0.44f, 0.18f, 0.34f, 1f);
        }
    }

}