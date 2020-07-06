using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryField : MonoBehaviour {
  [SerializeField]
  private float padding;

  protected RectTransform rectTransform;

  private bool invalidated;

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

  protected abstract void Revalidate();

  public void LayoutWithEqualSpacing() {
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
