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
    this.hand = this.player.Hand;
    this.hand.AddChangeHandler(this.Invalidate);
    this.Invalidate();
  }

  private void OnDisable() {
    this.hand.RemoveChangeHandler(this.Invalidate);
    this.hand = null;
    this.player = null;
    foreach (Transform child in this.rectTransform) {
      GameObject.Destroy(child.gameObject);
    }
  }

  protected override void Revalidate() {
    // Remove anything that shouldn't be here and keep track of the cards
    // that are present so we don't duplicate them.
    List<string> present = new List<string>(this.hand.Count);
    for (int i = this.rectTransform.childCount - 1; i >= 0; --i) {
      Transform transform = this.rectTransform.GetChild(i);
      CardView card = transform.gameObject.GetComponent<CardView>();
      if (card == null || !this.hand.Contains(card.Model.ID)) {
        // Should probably recycle this instead of deleting it.
        transform.SetParent(null);
        GameObject.Destroy(transform.gameObject);
      } else {
        present.Add(card.Model.ID);
      }
    }

    if (this.hand.Count == 0) {
      return;
    }

    // Instantiate and add the cards we're missing.
    foreach (CardModel card in this.hand) {
      if (present.Contains(card.ID)) {
        continue;
      }
      // TODO: Shouldn't be checking this in the client. Need a way to get back
      // what we can see from the server in such a way that if somebody played
      // a card that showed the opponents hand we could see it.
      GameObject obj = null;
      if (this.playerID.Value == this.player.PrivateID) {
        CardView view = GameObject.Instantiate(this.cardPrefab, this.rectTransform);
        view.Model = card;
        obj = view.gameObject;
      } else {
        obj = GameObject.Instantiate(this.cardBackPrefab, this.rectTransform);
      }
      RectTransform viewTransform = obj.transform as RectTransform;
      viewTransform.pivot = Vector2.zero; // new Vector2(0.5f, 0.5f);
      viewTransform.anchorMin = Vector2.zero;
      viewTransform.anchorMax = Vector2.zero;
    }

    // Fix spacing around the card objects.
    this.LayoutWithEqualSpacing();
  }
}
