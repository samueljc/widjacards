using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {
  /// <summary>
  /// The list of all cards.
  /// </summary>
  [SerializeField]
  private CardList cards;

  [SerializeField]
  private StringVariable playerID;

  [SerializeField]
  private StringVariable opponentID;

  [SerializeField]
  private BattleConnection connection;

  [SerializeField]
  private GameObject loadingView;

  [SerializeField]
  private GameObject playerSelectView;

  [SerializeField]
  private GameObject battleView;

  private Player player1;
  private Player player2;

  private void Start() {
    this.player1 = this.connection.AddPlayer(this.GenerateDeck());
    this.player2 = this.connection.AddPlayer(this.GenerateDeck());
    this.connection.StartBattle();
    this.GoToPlayer1();
  }

  public void GoToSwitchPlayers() {
    // TODO: disable the battle view and go to switch players
    this.loadingView.SetActive(false);
    this.battleView.SetActive(false);
    this.playerSelectView.SetActive(true);
  }

  public void GoToPlayer1() {
    this.playerID.Value = this.player1.PrivateID;
    this.opponentID.Value = this.player2.PublicID;

    this.loadingView.SetActive(false);
    this.playerSelectView.SetActive(false);
    this.battleView.SetActive(true);
  }

  public void GoToPlayer2() {
    this.playerID.Value = this.player2.PrivateID;
    this.opponentID.Value = this.player1.PublicID;

    this.loadingView.SetActive(false);
    this.playerSelectView.SetActive(false);
    this.battleView.SetActive(true);
  }

  public void EndTurn() {
    this.connection.EndTurn(playerID.Value);

    this.loadingView.SetActive(false);
    this.battleView.SetActive(false);
    this.playerSelectView.SetActive(true);
  }

  private List<CardModel> GenerateDeck() {
    int totalRarity = 0;
    foreach (CardDetails card in this.cards.List) {
      totalRarity += card.rarity;
    }

    List<CardModel> deck = new List<CardModel>(20);

    // Select cards by rarity.
    // FIXME: Complexity here is 20 * N. Is there a better approach that
    // doesn't duplicate the card list in a different structure? Maybe we can
    // bake something into the card list itself.
    for (int i = 0; i < deck.Capacity; ++i) {
      int index = StaticRandom.Range(0, totalRarity);
      foreach (CardDetails card in this.cards.List) {
        if (index < card.rarity) {
          deck.Add(new CardModel(card));
          break;
        } else {
          index -= card.rarity;
        }
      }
    }
    return deck;
  }
}
