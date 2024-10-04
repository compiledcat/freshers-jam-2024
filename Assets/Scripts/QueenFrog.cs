using UnityEngine;

public class QueenFrog : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.queenFrog = GetComponent<SpriteRenderer>();
    }
}