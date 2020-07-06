using System.Collections;
using System.Collections.Generic;

public class DeckModel {
  private List<CardModel> deck;

  public DeckModel(in List<CardModel> cards) {
    this.deck = cards;
  }

  public void Shuffle() {
    this.deck.Shuffle();
  }

  public CardModel Draw() {
    if (this.deck.Count > 0) {
      CardModel card = this.deck[this.deck.Count - 1];
      this.deck.RemoveAt(this.deck.Count - 1);
      return card;
    }
    return null;
  }
}

public class HandModel {
  private List<CardModel> hand;
}
