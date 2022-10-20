using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI powerUpsText;

    public void SetTimeText(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;

        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");


    }

    public void SetLiveText(int lives)
    {
        livesText.text = "Lives: " + lives.ToString();
    }

    public void SetPowerUpsText(int count)
    {
        powerUpsText.text = "Power Ups: " + count.ToString();
    

    }


}
