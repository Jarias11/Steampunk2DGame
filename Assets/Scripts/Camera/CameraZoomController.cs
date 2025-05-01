using System;
using NUnit.Framework;
using Unity.Cinemachine;
using UnityEngine;

public class CameraZoomController : MonoBehaviour {
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private float zoomMultiplier = 1.15f;
    [SerializeField] private float zoomSpeed = 5f;

    private float defaultZoom;
    private float targetZoom;

    public bool IsSprinting { get; set; }

    private void Start() {
        if (cinemachineCamera == null)
            cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();

        defaultZoom = cinemachineCamera.Lens.OrthographicSize;
        targetZoom = defaultZoom;

    }
    private void LateUpdate() {
        targetZoom = IsSprinting ? defaultZoom / zoomMultiplier : defaultZoom;
        var lens = cinemachineCamera.Lens;
        lens.OrthographicSize = Mathf.Lerp(lens.OrthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
        cinemachineCamera.Lens = lens;
    }
}
