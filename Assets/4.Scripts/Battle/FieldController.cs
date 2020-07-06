using UnityEngine;
using UnityEngine.EventSystems;

public class FieldController : MonoBehaviour, IDropHandler {
  public void OnDrop(PointerEventData eventData) {
    //BattleCardController card = eventData.pointerDrag as BattleCardController;
  }
}