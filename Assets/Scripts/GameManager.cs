using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int maxHealth = 20;
    public int health;
    public int currentLevel = 0;

    public SpriteRenderer queenFrog;
    [SerializeField] private Sprite idleFrogSprite;
    [SerializeField] private Sprite hurtFrogSprite;
    [SerializeField] private Sprite deadFrogSprite;

    public int Money;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        health = maxHealth;
        Money = 25;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale -= 1;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W))
        {
            Time.timeScale = 1;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale += 1;
        }
#endif
    }

    /// <summary>
    /// Deal damage to the player base
    /// </summary>
    public void HitBase(int dmgAmount)
    {
        health -= dmgAmount;

        if (health <= (float)maxHealth / 2 && queenFrog.sprite == idleFrogSprite)
        {
            queenFrog.sprite = hurtFrogSprite;
        }
        else if (health <= 0)
        {
            StartCoroutine(LoseGame());
        }
    }

    public IEnumerator LoseGame()
    {
        health = 0;
        queenFrog.sprite = deadFrogSprite;
        RoundManager.Instance.fadeOverTime.UnFade();
        yield return new WaitForSeconds(RoundManager.Instance.fadeOverTime.time);
        SceneManager.LoadScene("LoseScene");
    }
}