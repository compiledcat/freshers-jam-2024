using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedControls : MonoBehaviour
{
    int speed = 1;
    [SerializeField] private List<Sprite> speedIcons;
    [SerializeField] private Image speedIcon;

    private void Awake()
    {
        Time.timeScale = speed;
    }

    public void Change()
    {
        speed++;
        if (speed > speedIcons.Count)
        {
            speed = 1;
        }
        Time.timeScale = speed;
        speedIcon.sprite = speedIcons[speed - 1];
    }
}
