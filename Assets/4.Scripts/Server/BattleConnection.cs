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
    // Check the player status.
    Player player = this.players[playerID];
    if (!this.IsControllingPlayer(player, playerID)) {
      Debug.LogErrorFormat("invalid player: {0}", playerID);
      return;
    }
    if (player.Status != PlayerStatus.DRAW && player.Status != PlayerStatus.PLAY) {
      Debug.LogErrorFormat("invalid status (wait your turn): {0}", player.Status.ToString());
      return;
    }
    if (player.AvailableDraws == 0) {
      Debug.LogError("draws exceeded");
      return;
    }

    // Make sure there's still cards in the deck?
    if (player.Deck.Count == 0) {
      Debug.LogError("deck is empty");
      return;
    }

    // Check the hand status.
    if (player.Hand.Full) {
      Debug.LogError("hand is full");
      return;
    }

    player.Hand.Add(player.Deck.Pop());
    player.AvailableDraws -= 1;
  }

  public void PlayCard(string playerID, string cardID) {
    // Check the player status.
    Player player = this.players[playerID];
    if (!this.IsControllingPlayer(player, playerID)) {
      Debug.LogErrorFormat("invalid player: {0}", playerID);
      return;
    }
    if (player.Status != PlayerStatus.PLAY) {
      Debug.LogErrorFormat("invalid status (wait your turn): {0}", player.Status.ToString());
      return;
    }

    // Check the field status.
    if (player.Field.Full) {
      Debug.LogError("field is full");
      return;
    }

    // Check the card status.
    CardModel card = player.Hand.Find(cardID);
    if (card == null) {
      Debug.LogError("card not found");
      return;
    }

    // play the card in the field
    player.Hand.Remove(cardID);
    player.Field.Add(card);
  }

  public void Attack(string attackerID, string attackerCardID, string defenderID, string defenderCardID) {
    // Check the attacker status.
    Player attacker = this.players[attackerID];
    if (!this.IsControllingPlayer(attacker, attackerID)) {
      Debug.LogErrorFormat("invalid attacker: {0}", attackerID);
      return;
    }
    if (attacker.Status != PlayerStatus.PLAY) {
      Debug.LogErrorFormat("invalid status (wait your turn): {0}", attacker.Status.ToString());
      return;
    }

    // Check the defender status.
    Player defender = this.players[defenderID];
    if (defender == null) {
      Debug.LogErrorFormat("invalid defender: {0}", defenderID);
      return;
    }

    // Check the card status.
    CardModel attackerCard = attacker.Field.Find(attackerCardID);
    if (attackerCard == null){
      Debug.LogError("attacker card not found");
      return;
    }
    CardModel defenderCard = defender.Field.Find(defenderCardID);
    if (defenderCard == null) {
      Debug.LogError("defender card not found");
      return;
    }

    // TODO: roll for attack
    // TODO: attack effects
    if (defenderCard.Power < attackerCard.Power) {
      defender.Field.Remove(defenderCardID);
      defender.Grave.Add(defenderCard);
    }
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