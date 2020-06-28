using UnityEngine;

/// <summary>
/// A controller for the library grid view.
/// </summary>
public class LibraryController : MonoBehaviour {
  /// <summary>
  /// A list of all the cards.
  /// </summary>
  [SerializeField]
  private CardList cards;

  /// <summary>
  /// A single card prefab for rendering.
  /// </summary>
  [SerializeField]
  private GameObject cardPrefab;

  /// <summary>
  /// The grid to draw the cards into.
  /// </summary>
  [SerializeField]
  private RectTransform grid;

  /// <summary>
  /// The details controller for displaying more details about a single card.
  /// </summary>
  [SerializeField]
  private DetailsController details;

  private CardModel selected;

  private void Start() {
    // We want to get the total rarity so we can report the probability of
    // each card.
    float totalRarity = 0;
    foreach (CardDetails details in this.cards.List) {
      totalRarity += details.rarity;
    }

    // Add all the cards to the crid.
    // FIXME: If this turns out to be slow doing it all up front we can try
    // loading the cards progressively.
    foreach (CardDetails details in this.cards.List) {
      // Setup the model.
      float probability = details.rarity / totalRarity;
      LibraryCardModel model = new LibraryCardModel(details, probability);

      // Create the object and add it to the library grid.
      GameObject card = GameObject.Instantiate(cardPrefab, grid);

      CardView view = card.GetComponent<CardView>();
      view.Model = model;

      LibraryCardController controller = card.AddComponent<LibraryCardController>();
      controller.Model = model;
      controller.AddClickHandler(this.SelectCard);
    }
  }

  private void SelectCard(LibraryCardModel model) {
    this.details.Model = model;
  }
}
