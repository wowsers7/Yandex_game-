using UnityEngine;

namespace RogueSpirit.Player
{
    /// <summary>
    /// Статистика игрока: здоровье, урон, защита и другие параметры.
    /// Управляет сердечками здоровья и модификаторами.
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        #region Health Settings
        [Header("Health Settings")]
        [SerializeField] private int maxHearts = 10;
        [SerializeField] private int currentHearts = 5;
        
        [Header("Defense Settings")]
        [SerializeField] private float armor = 0f;
        [SerializeField] private float damageReduction = 0f;
        
        [Header("Offense Settings")]
        [SerializeField] private float meleeDamageBonus = 0f;
        [SerializeField] private float rangedDamageBonus = 0f;
        [SerializeField] private float magicDamageBonus = 0f;
        [SerializeField] private float criticalHitChance = 5f;
        [SerializeField] private float criticalHitMultiplier = 2f;
        
        [Header("Regeneration Settings")]
        [SerializeField] private float healthRegenRate = 0f;
        [SerializeField] private float regenDelay = 3f;
        #endregion

        #region State Variables
        private float timeSinceLastDamage;
        private bool isDead;
        #endregion

        #region Properties
        public int CurrentHearts => currentHearts;
        public int MaxHearts => maxHearts;
        public float Armor => armor;
        public bool IsDead => isDead;
        public float CriticalHitChance => criticalHitChance;
        public float CriticalHitMultiplier => criticalHitMultiplier;
        #endregion

        #region Events
        public delegate void HealthEvent(int current, int max);
        public static event HealthEvent OnHealthChanged;
        public static event HealthEvent OnMaxHealthChanged;
        public delegate void DeathEvent();
        public static event DeathEvent OnPlayerDeath;
        #endregion

        #region Unity Methods
        private void Update()
        {
            HandleHealthRegeneration();
            timeSinceLastDamage += Time.deltaTime;
        }
        #endregion

