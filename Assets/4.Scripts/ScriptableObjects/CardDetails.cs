using UnityEngine;

/// <summary>
/// Details about a card.
/// </summary>
[CreateAssetMenu(fileName="New Card Details", menuName="widjacards/Card Details")]
public class CardDetails : ScriptableObject {
  /// <summary>
  /// The type of card.
  /// </summary>
  public CardType type;

  /// <summary>
  /// Card title.
  /// </summary>
  public string Title;

  /// <summary>
  /// Card description.
  /// </summary>
  [TextArea]
  public string Description;

  /// <summary>
  /// The power level of the card.
  /// </summary>
  public int Power;

  /// <summary>
  /// The sprite to display.
  /// </summary>
  public Sprite Graphic;

  /// <summary>
  /// The card's affiliation.
  /// </summary>
  public Affiliation Affiliation;

  /// <summary>
  /// The cards rarity. Use 1 for the rarest cards and 100 for the most common.
  /// </summary>
  public int rarity = 100;

  public int Noses = 0;
  public int Ears = 0;
  public int Eyes = 0;
  public int Mouths = 0;
}
