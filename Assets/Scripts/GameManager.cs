using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score;
    private bool win;

    public int Score { get => score; set => score = value; }
    public bool Win { get => win; set => win = value; }

    public static GameManager instance;

    private void Awake()
    {
        // first time
        instance = this;
        if (instance == null)
            DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);
    }
}
