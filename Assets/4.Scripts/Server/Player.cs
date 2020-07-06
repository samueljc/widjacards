using System.Collections.Generic;

public enum PlayerStatus {
  DRAW,
  PLAY,
  WAIT,
}

public class Player {
  public string PublicID;
  public string PrivateID;

  public Inventory Deck;
  public Inventory Hand;
  public Inventory Field;
  public Inventory Grave;

  public int AvailableDraws;

  public PlayerStatus Status;

  public Player(List<CardModel> deck) {
    this.Deck = new Inventory(deck);
    this.Hand = new Inventory(10);
    this.Field = new Inventory(5);
    this.Grave = new Inventory(deck.Count);
  }
}
