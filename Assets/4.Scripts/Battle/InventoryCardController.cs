using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryCardController : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerExitHandler {
  /// <summary>
  /// The amount to scale the object by when hovering.
  /// </summary>
  public float HoverScale = 1.2f;

  public float dragOpacity = 0.6f;

  private RectTransform rectTransform;

  private Canvas rootCanvas;

  private CanvasGroup canvasGroup;

  private bool hovered = false;

  private bool dragging = false;

  private Vector2 startDragPosition;

  private void Awake() {
    this.rectTransform = this.GetComponent<RectTransform>();
    this.rootCanvas = this.GetComponentInParent<Canvas>().rootCanvas;
    this.canvasGroup = this.GetComponent<CanvasGroup>();
  }

  public void OnPointerEnter(PointerEventData eventData) {
    if (eventData.pointerDrag == null) {
      this.Hovered = true;
    }
  }

  public void OnPointerDown(PointerEventData eventData) {
    this.Dragging = true;
  }

  public void OnBeginDrag(PointerEventData eventData) {
    // don't do anything; we're tracking the drag initiation in
    // OnPointerDown instead
  }

  public void OnDrag(PointerEventData eventData) {
    this.rectTransform.anchoredPosition += eventData.delta / this.rootCanvas.scaleFactor;
  }

  public void OnEndDrag(PointerEventData eventData) {
    this.Dragging = false;
  }

  public void OnPointerUp(PointerEventData eventData) {
    this.Dragging = false;
  }

  public void OnPointerExit(PointerEventData eventData) {
    this.Hovered = false;
  }

  public bool Hovered {
    get { return this.hovered; }
    set {
      this.hovered = value;
      this.UpdateFocussed();
    }
  }

  public bool Dragging {
    get { return this.dragging; }
    set {
      this.dragging = value;
      if (this.dragging) {
        this.startDragPosition = this.rectTransform.anchoredPosition;
        this.canvasGroup.alpha = this.dragOpacity;
        this.canvasGroup.blocksRaycasts = false;
      } else {
        this.rectTransform.anchoredPosition = this.startDragPosition;
        this.canvasGroup.alpha = 1f;
        this.canvasGroup.blocksRaycasts = true;
      }
      this.UpdateFocussed();
    }
  }

  private void UpdateFocussed() {
    if (this.hovered || this.dragging) {
      this.rectTransform.localScale = new Vector3(this.HoverScale, this.HoverScale, 0f);
    } else {
      this.rectTransform.localScale = new Vector3(1f, 1f, 0f);
    }
  }
}
