using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageSparyChnage : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite sprite;
    public PlayerInfoObject playerInfoObject;
    [SerializeField] private ChangePlayerSpraySprite manager;

    public void Awake()
    {
        this.GetComponent<Image>().sprite = sprite;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (manager.GetcurrentPlayer())
        {
            case CurrentPlayer.Player1:
                playerInfoObject.P1Spray = sprite;
                manager.ChangeNone();
                break;

            case CurrentPlayer.Player2:
                playerInfoObject.P2Spray = sprite;
                manager.ChangeNone();
                break;

            case CurrentPlayer.None:
                break;
        }
    }
}
