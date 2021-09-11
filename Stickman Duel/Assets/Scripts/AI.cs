using UnityEngine;

public class AI : MonoBehaviour {

    PlayerController controller;
    Transform targetTorso, torso;
    public int aiLevel;

    float seed = 0f,
        maxAngle = 90f,
        frequency = 1f,
        jumpRayDistance = 25f;

    delegate void baseDelegate();
    baseDelegate aiCondition;

    public float aiPercent;

    void Awake() {
        aiLevel = Customization.instance.aiLevel;
        AiPercent();
    }

    void Start() {
        controller = GetComponent<PlayerController>();
        targetTorso = GameObject.Find("Player0").transform.Find("Torso");
        torso = transform.Find("Torso");

        seed = Random.Range(0f, 255f);

        aiCondition = GetComponentInChildren<Weapon>().AiCondition;
    }

    void Update() {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)torso.position - new Vector2(0f, 2.5f), Vector2.down, jumpRayDistance, controller.jumpLayers);
        Debug.Log(hit.transform == null);
        if (hit.transform == null && controller.canJump) controller.direction = Vector2.up;
        else controller.direction = UsefulMethods.RotateVector2(Vector2.ClampMagnitude(targetTorso.position - controller.forcePoint.position, 1f), aiPercent * maxAngle * 2f * (Mathf.PerlinNoise(Time.time * frequency, seed) - 0.5f));

        aiCondition();
    }

    void AiPercent() {
        aiPercent = (float)(Customization.instance.maxAiLevel - aiLevel) / (Customization.instance.maxAiLevel - 1);
    }
}
