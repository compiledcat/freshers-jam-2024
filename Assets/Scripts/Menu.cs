using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button[] buttons;

    public void ShowNext(int index)
    {
        switch (index)
        {
            case 4:
                SceneManager.LoadScene(1);
                break;
            default:
                buttons[index].gameObject.SetActive(true);
                break;
        }
    }
}
