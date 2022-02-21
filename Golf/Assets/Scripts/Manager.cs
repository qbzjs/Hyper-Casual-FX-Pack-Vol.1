using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;


public class Manager : MonoBehaviour {

    public UIManager UIManager;
    public EnemyClass EnemyClass;

    public GameObject player;
    public GameObject enemy;
    public GameObject cartEnemy;
    public GameObject projectile;
    public GameObject powerUpProjectile;
    public GameObject spawner;

    Animator playerAnimator;

    int soundEnabled = 1;
    int hapticsEnabled = 1;

    GameObject[] enemies;
    GameObject[] powerUpProjectiles;

    float   beganHolding = 0f;
    int     powerUpReq = 5;

    int enemiesToSpawn = 0;
    float enemySpawnInterval = 1f;
    float currentTime = 0f;

    float pot = 0f;

    float fireRateCounter = 0f;

    float fireRate = 0.5f;
    float ballSpeed = 25f;
    int income = 1;

    int fireRateLevel = 1;
    int fireRateCost = 100;

    int ballSpeedLevel = 1;
    int ballSpeedCost = 100;

    int incomeLevel = 1;
    int incomeCost = 100;

    int upgradeMaxLevel = 20;

    int money = 0;

    public bool playGame = false;

    public bool willDeleteSaves = false;

    int level = 1;

    void Start() {
        //Get EnemyClass
        EnemyClass = GetComponent<EnemyClass>();
        //Set Player Animator
        playerAnimator = player.GetComponent<Animator>();

        //Get all persistent values
        level = PlayerPrefs.GetInt("Level", 1);
        money = PlayerPrefs.GetInt("Money",4000000);
        fireRateLevel = PlayerPrefs.GetInt("FireRateLevel", 1);
        fireRateCost = PlayerPrefs.GetInt("FireRateCost", 100);
        ballSpeedLevel = PlayerPrefs.GetInt("BallSpeedLevel", 1);
        ballSpeedCost = PlayerPrefs.GetInt("BallSpeedCost", 100);
        incomeLevel = PlayerPrefs.GetInt("IncomeLevel", 1);
        incomeCost = PlayerPrefs.GetInt("IncomeCost", 100);
        soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1);
        hapticsEnabled = PlayerPrefs.GetInt("HapticsEnabled", 1);

        //Calculate difficulty for current level
        calculateDifficulty(level);
        
        //Update Upgrade Values
        UpdateUpgradeValues();

        //Update UI
        UIManager.UpdateMoney(money);
        UIManager.UpdateUpgradeInfo(1, fireRateLevel, fireRateCost);
        UIManager.UpdateUpgradeInfo(2, ballSpeedLevel, ballSpeedCost);
        UIManager.UpdateUpgradeInfo(3, incomeLevel, incomeCost);

