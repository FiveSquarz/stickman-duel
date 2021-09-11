using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundHandler : MonoBehaviour {

    [SerializeField]
    int testMap = 0;

    [SerializeField]
    float waitTime = 0f;

    [SerializeField]
    PlayerInfo[] players = null;
    Rigidbody2D[] torsos = new Rigidbody2D[2];

    Text[] texts = new Text[2];
    static int[] scores = { 0, 0 };

    public static bool bothAlive = true;

    void Start() {
        Transform map;
        if (testMap != 0) map = Instantiate(Resources.Load<GameObject>("Maps/Map" + testMap)).transform;
        else {
            GameObject[] maps = Resources.LoadAll<GameObject>("Maps");
            map = Instantiate(maps[Random.Range(0, maps.Length)]).transform;
        }

        bothAlive = true;
        for (int i = 0; i < 2; i++) {
            torsos[i] = players[i].transform.Find("Torso").GetComponent<Rigidbody2D>();
            torsos[i].transform.parent.position = map.Find("SpawnPoint" + i).position;

            texts[i] = transform.Find("Score" + i).GetComponent<Text>();
            texts[i].text = scores[i].ToString();
        }
    }

    void Update() {
        if ((players[0].health <= 0f || players[1].health <= 0f) && bothAlive) {
            bothAlive = false;
            torsos[0].constraints = torsos[1].constraints = RigidbodyConstraints2D.FreezePosition;
            int winner = players[0].health >= players[1].health ? 0 : 1;
            texts[winner].text = (++scores[winner]).ToString();
            foreach (Text text in texts) text.enabled = true;
            Invoke(nameof(nextRound), waitTime);
        }
    }

    void nextRound() {
        foreach (Text text in texts) text.enabled = false;
        InputHandler.fingerIds[0] = InputHandler.fingerIds[1] = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}