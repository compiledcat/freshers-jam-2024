using PrimeTween;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class WorldMap : MonoBehaviour
{
    [System.Serializable]
    public struct Positions
    {
        [Required]
        public Transform position;
        public Vector2 maxSize;
    }

    [SerializeField] Camera cam;
    [SerializeField] GameObject congrats, ending, returnBtn, frog;
    [SerializeField] List<Positions> scenePositions;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private int numFrogs;


    private void Awake()
    {
        if (GameManager.Instance.currentLevel == 2)
        {
            StartCoroutine(PlayEnding());
            return;
        }

        LerpCam(GameManager.Instance.currentLevel + 1, GameManager.Instance.currentLevel + 2);
        GameManager.Instance.currentLevel++;
    }

    private IEnumerator PlayEnding()
    {
        LerpCam(GameManager.Instance.currentLevel + 1, 0);
        yield return new WaitForSeconds(Vector3.Distance(scenePositions[GameManager.Instance.currentLevel + 1].position.position, scenePositions[0].position.position) / cameraSpeed);
        float timeToWait = 4f;
        for (int i = 0; i < numFrogs; i++)
        {
            GameObject newFrog = Instantiate(frog, new Vector3(Random.Range(-scenePositions[0].maxSize.x, scenePositions[0].maxSize.x), Random.Range(-scenePositions[0].maxSize.y, scenePositions[0].maxSize.y), -.1f), Quaternion.Euler(0, 0, Random.Range(-10, 10)));
            newFrog.transform.localScale *= Random.Range(.25f, .4f);
            yield return new WaitForSeconds(timeToWait);
            timeToWait *= .8f;
        }
        yield return new WaitForSeconds(.5f);
        ending.SetActive(true);
        yield return new WaitForSeconds(10f);
        congrats.SetActive(true);
        Tween.Scale(returnBtn.transform, .76f, .84f, 4, Ease.Linear, cycles: -1, cycleMode: CycleMode.Rewind);
        Tween.LocalRotation(returnBtn.transform, Vector3.forward * -10, Vector3.forward * 10, 4, Ease.Linear, cycles: -1, cycleMode: CycleMode.Rewind);
    }

    public void MainMenu()
    {
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(0);
    }

    // lerps the camera from start to end (where each represents a boundary of the scene) with t as the lerp value
    private void LerpCam(int start, int end, bool loadScene = false)
    {
        //float normal_aspect = 1920 / 1080f;
        /*if (cam.aspect > normal_aspect) // wider
        {
            cam.orthographicSize = Mathf.Lerp(scenePositions[start].maxSize.x, scenePositions[end].maxSize.x, t);
            return;
        }*/
        float advancementTimeForNextScene = Vector3.Distance(scenePositions[start].position.position, scenePositions[end].position.position) / cameraSpeed;
        //cam.orthographicSize = Mathf.Lerp(scenePositions[start].maxSize.y, scenePositions[end].maxSize.y, t);
        cam.transform.position = scenePositions[start].position.position;
        Sequence.Create()
            .Group(Tween.Position(cam.transform, scenePositions[start].position.position - Vector3.forward * 10, scenePositions[0].position.position - Vector3.forward * 10, advancementTimeForNextScene, ease: Ease.InOutCubic))
            .Group(Tween.CameraOrthographicSize(cam, scenePositions[start].maxSize.y, scenePositions[0].maxSize.y, advancementTimeForNextScene, ease: Ease.InOutCubic))
            .ChainDelay(1f)
            .Group(Tween.Position(cam.transform, scenePositions[end].position.position - Vector3.forward * 10, advancementTimeForNextScene, ease: Ease.InOutCubic))
            .Group(Tween.CameraOrthographicSize(cam, scenePositions[end].maxSize.y, advancementTimeForNextScene, ease: Ease.InOutCubic))
            .OnComplete(() => { if (loadScene) { SceneManager.LoadScene("Level" + end.ToString()); } });
            ;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var pos in scenePositions)
        {
            if (pos.position == null) { continue; }
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(pos.position.position, new Vector3(pos.maxSize.x * 2, pos.maxSize.y * 2, 1));
        }
    }
}
