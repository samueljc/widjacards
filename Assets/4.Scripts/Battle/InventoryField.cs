using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryField : MonoBehaviour {
  [SerializeField]
  private float padding;

  protected RectTransform rectTransform;

  private bool invalidated;

  private Inventory inventory;

  protected Inventory Inventory {
    set {
      if (value != null) {
        this.inventory = value;
        this.inventory.AddChangeHandler(this.Invalidate);
        this.Invalidate();
      } else {
        this.inventory.RemoveChangeHandler(this.Invalidate);
        this.inventory = value;
        foreach (Transform child in this.rectTransform) {
          GameObject.Destroy(child.gameObject);
        }
      }
    }
  }

  protected virtual void Awake() {
    this.rectTransform = this.GetComponent<RectTransform>();
  }

  private void LateUpdate() {
    if (this.invalidated) {
      this.Revalidate();
      this.invalidated = false;
    }
  }

  public void Invalidate() {
    this.invalidated = true;
  }

  protected abstract GameObject InstantiateCard(CardModel model);

  protected virtual void Revalidate() {
    // Remove anything that shouldn't be here and keep track of the cards
    // that are present so we don't duplicate them.
    List<string> present = new List<string>(this.inventory.Count);
    for (int i = this.rectTransform.childCount - 1; i >= 0; --i) {
      Transform transform = this.rectTransform.GetChild(i);
      CardView card = transform.gameObject.GetComponent<CardView>();
      if (card == null || !this.inventory.Contains(card.Model.ID)) {
        // Should probably recycle this instead of deleting it.
        transform.SetParent(null);
        GameObject.Destroy(transform.gameObject);
      } else {
        present.Add(card.Model.ID);
      }
    }

    if (this.inventory.Count == 0) {
      return;
    }

    // Instantiate and add the cards we're missing.
    foreach (CardModel model in this.inventory) {
      if (present.Contains(model.ID)) {
        continue;
      }
      // TODO: Shouldn't be checking this in the client. Need a way to get back
      // what we can see from the server in such a way that if somebody played
      // a card that showed the opponents hand we could see it.
      GameObject card = this.InstantiateCard(model);
      RectTransform transform = card.transform as RectTransform;
      transform.pivot = Vector2.zero; // new Vector2(0.5f, 0.5f);
      transform.anchorMin = Vector2.zero;
      transform.anchorMax = Vector2.zero;
    }

    // Fix spacing around the card objects.
    this.LayoutWithEqualSpacing();
  }

  protected void LayoutWithEqualSpacing() {
    if (this.rectTransform.childCount == 0) {
      return;
    }

    // Position the cards. Order should be relative to our list and not to
    // whatever the current GameObject ordering is.
    float availableSpace = this.rectTransform.rect.width - padding * 2;
    float spaceRemaining = availableSpace;
    foreach (RectTransform transform in this.rectTransform) {
      spaceRemaining -= transform.rect.width;
    }

    // Calculate the spacing such that we try to preserve equal spacing
    // and overlapping when we run out of space.
    float deltaX;
    float x;
    if (spaceRemaining > 0) {
      deltaX = spaceRemaining / (this.rectTransform.childCount + 1f);
      x = padding + deltaX;
    } else {
      x = padding;
      deltaX = spaceRemaining / (this.rectTransform.childCount - 1f);
    }

    // Run through the cards in order and put them in place.
    foreach (RectTransform transform in this.rectTransform) {
      float y = (this.rectTransform.rect.height - transform.rect.height) / 2f;
      CardView view = transform.gameObject.GetComponent<CardView>();
      transform.anchoredPosition = new Vector2(x, y);
      x += transform.rect.width + deltaX;
    }
  }
}
