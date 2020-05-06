using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentScore;

    public Transform playerGUI;

    public TextMesh HPCounterIn;
    public TextMesh HPCounterOut;

    public Vector2 offset;

    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (player != null)
        {
            playerGUI.position = new Vector2(player.transform.position.x + offset.x,
                                             player.transform.position.y + offset.y);

            HPCounterIn.text = "" + player.HP;
            HPCounterOut.text = "" + player.HP;
        }
    }
}
