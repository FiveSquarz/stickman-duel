using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Customization : Singleton<Customization> {

    public int maxAiLevel;

    [SerializeField]
    GameObject[] weapons = null;
    int[] weaponInts = new int[2];

    [SerializeField]
    Color32[] colors = null;
    int[] colorInts = new int[2];

    [SerializeField]
    PlayerInfo[] playerInfos = null;
    [SerializeField]
    Text[] texts = null;

    [SerializeField]
    Text player2Text = null;
    [HideInInspector]
    public int aiLevel;

    void Start() {
        weaponInts[0] = weaponInts[1] = weapons.Length;
    }

    public void switchWeapon(String hybrid) { //player.increment
        int[] parts = Array.ConvertAll(hybrid.Split('.'), int.Parse);
        int counter = weaponInts[parts[0]] += parts[1];
        if (counter == -1) counter = weaponInts[parts[0]] = weapons.Length;
        weaponInts[parts[0]] = counter %= weapons.Length + 1;
        if (counter == weapons.Length) texts[parts[0]].text = "Random";
        else texts[parts[0]].text = weapons[counter].name;

        if (weaponInts[parts[0]] == weapons.Length) playerInfos[parts[0]].RemoveWeapon();
        else playerInfos[parts[0]].EquipWeapon(GetWeapon(parts[0]));
    }

    public void switchColor(String hybrid) { //player.increment
        int[] parts = Array.ConvertAll(hybrid.Split('.'), int.Parse);
        int counter = colorInts[parts[0]] += parts[1];
        if (counter == -1) counter = colorInts[parts[0]] = colors.Length - 1;
        colorInts[parts[0]] = counter %= colors.Length;

        playerInfos[parts[0]].Color(GetColor(parts[0]));

        if (playerInfos[parts[0]].weapon) playerInfos[parts[0]].weapon.LookReady();
    }

    public void loadMap() {
        SceneManager.LoadScene("MainScene");
    }

    public GameObject GetWeapon(int player) {
        if (weaponInts[player] == weapons.Length) return weapons[UnityEngine.Random.Range(0, weapons.Length)];
        else return weapons[weaponInts[player]];
    }

    public Color32 GetColor(int player) {
        return colors[colorInts[player]];
    }

    public void changeMode() {
        if (++aiLevel == maxAiLevel + 1) {
            aiLevel = 0;
            player2Text.text = "Player 2";
        } else player2Text.text = "AI " + aiLevel;
    }
}
