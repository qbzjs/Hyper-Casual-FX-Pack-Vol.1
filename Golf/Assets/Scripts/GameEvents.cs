using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    
    public static GameEvents current;

    void Awake()
    {
        current = this;
    }

    // UI MANAGER EVENTS
    public event Action<float> OnKillsChange;
    public void KillsChange(float n) {
        OnKillsChange?.Invoke(n);
    }

    public event Action<int, int, int> OnUpgradesChange;
    public void UpgradesChange(int n, int level, int cost) {
        OnUpgradesChange?.Invoke(n,level,cost);
    }

    public event Action<int> OnMoneyChange;
    public void MoneyChange(int n) {
        OnMoneyChange?.Invoke(n);
    }

    public event Action<int> OnLevelChange;
    public void LevelChange(int n) {
        OnLevelChange?.Invoke(n);
    }

    public event Action OnSettingsChange;
    public void SettingsChange() {
        OnSettingsChange?.Invoke();
    }

    public event Action<int, int> OnHandleVictory;
    public void HandleVictory(int n, int j) {
        OnHandleVictory?.Invoke(n,j);
    }

    public event Action OnHandleDefeat;
    public void HandleDefeat() {
        OnHandleDefeat?.Invoke();
    }

    public event Action<int> OnProgressChange;
    public void ProgressChange(int n) {
        OnProgressChange?.Invoke(n);
    }

    // MANAGER EVENTS
    public event Action OnUpgrade1Request;
    public void Upgrade1Request() {
        OnUpgrade1Request?.Invoke();
    }

    public event Action OnUpgrade2Request;
    public void Upgrade2Request() {
        OnUpgrade2Request?.Invoke();
    }

    public event Action OnUpgrade3Request;
    public void Upgrade3Request() {
        OnUpgrade3Request?.Invoke();
    }

    public event Action OnAwardRegularPressed;
    public void AwardRegularPressed() {
        OnAwardRegularPressed?.Invoke();
    }

    public event Action OnAwardDoublePressed;
    public void AwardDoublePressed() {
        OnAwardDoublePressed?.Invoke();
    }

    public event Action OnSkipLevelPressed;
    public void SkipLevelPressed() {
        OnSkipLevelPressed?.Invoke();
    }
    
    public event Action OnDefeat;
    public void Defeat() {
        OnDefeat?.Invoke();
    }

    public event Action OnReward;
    public void Reward() {
        OnReward?.Invoke();
    }

    public event Action OnReturnMainMenu;
    public void ReturnMainMenu() {
        OnReturnMainMenu?.Invoke();
    }

    // AUDIO MANAGER EVENTS
    public event Action OnEnemyHit;
    public void EnemyHit() {
        OnEnemyHit?.Invoke();
    }

    public event Action OnBallHit;
    public void BallHit() {
        OnBallHit?.Invoke();
    }

    public event Action OnChargeStart;
    public void ChargeStart() {
        OnChargeStart?.Invoke();
    }

    public event Action OnChargeStop;
    public void ChargeStop() {
        OnChargeStop?.Invoke();
    }

    public event Action OnPowerUp;
    public void PowerUp() {
        OnPowerUp?.Invoke();
    }
}
