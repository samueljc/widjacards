using System.Collections.Generic;
using UnityEngine;

public enum BattleState {
  START_BATTLE,
}

/// <summary>
/// The battle connection class handles the endpoints 
/// </summary>
public class BattleConnection : MonoBehaviour {
  private Dictionary<string, Player> players;
  private List<string> playerOrder;

  private int turnCount;

  private void Awake() {
    this.players = new Dictionary<string, Player>(2);
    this.playerOrder = new List<string>(2);
  }

  public Player AddPlayer(List<CardModel> deck) {
    // Generate the IDs.
    string publicID = System.Guid.NewGuid().ToString();
    string privateID = System.Guid.NewGuid().ToString();

    // Initialize the player.
    Player player = new Player(deck);
    player.PublicID = publicID;
    player.PrivateID = privateID;

    // Player initialized; add to our data structures.
    this.players[privateID] = player;
    this.players[player.PublicID] = player;
    // Use the public ID in the order so anybody can get it.
    this.playerOrder.Add(player.PublicID);

    return player;
  }

  public Player GetPlayer(string id) {
    // TODO: limit access based on ID
    return this.players[id];
  }

  public void StartBattle() {
    this.playerOrder.Shuffle();
    this.turnCount = 0;
    foreach (var player in players.Values) {
      player.Deck.Shuffle();
      player.AvailableDraws = 5;
      player.Status = PlayerStatus.DRAW;
    }
  }

  public void DrawCard(string playerID) {
    Player player = this.players[playerID];
    if (!this.IsControllingPlayer(player, playerID)) {
      // insufficient permissions
      return;
    }

    if (player.Status != PlayerStatus.DRAW && player.Status != PlayerStatus.PLAY) {
      // can't draw; invalid state
      return;
    }

    if (player.Deck.Count == 0) {
      // can't draw; no cards
      return;
    }

    if (player.AvailableDraws == 0) {
      // already drew cards
      return;
    }

    if (player.Hand.Full) {
      // no space for drawing cards
      return;
    }

    player.Hand.Add(player.Deck.Pop());
    player.AvailableDraws -= 1;
  }

  public void PlayCard(string playerID, string cardID) {
    Player player = this.players[playerID];
    if (!this.IsControllingPlayer(player, playerID)) {
      // insufficient permissions
      return;
    }


    if (player.Status != PlayerStatus.PLAY) {
      // can't play
      return;
    }

    if (player.Field.Full) {
      // can't play; no space
      return;
    }

    CardModel card = player.Hand.Find(cardID);
    if (card == null) {
      // invalid card
      return;
    }

    // play the card in the field
    player.Field.Add(card);
  }

  public void Attack(string attackerID, string attackerCardID, string defenderID, string defenderCardID) {
    Player attacker = this.players[attackerID];
    if (!this.IsControllingPlayer(attacker, attackerID)) {
      // insufficient permissions
      return;
    }

    if (attacker.Status != PlayerStatus.PLAY) {
      // can't play; not your turn
      return;
    }

    Player defender = this.players[defenderID];

    if (!attacker.Hand.Contains(attackerCardID) || !defender.Hand.Contains(defenderCardID)) {
      // invalid card
      return;
    }

    // TODO: perform attack
  }

  public void EndTurn(string playerID) {
    Player player = this.players[playerID];
    if (!this.IsControllingPlayer(player, playerID)) {
      return;
    }

    if (player.Status == PlayerStatus.DRAW && player.AvailableDraws != 0) {
      // You have to use your draws at the beginning of the game.
      return;
    }

    player.Status = PlayerStatus.WAIT;
    this.AdvanceTurn();
  }

  private bool IsControllingPlayer(Player player, string id) {
    return player != null && player.PrivateID == id;
  }

  private void AdvanceTurn() {
    if (this.turnCount == 0) {
      foreach (Player p in this.players.Values) {
        // Make sure everybody is done drawing before going to the next turn.
        if (p.Status != PlayerStatus.WAIT) {
          return;
        }
      }
    }

    ++this.turnCount;
    int playerIndex = (this.turnCount - 1) % this.playerOrder.Count;
    string playerID = this.playerOrder[playerIndex];
    Player player = this.players[playerID];
    if (this.turnCount > 1) {
      // The player going first doesn't get to draw on that turn.
      player.AvailableDraws = 1;
    }
    player.Status = PlayerStatus.PLAY;
  }
}