using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldController : InventoryField, IDropHandler {
  [SerializeField]
  private CardView cardPrefab;

  [SerializeField]
  private BattleConnection connection;

  [SerializeField]
  private StringVariable playerID;

  private Player player;
  private Inventory field;

  private void OnEnable() {
    this.player = this.connection.GetPlayer(this.playerID.Value);
    this.Inventory = this.player.Field;
  }

  private void OnDisable() {
    this.Inventory = null;
    this.player = null;
  }

  public void OnDrop(PointerEventData eventData) {
    CardView card = eventData.pointerDrag.GetComponent<CardView>();
    if (card == null) {
      // TODO: handle error? Not much of an error here...
      return;
    }

    this.connection.PlayCard(this.playerID.Value, card.Model.ID);
  }

  protected override GameObject InstantiateCard(in CardModel model) {
    CardView view = GameObject.Instantiate(this.cardPrefab, this.rectTransform);
    view.Model = model;
    return view.gameObject;
  }
}