        //Debug delete
        if(willDeleteSaves) {
            PlayerPrefs.DeleteAll();
        }
    }

    void Update() {
        //Gameplay
        if (!playGame) return;

        //Spawn Enemies
        if (currentTime < enemySpawnInterval) {
            currentTime+=Time.deltaTime;
        } else if (EnemyClass.GetData("AliveCount") < enemiesToSpawn) {
            currentTime = 0f;
            float chance = Random.Range(0f,1f);
            if (chance < 0.1f) {
                Instantiate(cartEnemy, new Vector3(-30-Random.Range(0,10),2,Random.Range(-4.7f,4.7f)), Quaternion.identity);
            } else {
                Instantiate(enemy, new Vector3(-30-Random.Range(0,10),2,Random.Range(-4.7f,4.7f)), Quaternion.identity);
            }
        }

        //Handle Win/Loss
        if (EnemyClass.GetData("TotalKilledCount") == enemiesToSpawn) {
            playGame = false;
            Enemy.ResetStatics();
            level++;
            PlayerPrefs.SetInt("Level", level);
            calculateDifficulty(level);
            UIManager.OpenMainMenu();
        }

        // Adjust Power Up Slider
        if (EnemyClass.GetData("KilledCount") <= powerUpReq) {
            UIManager.UpdatePowerUpSlider(EnemyClass.GetData("KilledCount"));
        }

        // Shooting / Power up
        if (Input.GetMouseButtonDown(0)) {
            beganHolding = Time.time;
        }

        if (Input.GetMouseButton(0)) {
            float holdTime = Time.time - beganHolding;
            if (EnemyClass.GetData("KilledCount") >= powerUpReq && holdTime > 0.1f) {
                pot += Time.deltaTime;
                UIManager.UpdatePowerUpSlider(Mathf.Lerp(powerUpReq, 0, pot / 1f));
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            float holdTime = Time.time - beganHolding;
            if (holdTime >= 1f && EnemyClass.GetData("KilledCount") >= powerUpReq) {
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                powerUpProjectiles = new GameObject[enemies.Length];
                for (int i = 0; i <= enemies.Length - 1; i++) {
                    int r = 3;
                    float x = r * Mathf.Cos(i * (Mathf.PI / (enemies.Length - 1)));
                    float y = r * Mathf.Sin(i * (Mathf.PI / (enemies.Length - 1)));
                    powerUpProjectiles[i] = Instantiate(powerUpProjectile, spawner.transform.position + new Vector3(0, y, x), Quaternion.identity);
                    powerUpProjectiles[i].GetComponent<Rigidbody>().useGravity = true;
                    powerUpProjectiles[i].transform.LookAt(enemies[i].transform.position);
                    powerUpProjectiles[i].GetComponent<Rigidbody>().velocity = powerUpProjectiles[i].transform.forward * 100f;
                }
                EnemyClass.SetKilledCount(0);
                pot = 0;
            } else {
                playerAnimator.SetBool("Swing",false);
                playerAnimator.SetBool("Swing",true);
                pot = 0;
                UIManager.UpdatePowerUpSlider(EnemyClass.GetData("KilledCount"));
            }
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("drive") && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
            playerAnimator.SetBool("Swing",false);
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("drive") && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f) {
            fireProjectile(ref fireRateCounter, Mathf.Lerp(0.8f,0.4f, (float)fireRateLevel/upgradeMaxLevel));
        }
    }

    // Functions
    void fireProjectile(ref float fireRateCounter, float nextFire) {
        if (Time.time > fireRateCounter) {
            fireRateCounter = Time.time + nextFire;
            Vector3 flatAimTarget = calculateTarget();
            GameObject p = Instantiate(projectile, spawner.transform.position, Quaternion.identity);
            p.transform.LookAt(flatAimTarget);
            p.GetComponent<Rigidbody>().useGravity = true;
            p.GetComponent<Rigidbody>().velocity = p.transform.forward * ballSpeed;
        }
    }

    Vector3 calculateTarget() {
        Vector3 cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return screenPoint + cursorRay / Mathf.Abs(cursorRay.y) * Mathf.Abs(screenPoint.y - spawner.transform.position.y);
    }

    void calculateDifficulty (int level) {
        enemiesToSpawn = (int)Mathf.Ceil((level + 10) * 1.2f);
        enemySpawnInterval = Mathf.Clamp(1-level/100f, 0.2f,1f);
    }

    void UpdateUpgradeValues() {
        fireRate = Mathf.Lerp(1.0f,5.0f,(float)fireRateLevel/upgradeMaxLevel);
        ballSpeed = Mathf.Lerp(25f,50f,ballSpeedLevel/upgradeMaxLevel);
        playerAnimator.SetFloat("Speed", fireRate);
        income = incomeLevel;
    }

    public int GetSound() {
        return soundEnabled;
    }

    public void SetSound(int value) {
        soundEnabled = value;
        PlayerPrefs.SetInt("SoundEnabled", soundEnabled);
    }

    public int GetHaptics() {
        return hapticsEnabled;
    }

    public void SetHaptics(int value) {
        hapticsEnabled = value;
        PlayerPrefs.SetInt("HapticsEnabled", hapticsEnabled);
    }

    public int GetMoney() {
        return money;
    }

    public void HandleUpgrade1() {
        if (money > fireRateCost && fireRateLevel < upgradeMaxLevel) {
            money -= fireRateCost;
            fireRateLevel++;
            fireRateCost += 100;
        }
        UpdateUpgradeValues();
        UIManager.UpdateUpgradeInfo(1, fireRateLevel, fireRateCost);
        PlayerPrefs.SetInt("FireRateLevel", fireRateLevel);
        PlayerPrefs.SetInt("FireRateCost", fireRateCost);
    }

    public void HandleUpgrade2() {
        if (money > ballSpeedCost && ballSpeedLevel < upgradeMaxLevel) {
            money -= ballSpeedCost;
            ballSpeedLevel++;
            ballSpeedCost += 100;
        }
        UpdateUpgradeValues();
        UIManager.UpdateUpgradeInfo(2, ballSpeedLevel, ballSpeedCost);
        PlayerPrefs.SetInt("BallSpeedLevel", ballSpeedLevel);
        PlayerPrefs.SetInt("BallSpeedCost", ballSpeedCost);
    }

    public void HandleUpgrade3() {
        if (money > incomeCost && incomeLevel < upgradeMaxLevel) {
            money -= incomeCost;
            incomeLevel++;
            incomeCost += 100;
        }
        UpdateUpgradeValues();
        UIManager.UpdateUpgradeInfo(3, incomeLevel, incomeCost);
        PlayerPrefs.SetInt("IncomeLevel", incomeLevel);
        PlayerPrefs.SetInt("IncomeCost", incomeCost);
    }

    public void HandleReward() {
        money += Random.Range(1, income);
    }
}