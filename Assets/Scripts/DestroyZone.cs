using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyZone : MonoBehaviour
{
    public EventTrigger.TriggerEvent destroyTrigger;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BaseEventData eventData = new BaseEventData(EventSystem.current);
        this.destroyTrigger.Invoke(eventData);
        Destroy(collision.gameObject);
    }
}
