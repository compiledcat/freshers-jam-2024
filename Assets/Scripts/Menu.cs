using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] Button quitBtn;


#if UNITY_WEBGL
    private void Awake()
    {
        Destroy(quitBtn.gameObject);
    }
#endif

    public void Quit()
    {
       Application.Quit();
    }

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
