using PrimeTween;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] GameObject ending;
    [SerializeField] List<Positions> scenePositions;
    [SerializeField] private float cameraSpeed;


    private void Awake()
    {
        if (GameManager.Instance.currentLevel == 2)
        {
            ending.SetActive(true);
            //Tween.Scale(ending.transform, Vector3.one * .8f, 1f, ease: Ease.InOutCubic);
            return;
        }

        LerpCam(GameManager.Instance.currentLevel + 1, GameManager.Instance.currentLevel + 2);
        GameManager.Instance.currentLevel++;
    }

    public void MainMenu()
    {
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(0);
    }

    // lerps the camera from start to end (where each represents a boundary of the scene) with t as the lerp value
    private void LerpCam(int start, int end)
    {
        //float normal_aspect = 1920 / 1080f;
        /*if (cam.aspect > normal_aspect) // wider
        {
            cam.orthographicSize = Mathf.Lerp(scenePositions[start].maxSize.x, scenePositions[end].maxSize.x, t);
            return;
        }*/
        float advancementTimeForNextScene = Vector3.Distance(scenePositions[start].position.position, scenePositions[end].position.position) / cameraSpeed;
        //cam.orthographicSize = Mathf.Lerp(scenePositions[start].maxSize.y, scenePositions[end].maxSize.y, t);
        transform.position = scenePositions[start].position.position;
        Sequence.Create()
            .Group(Tween.Position(cam.transform, scenePositions[start].position.position - Vector3.forward * 10, scenePositions[0].position.position - Vector3.forward * 10, advancementTimeForNextScene, ease: Ease.InOutCubic))
            .Group(Tween.CameraOrthographicSize(cam, scenePositions[start].maxSize.y, scenePositions[0].maxSize.y, advancementTimeForNextScene, ease: Ease.InOutCubic))
            .ChainDelay(1f)
            .Group(Tween.Position(cam.transform, scenePositions[end].position.position - Vector3.forward * 10, advancementTimeForNextScene, ease: Ease.InOutCubic))
            .Group(Tween.CameraOrthographicSize(cam, scenePositions[end].maxSize.y, advancementTimeForNextScene, ease: Ease.InOutCubic))
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
