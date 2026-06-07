using UnityEngine;

public class PlayersSetup : MonoBehaviour
{
    [SerializeField] private PlayerInfoObject playerInfo;
    [SerializeField] private PlayerGraffiti playerOne;
    [SerializeField] private PlayerGraffiti playerTwo;
    private void Awake()
    {
        playerOne.SetplayerGraffiti(playerInfo.P1Spray);
        playerTwo.SetplayerGraffiti(playerInfo.P2Spray);
    }
}
