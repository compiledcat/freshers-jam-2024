using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int maxHealth = 20;
    public int health;
    [SerializeField] private SpriteRenderer queenFrog;
    [SerializeField] private Sprite idleFrogSprite;
    [SerializeField] private Sprite hurtFrogSprite;
    [SerializeField] private Sprite deadFrogSprite;

    private void Awake()
    {
        health = maxHealth;
        Instance = this;
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
            health = 0;
            queenFrog.sprite = deadFrogSprite;
        }
    }
}