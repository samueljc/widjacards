using UnityEngine;
using UnityEngine.EventSystems;

public class Defender : MonoBehaviour, IDropHandler {
  private string defenderID;

  private CardView cardView;

  public string DefenderID {
    set { this.defenderID = value; }
  }

  private void Awake() {
    this.cardView = this.GetComponent<CardView>();
  }

  public void OnDrop(PointerEventData eventData) {
    Debug.Log("on drop");
    Attacker character = eventData.pointerDrag?.GetComponent<Attacker>();
    if (character == null) {
      Debug.LogError("no card dropped");
      return;
    }
    character.Attack(this.defenderID, this.cardView.Model.ID);
  }
}
