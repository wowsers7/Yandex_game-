using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueSpirit.Managers
{
    /// <summary>
    /// Главный менеджер игры. Управляет состоянием игры, сценами и общим потоком.
    /// Использует паттерн Singleton для глобального доступа.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region Game State
        public enum GameState
        {
            Menu,
            Playing,
            Paused,
            GameOver,
            Victory
        }

        public GameState CurrentState { get; private set; } = GameState.Menu;
        
        [Header("Game Settings")]
        [SerializeField] private int startingLives = 5;
        [SerializeField] private float scoreMultiplier = 1f;
        
        // Текущие показатели игрока
        public int CurrentLives { get; private set; }
        public int CurrentScore { get; private set; }
        public int CurrentLevel { get; private set; } = 1;
        public int TotalKills { get; private set; }
        #endregion

        #region Events
        public delegate void GameEvent();
        public static event GameEvent OnGameStarted;
        public static event GameEvent OnGamePaused;
        public static event GameEvent OnGameResumed;
        public static event GameEvent OnGameOver;
        public static event GameEvent OnVictory;
        public static event IntEvent OnScoreChanged;
        public static event IntEvent OnLivesChanged;
        public static event IntEvent OnLevelChanged;

        public delegate void IntEvent(int value);
        #endregion

        #region Unity Methods
        private void Start()
        {
            InitializeGame();
        }

        private void Update()
        {
            HandleInput();
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Инициализация игры при старте
        /// </summary>
        public void InitializeGame()
        {
            CurrentLives = startingLives;
            CurrentScore = 0;
            CurrentLevel = 1;
            TotalKills = 0;
            CurrentState = GameState.Menu;
            
            Debug.Log("[GameManager] Game initialized!");
        }
        #endregion

        #region Game Flow Control
        /// <summary>
        /// Начало новой игры
        /// </summary>
        public void StartNewGame()
        {
            CurrentLives = startingLives;
            CurrentScore = 0;
            CurrentLevel = 1;
            TotalKills = 0;
            CurrentState = GameState.Playing;
            
            SceneManager.LoadScene(1); // Загрузка первого уровня
            
            OnGameStarted?.Invoke();
            OnLivesChanged?.Invoke(CurrentLives);
            OnScoreChanged?.Invoke(CurrentScore);
            OnLevelChanged?.Invoke(CurrentLevel);
            
            Debug.Log("[GameManager] New game started!");
        }

        /// <summary>
        /// Переход на следующий уровень
        /// </summary>
        public void NextLevel()
        {
            CurrentLevel++;
            
            if (CurrentLevel > 5)
            {
                Victory();
                return;
            }
            
            OnLevelChanged?.Invoke(CurrentLevel);
            SceneManager.LoadScene(CurrentLevel);
            
            Debug.Log($"[GameManager] Advanced to level {CurrentLevel}");
        }

        /// <summary>
        /// Рестарт текущего уровня
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            CurrentState = GameState.Playing;
            Time.timeScale = 1f;
            
            Debug.Log("[GameManager] Level restarted");
        }

        /// <summary>
        /// Возврат в главное меню
        /// </summary>
        public void ReturnToMenu()
        {
            CurrentState = GameState.Menu;
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
            
            Debug.Log("[GameManager] Returned to menu");
        }
        #endregion

        #region Score & Lives System
        /// <summary>
        /// Добавление очков
        /// </summary>
        public void AddScore(int points)
        {
            CurrentScore += Mathf.RoundToInt(points * scoreMultiplier);
            OnScoreChanged?.Invoke(CurrentScore);
            
            Debug.Log($"[GameManager] Score added: {points}, Total: {CurrentScore}");
        }

        /// <summary>
        /// Потеря жизни
        /// </summary>
        public void LoseLife(int damage = 1)
        {
            CurrentLives -= damage;
            OnLivesChanged?.Invoke(CurrentLives);
            
            if (CurrentLives <= 0)
            {
                GameOver();
            }
            
            Debug.Log($"[GameManager] Life lost: {damage}, Remaining: {CurrentLives}");
        }

        /// <summary>
        /// Восстановление жизни
        /// </summary>
        public void HealLife(int amount = 1)
        {
            CurrentLives = Mathf.Min(CurrentLives + amount, startingLives);
            OnLivesChanged?.Invoke(CurrentLives);
            
            Debug.Log($"[GameManager] Healed: {amount}, Current: {CurrentLives}");
        }

        /// <summary>
        /// Увеличение максимального здоровья
        /// </summary>
        public void IncreaseMaxHealth()
        {
            startingLives++;
            CurrentLives++;
            OnLivesChanged?.Invoke(CurrentLives);
            
            Debug.Log("[GameManager] Max health increased!");
        }
        #endregion

        #region Enemy Kills
        /// <summary>
        /// Регистрация убийства врага
        /// </summary>
        public void RegisterKill(int baseScore)
        {
            TotalKills++;
            AddScore(baseScore);
            
            Debug.Log($"[GameManager] Enemy killed! Total kills: {TotalKills}");
        }
        #endregion

        #region Game States
        /// <summary>
        /// Конец игры
        /// </summary>
        public void GameOver()
        {
            CurrentState = GameState.GameOver;
            Time.timeScale = 0f;
            OnGameOver?.Invoke();
            
            Debug.Log("[GameManager] GAME OVER!");
        }

        /// <summary>
        /// Победа в игре
        /// </summary>
        public void Victory()
        {
            CurrentState = GameState.Victory;
            Time.timeScale = 0f;
            OnVictory?.Invoke();
            
            Debug.Log("[GameManager] VICTORY! You beat the game!");
        }

        /// <summary>
        /// Пауза игры
        /// </summary>
        public void PauseGame()
        {
            if (CurrentState != GameState.Playing) return;
            
            CurrentState = GameState.Paused;
            Time.timeScale = 0f;
            OnGamePaused?.Invoke();
            
            Debug.Log("[GameManager] Game paused");
        }

        /// <summary>
        /// Снятие с паузы
        /// </summary>
        public void ResumeGame()
        {
            if (CurrentState != GameState.Paused) return;
            
            CurrentState = GameState.Playing;
            Time.timeScale = 1f;
            OnGameResumed?.Invoke();
            
            Debug.Log("[GameManager] Game resumed");
        }
        #endregion

        #region Input Handling
        /// <summary>
        /// Обработка ввода для паузы
        /// </summary>
        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (CurrentState == GameState.Playing)
                {
                    PauseGame();
                }
                else if (CurrentState == GameState.Paused)
                {
                    ResumeGame();
                }
            }
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Выход из игры
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("[GameManager] Quitting game...");
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        /// <summary>
        /// Получение текущего множителя очков
        /// </summary>
        public float GetScoreMultiplier()
        {
            return scoreMultiplier;
        }

        /// <summary>
        /// Установка множителя очков (для комбо)
        /// </summary>
        public void SetScoreMultiplier(float multiplier)
        {
            scoreMultiplier = Mathf.Max(1f, multiplier);
        }
        #endregion
    }
}
