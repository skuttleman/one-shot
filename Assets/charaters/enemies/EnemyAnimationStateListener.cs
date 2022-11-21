using Game.System.Events.Player;
using UnityEngine;

public class EnemyAnimationStateListener : MonoBehaviour {
    public void OnStanceChange(PlayerStance stance) { }
    public void OnAttackMode(PlayerAttackMode mode) { }
    public void OnMovement(int moving) { }
    public void OnScope(int enabled) { }
    public void OnStep() { }
}