        #region Damage System
        /// <summary>
        /// Получение урона
        /// </summary>
        public void TakeDamage(float damage)
        {
            if (isDead) return;
            
            // Применение брони
            float actualDamage = ApplyArmor(damage);
            
            // Округление до целого числа сердец
            int heartsLost = Mathf.CeilToInt(actualDamage);
            
            currentHearts -= heartsLost;
            timeSinceLastDamage = 0f;
            
            Debug.Log($"[PlayerStats] Took {heartsLost} damage! Remaining: {currentHearts}/{maxHearts}");
            
            // Событие изменения здоровья
            OnHealthChanged?.Invoke(currentHearts, maxHearts);
            
            // Проверка смерти
            if (currentHearts <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Расчёт урона с учётом брони
        /// </summary>
        private float ApplyArmor(float damage)
        {
            // Простая формула: каждая единица брони уменьшает урон на 1
            float reducedDamage = damage - armor - damageReduction;
            return Mathf.Max(1f, reducedDamage); // Минимум 1 урон
        }
        #endregion

        #region Healing System
        /// <summary>
        /// Лечение игрока
        /// </summary>
        public void Heal(int amount)
        {
            if (isDead) return;
            
            int oldHearts = currentHearts;
            currentHearts = Mathf.Min(currentHearts + amount, maxHearts);
            int healedAmount = currentHearts - oldHearts;
            
            Debug.Log($"[PlayerStats] Healed {healedAmount} hearts! Current: {currentHearts}/{maxHearts}");
            
            OnHealthChanged?.Invoke(currentHearts, maxHearts);
        }

        /// <summary>
        /// Автоматическая регенерация здоровья
        /// </summary>
        private void HandleHealthRegeneration()
        {
            if (healthRegenRate <= 0 || isDead) return;
            if (currentHearts >= maxHearts) return;
            
            if (timeSinceLastDamage >= regenDelay)
            {
                currentHearts = Mathf.Min(currentHearts + Mathf.FloorToInt(healthRegenRate * Time.deltaTime), maxHearts);
                OnHealthChanged?.Invoke(currentHearts, maxHearts);
            }
        }
        #endregion

        #region Max Health System
        /// <summary>
        /// Увеличение максимального здоровья
        /// </summary>
        public void IncreaseMaxHealth(int amount = 1)
        {
            maxHearts = Mathf.Min(maxHearts + amount, 20); // Максимум 20 сердец
            currentHearts += amount;
            
            Debug.Log($"[PlayerStats] Max health increased to {maxHearts}!");
            
            OnMaxHealthChanged?.Invoke(currentHearts, maxHearts);
            OnHealthChanged?.Invoke(currentHearts, maxHearts);
        }

        /// <summary>
        /// Установка максимального здоровья
        /// </summary>
        public void SetMaxHealth(int newMax)
        {
            maxHearts = Mathf.Clamp(newMax, 1, 20);
            currentHearts = Mathf.Min(currentHearts, maxHearts);
            
            OnMaxHealthChanged?.Invoke(currentHearts, maxHearts);
            OnHealthChanged?.Invoke(currentHearts, maxHearts);
        }
        #endregion

        #region Death & Revival
        /// <summary>
        /// Смерть игрока
        /// </summary>
        private void Die()
        {
            isDead = true;
            currentHearts = 0;
            
            Debug.Log("[PlayerStats] PLAYER DEAD!");
            
            OnPlayerDeath?.Invoke();
            
            // Вызов GameManager для обработки конца игры
            Managers.GameManager.Instance?.GameOver();
        }

        /// <summary>
        /// Воскрешение игрока (для артефактов/способностей)
        /// </summary>
        public void Revive(int reviveHearts = 3)
        {
            isDead = false;
            currentHearts = Mathf.Min(reviveHearts, maxHearts);
            timeSinceLastDamage = regenDelay + 1f; // Защита от мгновенной смерти
            
            Debug.Log($"[PlayerStats] REVIVED with {currentHearts} hearts!");
            
            OnHealthChanged?.Invoke(currentHearts, maxHearts);
        }
        #endregion

        #region Buffs & Debuffs
        /// <summary>
        /// Временное увеличение урона
        /// </summary>
        public void AddDamageBuff(string buffType, float amount, float duration)
        {
            switch (buffType.ToLower())
            {
                case "melee":
                    meleeDamageBonus += amount;
                    Invoke(nameof(RemoveMeleeBuff), duration);
                    break;
                case "ranged":
                    rangedDamageBonus += amount;
                    Invoke(nameof(RemoveRangedBuff), duration);
                    break;
                case "magic":
                    magicDamageBonus += amount;
                    Invoke(nameof(RemoveMagicBuff), duration);
                    break;
            }
            
            Debug.Log($"[PlayerStats] Added {buffType} damage buff: +{amount}");
        }

        private void RemoveMeleeBuff() => meleeDamageBonus = 0f;
        private void RemoveRangedBuff() => rangedDamageBonus = 0f;
        private void RemoveMagicBuff() => magicDamageBonus = 0f;

        /// <summary>
        /// Временное увеличение критического шанса
        /// </summary>
        public void AddCritBuff(float chanceBonus, float duration)
        {
            float originalCrit = criticalHitChance;
            criticalHitChance = Mathf.Min(criticalHitChance + chanceBonus, 100f);
            
            Invoke(() => criticalHitChance = originalCrit, duration);
            
            Debug.Log($"[PlayerStats] Crit buff added: +{chanceBonus}%");
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Проверка критического удара
        /// </summary>
        public bool CheckCriticalHit()
        {
            return Random.Range(0f, 100f) < criticalHitChance;
        }

        /// <summary>
        /// Расчёт итогового урона с бонусами
        /// </summary>
        public float CalculateTotalDamage(float baseDamage, string damageType)
        {
            float totalDamage = baseDamage;
            
            switch (damageType.ToLower())
            {
                case "melee":
                    totalDamage += meleeDamageBonus;
                    break;
                case "ranged":
                    totalDamage += rangedDamageBonus;
                    break;
                case "magic":
                    totalDamage += magicDamageBonus;
                    break;
            }
            
            // Критический удар
            if (CheckCriticalHit())
            {
                totalDamage *= criticalHitMultiplier;
                Debug.Log("[PlayerStats] CRITICAL HIT!");
            }
            
            return totalDamage;
        }

        /// <summary>
        /// Полное восстановление здоровья
        /// </summary>
        public void FullHeal()
        {
            currentHearts = maxHearts;
            OnHealthChanged?.Invoke(currentHearts, maxHearts);
            
            Debug.Log("[PlayerStats] Full heal!");
        }
        #endregion

        #region Save/Load
        /// <summary>
        /// Сохранение статистики в PlayerPrefs (для демонстрации)
        /// </summary>
        public void SaveStats()
        {
            PlayerPrefs.SetInt("MaxHearts", maxHearts);
            PlayerPrefs.SetInt("CurrentHearts", currentHearts);
            PlayerPrefs.SetFloat("Armor", armor);
            PlayerPrefs.Save();
            
            Debug.Log("[PlayerStats] Stats saved!");
        }

        /// <summary>
        /// Загрузка статистики из PlayerPrefs
        /// </summary>
        public void LoadStats()
        {
            maxHearts = PlayerPrefs.GetInt("MaxHearts", 5);
            currentHearts = PlayerPrefs.GetInt("CurrentHearts", 5);
            armor = PlayerPrefs.GetFloat("Armor", 0f);
            
            OnHealthChanged?.Invoke(currentHearts, maxHearts);
            
            Debug.Log("[PlayerStats] Stats loaded!");
        }
        #endregion
    }
}
