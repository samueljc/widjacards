using UnityEngine;

public class Attacker : MonoBehaviour {
  public delegate void AttackHandler(string defenderPlayerID, string defenderCardID);
  public AttackHandler Attack;
}
