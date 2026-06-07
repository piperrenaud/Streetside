using UnityEngine;
using UnityEngine.UI;

public enum CurrentPlayer
{
    None,
    Player1,
    Player2
}

public class ChangePlayerSpraySprite : MonoBehaviour
{
    private CurrentPlayer currentPlayer = CurrentPlayer.None;

    [SerializeField] private PlayerInfoObject playerInfoObject;

    [SerializeField] private Image sprite1;
    [SerializeField] private Image sprite2;

    public void Awake()
    {
        currentPlayer = CurrentPlayer.None;
        ChangeSprites();
    }

    public void ChangePlayer1()
    {
        currentPlayer = CurrentPlayer.Player1;
    }

    public void ChangePlayer2()
    {
        currentPlayer = CurrentPlayer.Player2;
    }

    public void ChangeNone()
    {
        ChangeSprites();
        currentPlayer = CurrentPlayer.None;
    }

    public void ChangeSprites()
    {
        sprite1.sprite = playerInfoObject.P1Spray;
        sprite2.sprite = playerInfoObject.P2Spray;
    }

    public CurrentPlayer GetcurrentPlayer()
    {
        return currentPlayer;
    }
}
