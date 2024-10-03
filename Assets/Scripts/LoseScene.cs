using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScene : MonoBehaviour
{
    public void MainMenu()
    {
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(0);
    }
}
