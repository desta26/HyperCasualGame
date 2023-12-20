using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
     
    public void OnPointerDown(PointerEventData eventData)
    {
        if(GameManager.Instance.player != null)
            GameManager.Instance.player.GetComponent<PlayerJump>().SetPower(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager.Instance.player != null)
            GameManager.Instance.player.GetComponent<PlayerJump>().SetPower(false);
    }
}
