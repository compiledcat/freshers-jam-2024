using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;


[Serializable]
public struct RoundData
{
    public RoundData(List<WaveData> _waves = null, List<float> _timeBetweenWaves = null)
    {
        waves = _waves ?? new List<WaveData>();
        timeBetweenWaves = _timeBetweenWaves ?? new List<float>();
    }

    //Parallel arrays - remember your fence posts (length of timeBetweenWaves should be one less than length of waves)
    public List<WaveData> waves;
    public List<float> timeBetweenWaves; //In seconds
}


[Serializable]
public struct WaveData
{
    public WaveData(int _numEnemies = 0, float _timeBetweenEnemies = 0)
    {
        numEnemies = _numEnemies;
        timeBetweenEnemies = _timeBetweenEnemies;
    }

    //public List<GameObject> enemies;
    public int numEnemies;

    public float timeBetweenEnemies; //In seconds
}



public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
    [SerializeField] private GameObject enemy_prefab;

    private List<RoundData> rounds;
    public float timeBetweenRounds; //In seconds
    public int currentRound;

    [SerializeField] private int roundsPerScene;
    [SerializeField] private CameraMovement cameraMovement;


    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        FindObjectOfType<SplineContainer>().transform.localScale = new Vector3(1, 1, 0);

        currentRound = 0;
        timeBetweenRounds = 3.0f;

        rounds = new List<RoundData>();
        
        for (int i=0; i<roundsPerScene*3; i++)
        {
            Debug.Log(i);
            int numWaves = UnityEngine.Random.Range(3, 10);
            RoundData round = new RoundData(null, null);
            for (int k = 0; k < numWaves; k++) {
                round.waves.Add(new WaveData(UnityEngine.Random.Range(5, 25), UnityEngine.Random.Range(0.25f, 3.0f)));
            }
            rounds.Add(round);
        }
        Debug.Log(rounds.Count);
        StartCoroutine(PlayGame());
    }


    Color StrengthToColor(float strength, float maxStrength)
    {
        return Color.Lerp(Color.green, Color.red, strength/maxStrength);
    }

    float StrengthToScale(float strength, float maxStrength)
    {
        // Debug.Log("");
        // Debug.Log(strength);
        // Debug.Log(maxStrength);
        // Debug.Log(Mathf.Lerp(0.5f, 2.0f, strength / maxStrength));
        // Debug.Log("");
        return Mathf.Lerp(0.5f, 2.0f, strength / maxStrength);
    }


    //Plays all rounds in game
    IEnumerator PlayGame()
    {
        //Loop through all rounds in game
        while (currentRound < rounds.Count)
        {
            Debug.Log("STARTING ROUND.");
            //Loop through all waves in round
            int currentWave = 0;
            while (currentWave < rounds[currentRound].waves.Count)
            {
                Debug.Log("Starting wave.");
                //Loop through all enemies in wave
                int currentEnemy = 0;
                while (currentEnemy < rounds[currentRound].waves[currentWave].numEnemies)
                {
                    //Debug.Log("Spawning enemy.");
                    //Instantiate(rounds[currentRound].waves[currentWave].enemies[currentEnemy]);

                    Enemy enemy = Instantiate(enemy_prefab).GetComponent<Enemy>();
                    enemy.health = UnityEngine.Random.Range(enemy.minHealth, enemy.maxHealth);
                    enemy.damage = UnityEngine.Random.Range(enemy.minHealth, enemy.maxHealth);
                    enemy.strength = enemy.health + enemy.damage * 2;
                    float maxStrength = enemy.maxHealth + enemy.maxDamage * 2;
                    enemy.GetComponent<SpriteRenderer>().color = StrengthToColor(enemy.strength, maxStrength);
                    float scale = StrengthToScale(enemy.strength, maxStrength);
                    enemy.transform.localScale = new Vector3(scale, scale, 1);

                    currentEnemy += 1;
                    yield return new WaitForSeconds(rounds[currentRound].waves[currentWave].timeBetweenEnemies);
                }
                //Debug.Log("Wave complete.");

                currentWave += 1;

                //ugly code pls help me jowsey (from ava :p)
                if (currentWave < rounds[currentRound].timeBetweenWaves.Count)
                {
                    yield return new WaitForSeconds(rounds[currentRound].timeBetweenWaves[currentWave]);
                }
                else
                {
                    yield return null;
                }
            }
            //Debug.Log("Round complete.");

            currentRound += 1;

            //Only go to next round if all enemies are dead
            yield return new WaitUntil(() => FindObjectOfType<Enemy>() == null);
            
            //ugly code pls help me jowsey (from ava :p)
            if (currentRound < rounds.Count)
            {
                //Check if camera has to advance to next scene, if so, add the time it takes for the camera to move to the delay between rounds
                if (currentRound % roundsPerScene == 0 && currentRound > 0)
                {
                    yield return new WaitForSeconds(timeBetweenRounds + cameraMovement.advancementTimeForNextScene);
                    SceneManager.LoadScene("TransitionScene");
                }
                else
                {
                    yield return new WaitForSeconds(timeBetweenRounds);
                }
            }
            else
            {
                yield return null;
            }
        }
        //Debug.Log("Game complete.");
    }
}