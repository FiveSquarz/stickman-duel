using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour {

    [SerializeField]
    RectTransform[] outers = null,
        inners = null;

    [SerializeField]
    float maxViewportX = 0f,
        maxViewportY = 0f,
        distanceFromCenter = 0f;

    [SerializeField]
    PlayerController[] controllers = null;

    public static int?[] fingerIds = { null, null };

    public delegate void baseDelegate();
    public static baseDelegate[] abilities = new baseDelegate[2];

    Camera mainCamera;

    void Start() {
        if (Customization.instance.aiLevel != 0) maxViewportX = 1f;

        foreach (Transform child in transform) child.GetComponent<Image>().color = Customization.instance.GetColor(int.Parse(child.name[child.name.Length - 1].ToString()));
        foreach (Transform item in outers) {
            Color32 color = item.GetComponent<Image>().color;
            color.a = 128;
            item.GetComponent<Image>().color = color;
        }

        mainCamera = Camera.main;
    }

    void Update() {
        for (int x = 0; x < Input.touchCount; x++) {
            Touch touch = Input.GetTouch(x);
            Vector2 viewportPoint = mainCamera.ScreenToViewportPoint(touch.position);
            for (int i = 0; i < (Customization.instance.aiLevel == 0 ? 2 : 1); i++) {
                if (touch.phase == TouchPhase.Began && fingerIds[i] == null && Mathf.Abs(i - viewportPoint.x) <= maxViewportX && viewportPoint.y <= maxViewportY) {
                    fingerIds[i] = touch.fingerId;
                    outers[i].position = touch.position;
                    inners[i].position = outers[i].position;
                    outers[i].gameObject.SetActive(true);
                    inners[i].gameObject.SetActive(true);
                } else if (touch.phase == TouchPhase.Moved && touch.fingerId == fingerIds[i]) {
                    Vector2 direction = touch.position - (Vector2)outers[i].position;
                    inners[i].position = outers[i].position + Vector3.ClampMagnitude(direction, distanceFromCenter);
                    controllers[i].direction = Vector3.ClampMagnitude(direction / distanceFromCenter, 1f);
                } else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && touch.fingerId == fingerIds[i]) {
                    fingerIds[i] = null;
                    controllers[i].direction *= 0;
                    outers[i].gameObject.SetActive(false);
                    inners[i].gameObject.SetActive(false);
                    abilities[i]();
                }
            }
        }
    }
}