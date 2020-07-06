using UnityEngine;
using UnityEngine.EventSystems;

public class DeckController : MonoBehaviour, IPointerClickHandler {
  /// <summary>
  /// The prefab to use for deck objects.
  /// </summary>
  [SerializeField]
  private GameObject cardBackPrefab;

  /// <summary>
  /// The controller for the battle.
  /// </summary>
  [SerializeField]
  private BattleConnection connection;

  /// <summary>
  /// The player ID.
  /// </summary>
  [SerializeField]
  private StringVariable playerID;

  private RectTransform rectTransform;

  private Player player;
  private Inventory deck;

  private bool invalidated;

  private void Awake() {
    this.rectTransform = this.GetComponent<RectTransform>();
  }

  private void OnEnable() {
    this.player = this.connection.GetPlayer(playerID.Value);
    this.deck = player.Deck;
    this.deck.AddChangeHandler(this.Invalidate);
    this.Invalidate();
  }

  private void LateUpdate() {
    if (this.invalidated) {
      this.Revalidate();
    }
  }

  private void OnDisable() {
    this.deck.RemoveChangeHandler(this.Invalidate);
    this.deck = null;
    this.player = null;
    foreach (Transform child in this.rectTransform) {
      GameObject.Destroy(child.gameObject);
    }
  }

  public void OnPointerClick(PointerEventData eventData) {
    this.connection.DrawCard(playerID.Value);
  }

  public void Invalidate() {
    this.invalidated = true;
  }

  public void Revalidate() {
    int childCount = this.rectTransform.childCount;
    if (this.deck.Count < childCount) {
      // There are currently more children than there should be, so remove
      // children from the top of the stack until we're back to the right
      // number.
      int removeCount = childCount - this.deck.Count;
      for (int i = 0; i < removeCount; ++i) {
        int back = this.rectTransform.childCount - 1;
        GameObject child = this.rectTransform.GetChild(back).gameObject;
        child.transform.SetParent(null);
        GameObject.Destroy(child);
      }
    } else if (this.deck.Count > childCount) {
      // There are currently not enough children, so add card backs until we
      // get to the right number.
      int addCount = this.deck.Count - childCount;
      for (int i = 0; i < addCount; ++i) {
        GameObject card = GameObject.Instantiate(cardBackPrefab, this.rectTransform);
        RectTransform cardTransform = card.GetComponent<RectTransform>();
        cardTransform.pivot = new Vector2(0.5f, 0.5f);
        cardTransform.anchorMin = new Vector2(0.5f, 0.5f);
        cardTransform.anchorMax = new Vector2(0.5f, 0.5f);
        // Rotate the card a littel bit to make it sloppy.
        cardTransform.Rotate(0f, 0f, StaticRandom.Range(-3f, 3f));
        // Shift the card slightly for the same reason.
        cardTransform.anchoredPosition = StaticRandom.Range(
          new Vector2(-3f, -3f),
          new Vector2(-3f, -3f)
        ) / this.rectTransform.localScale;
      }
    }
    this.invalidated = false;
  }
}
