using PrimeTween;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> scenePositions; //Exclude the first scene
    private int currentScene;

    [SerializeField] private float cameraSpeed;
    [HideInInspector] public float advancementTimeForNextScene; //The time it will take for the camera to move to the next scene, used to pause round cooldown

    public void AdvanceScene()
    {
        if (currentScene == scenePositions.Count) { return; }
        advancementTimeForNextScene = Vector3.Distance(transform.position, scenePositions[currentScene].position) / cameraSpeed;
        Tween.Position(transform, scenePositions[currentScene].position, advancementTimeForNextScene, ease: Ease.InOutCubic);
        currentScene++;
    }
}
