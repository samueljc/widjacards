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
    CardView card = eventData.pointerDrag?.GetComponent<CardView>();
    if (card == null) {
      Debug.LogError("no card dropped");
      return;
    }
    this.connection.PlayCard(this.playerID.Value, card.Model.ID);
  }

  protected override GameObject InstantiateCard(CardModel model) {
    CardView card = GameObject.Instantiate(this.cardPrefab, this.rectTransform);
    card.Model = model;
    // If this is not the controlling player, then this is an attack target.
    if (this.playerID.Value != this.player.PrivateID) {
      Defender target = card.gameObject.AddComponent<Defender>();
      target.DefenderID = playerID.Value;
    } else {
      Attacker character = card.gameObject.AddComponent<Attacker>();
      character.Attack = (string defenderID, string defenderCardID) => {
        this.connection.Attack(
          this.playerID.Value,
          model.ID,
          defenderID,
          defenderCardID
        );
      };
    }
    return card.gameObject;
  }
}