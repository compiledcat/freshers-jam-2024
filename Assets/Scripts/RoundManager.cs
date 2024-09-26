using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;


[Serializable]
public struct RoundData
{
    RoundData(List<WaveData> _waves, List<float> _timeBetweenWaves) { waves = _waves; timeBetweenWaves = _timeBetweenWaves; }

    //Parallel arrays - remember your fence posts (length of timeBetweenWaves should be one less than length of waves)
    public List<WaveData> waves;
    public List<float> timeBetweenWaves; //In seconds
}


[Serializable]
public struct WaveData
{
    WaveData(List<GameObject> _enemies, float _timeBetweenEnemies) { enemies = _enemies; timeBetweenEnemies = _timeBetweenEnemies; }

    public List<GameObject> enemies;
    public float timeBetweenEnemies; //In seconds
}



public class RoundManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy_prefab;

    public List<RoundData> rounds;
    public float timeBetweenRounds; //In seconds
    public int currentRound;


    private void Start()
    {

        FindObjectOfType<SplineContainer>().transform.localScale = new Vector3(1, 1, 0);

        currentRound = 0;
        timeBetweenRounds = 3.0f;
        
        StartCoroutine(PlayGame());
    }


    //Plays all rounds in game
    IEnumerator PlayGame()
    {
        //Loop through all rounds in game
        while (currentRound < rounds.Count)
        {
            //Debug.Log("Starting round.");
            //Loop through all waves in round
            int currentWave = 0;
            while (currentWave < rounds[currentRound].waves.Count)
            {
                //Debug.Log("Starting wave.");
                //Loop through all enemies in wave
                int currentEnemy = 0;
                while (currentEnemy < rounds[currentRound].waves[currentWave].enemies.Count)
                {
                    //Debug.Log("Spawning enemy.");
                    Instantiate(rounds[currentRound].waves[currentWave].enemies[currentEnemy]);
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
                yield return new WaitForSeconds(timeBetweenRounds);
            }
            else
            {
                yield return null;
            }
        }
        //Debug.Log("Game complete.");
    }
}