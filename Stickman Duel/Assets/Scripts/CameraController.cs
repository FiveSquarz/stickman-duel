using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    Rigidbody2D torso0 = null,
        torso1 = null;

    [SerializeField]
    float minSize = 0f,
        positionSpeed = 0f;
    Camera mainCamera;

    float horizontalDistance,
        verticalDistance,
        targetRatio;

    void Start() {
        mainCamera = Camera.main;

        AdjustCamera();
    }

    void Update() {
        AdjustCamera();
    }

    void AdjustCamera() {
        Vector3 position = (torso0.position + torso1.position) / 2f;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(position.x, position.y, -10f), Time.deltaTime * positionSpeed);

        horizontalDistance = Mathf.Max(Mathf.Abs(torso0.position.x - mainCamera.transform.position.x), Mathf.Abs(torso1.position.x - mainCamera.transform.position.x)) * 2f + minSize;
        verticalDistance = Mathf.Max(Mathf.Abs(torso0.position.y - mainCamera.transform.position.y), Mathf.Abs(torso1.position.y - mainCamera.transform.position.y)) * 2f + minSize;

        targetRatio = horizontalDistance / verticalDistance;
        mainCamera.orthographicSize = verticalDistance / 2f * (mainCamera.aspect >= targetRatio ? 1f : targetRatio / mainCamera.aspect);
    }
}
