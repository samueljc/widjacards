using System.Collections.Generic;
using UnityEngine;

public class HandController : InventoryField {
  [SerializeField]
  private CardView cardPrefab;

  [SerializeField]
  private GameObject cardBackPrefab;

  [SerializeField]
  private BattleConnection connection;

  [SerializeField]
  private StringVariable playerID;

  private Player player;
  private Inventory hand;

  private void OnEnable() {
    this.player = this.connection.GetPlayer(this.playerID.Value);
    this.Inventory = this.player.Hand;
  }

  private void OnDisable() {
    this.Inventory = null;
    this.player = null;
  }

  protected override GameObject InstantiateCard(in CardModel model) {
    if (this.playerID.Value != this.player.PrivateID) {
      return GameObject.Instantiate(this.cardBackPrefab, this.rectTransform);
    }
    CardView view = GameObject.Instantiate(this.cardPrefab, this.rectTransform);
    view.Model = model;
    return view.gameObject;
  }
}
