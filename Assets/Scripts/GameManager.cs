using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int Health = 20;

    private void Awake()
    {
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
        Health -= dmgAmount;
    }
}