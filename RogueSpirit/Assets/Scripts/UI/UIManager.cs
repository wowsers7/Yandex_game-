using UnityEngine;

namespace RogueSpirit.UI
{
    /// <summary>
    /// UI менеджер. Управляет интерфейсом: здоровье, очки, меню, инвентарь.
    /// Подписывается на события GameManager и PlayerStats для обновления UI.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        #region Singleton
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        #endregion

        #region UI References
        [Header("HUD Elements")]
        [SerializeField] private Transform heartsContainer;
        [SerializeField] private GameObject heartPrefab;
        [SerializeField] private Sprite fullHeartSprite;
        [SerializeField] private Sprite emptyHeartSprite;
        
        [Header("Score Display")]
        [SerializeField] private UnityEngine.UI.Text scoreText;
        [SerializeField] private UnityEngine.UI.Text levelText;
        
        [Header("Stamina Bar")]
        [SerializeField] private UnityEngine.UI.Slider staminaSlider;
        
        [Header("Weapon Info")]
        [SerializeField] private UnityEngine.UI.Image weaponIcon;
        [SerializeField] private UnityEngine.UI.Text weaponNameText;
        [SerializeField] private UnityEngine.UI.Text ammoText;
        
        [Header("Menus")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private GameObject howToPlayPanel;
        [SerializeField] private GameObject characterSelectPanel;
        
        [Header("Final Stats")]
        [SerializeField] private UnityEngine.UI.Text finalScoreText;
        [SerializeField] private UnityEngine.UI.Text finalKillsText;
        [SerializeField] private UnityEngine.UI.Text finalLevelText;
        #endregion

        #region State Variables
        private int currentMaxHearts = 5;
        private GameObject[] heartObjects;
        #endregion

        #region Unity Methods
        private void Start()
        {
            InitializeUI();
            SubscribeToEvents();
            ShowMainMenu();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void Update()
        {
            HandleInput();
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Инициализация UI элементов
        /// </summary>
        private void InitializeUI()
        {
            // Создание сердечек здоровья
            if (heartsContainer != null && heartPrefab != null)
            {
                CreateHearts(5);
            }
            
            // Инициализация слайдера выносливости
            if (staminaSlider != null)
            {
                staminaSlider.value = 1f;
                staminaSlider.gameObject.SetActive(false);
            }
            
            // Скрытие всех панелей
            HideAllPanels();
        }

        /// <summary>
        /// Создание сердечек здоровья
        /// </summary>
        private void CreateHearts(int count)
        {
            // Очистка старых сердечек
            if (heartObjects != null)
            {
                foreach (GameObject heart in heartObjects)
                {
                    Destroy(heart);
                }
            }
            
            heartObjects = new GameObject[count];
            
            for (int i = 0; i < count; i++)
            {
                GameObject heart = Instantiate(heartPrefab, heartsContainer);
                heartObjects[i] = heart;
                
                UnityEngine.UI.Image heartImage = heart.GetComponent<UnityEngine.UI.Image>();
                if (heartImage != null)
                {
                    heartImage.sprite = emptyHeartSprite;
                }
            }
        }
        #endregion

        #region Event Subscription
        /// <summary>
        /// Подписка на события
        /// </summary>
        private void SubscribeToEvents()
        {
            Managers.GameManager.OnScoreChanged += UpdateScore;
            Managers.GameManager.OnLivesChanged += UpdateLives;
            Managers.GameManager.OnLevelChanged += UpdateLevel;
            Managers.GameManager.OnGamePaused += ShowPauseMenu;
            Managers.GameManager.OnGameResumed += HidePauseMenu;
            Managers.GameManager.OnGameOver += ShowGameOver;
            Managers.GameManager.OnVictory += ShowVictory;
            
            Player.PlayerStats.OnHealthChanged += UpdateHearts;
            Player.PlayerStats.OnMaxHealthChanged += UpdateMaxHearts;
            Player.PlayerController.OnStaminaChanged += UpdateStamina;
        }

        /// <summary>
        /// Отписка от событий
        /// </summary>
        private void UnsubscribeFromEvents()
        {
            Managers.GameManager.OnScoreChanged -= UpdateScore;
            Managers.GameManager.OnLivesChanged -= UpdateLives;
            Managers.GameManager.OnLevelChanged -= UpdateLevel;
            Managers.GameManager.OnGamePaused -= ShowPauseMenu;
            Managers.GameManager.OnGameResumed -= HidePauseMenu;
            Managers.GameManager.OnGameOver -= ShowGameOver;
            Managers.GameManager.OnVictory -= ShowVictory;
            
            Player.PlayerStats.OnHealthChanged -= UpdateHearts;
            Player.PlayerStats.OnMaxHealthChanged -= UpdateMaxHearts;
            Player.PlayerController.OnStaminaChanged -= UpdateStamina;
        }
        #endregion

        #region HUD Updates
        /// <summary>
        /// Обновление отображения очков
        /// </summary>
        public void UpdateScore(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"SCORE: {score:N0}";
            }
        }

        /// <summary>
        /// Обновление отображения жизней
        /// </summary>
        public void UpdateLives(int lives)
        {
            UpdateHearts(lives, currentMaxHearts);
        }

        /// <summary>
        /// Обновление сердечек здоровья
        /// </summary>
        public void UpdateHearts(int current, int max)
        {
            if (heartObjects == null || heartObjects.Length == 0) return;
            
            currentMaxHearts = max;
            
            // Пересоздание сердечек если изменилось максимальное количество
            if (heartObjects.Length != max)
            {
                CreateHearts(max);
            }
            
            // Обновление спрайтов
            for (int i = 0; i < heartObjects.Length; i++)
            {
                if (heartObjects[i] != null)
                {
                    UnityEngine.UI.Image heartImage = heartObjects[i].GetComponent<UnityEngine.UI.Image>();
                    if (heartImage != null)
                    {
                        heartImage.sprite = (i < current) ? fullHeartSprite : emptyHeartSprite;
                        
                        // Анимация при изменении
                        if (i == current && current < max)
                        {
                            // Можно добавить анимацию потери сердца
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Обновление максимального количества сердечек
        /// </summary>
        public void UpdateMaxHearts(int current, int newMax)
        {
            UpdateHearts(current, newMax);
        }

        /// <summary>
        /// Обновление уровня
        /// </summary>
        public void UpdateLevel(int level)
        {
            if (levelText != null)
            {
                levelText.text = $"LEVEL: {level}/5";
            }
        }

        /// <summary>
        /// Обновление полоски выносливости
        /// </summary>
        public void UpdateStamina(float current, float max)
        {
            if (staminaSlider != null)
            {
                staminaSlider.value = current / max;
                staminaSlider.gameObject.SetActive(current < max);
            }
        }

        /// <summary>
        /// Обновление информации об оружии
        /// </summary>
        public void UpdateWeaponInfo(string weaponName, int currentAmmo, int maxAmmo, Sprite icon)
        {
            if (weaponNameText != null)
            {
                weaponNameText.text = weaponName;
            }
            
            if (ammoText != null)
            {
                ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";
            }
            
            if (weaponIcon != null && icon != null)
            {
                weaponIcon.sprite = icon;
            }
        }
        #endregion

        #region Menu Management
        /// <summary>
        /// Показать главное меню
        /// </summary>
        public void ShowMainMenu()
        {
            HideAllPanels();
            
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(true);
            }
            
            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        /// <summary>
        /// Показать меню паузы
        /// </summary>
        public void ShowPauseMenu()
        {
            HideAllPanels();
            
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(true);
            }
            
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        /// <summary>
        /// Скрыть меню паузы
        /// </summary>
        public void HidePauseMenu()
        {
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(false);
            }
            
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        /// <summary>
        /// Показать экран проигрыша
        /// </summary>
        public void ShowGameOver()
        {
            HideAllPanels();
            
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                
                // Обновление финальной статистики
                if (finalScoreText != null)
                    finalScoreText.text = $"Final Score: {Managers.GameManager.Instance.CurrentScore:N0}";
                
                if (finalKillsText != null)
                    finalKillsText.text = $"Enemies Defeated: {Managers.GameManager.Instance.TotalKills}";
                
                if (finalLevelText != null)
                    finalLevelText.text = $"Reached Level: {Managers.GameManager.Instance.CurrentLevel}";
            }
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        /// <summary>
        /// Показать экран победы
        /// </summary>
        public void ShowVictory()
        {
            HideAllPanels();
            
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
                
                // Обновление финальной статистики
                if (finalScoreText != null)
                    finalScoreText.text = $"Final Score: {Managers.GameManager.Instance.CurrentScore:N0}";
                
                if (finalKillsText != null)
                    finalKillsText.text = $"Enemies Defeated: {Managers.GameManager.Instance.TotalKills}";
            }
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        /// <summary>
        /// Показать меню "Как играть"
        /// </summary>
        public void ShowHowToPlay()
        {
            HideAllPanels();
            
            if (howToPlayPanel != null)
            {
                howToPlayPanel.SetActive(true);
            }
        }

        /// <summary>
        /// Показать выбор персонажа
        /// </summary>
        public void ShowCharacterSelect()
        {
            HideAllPanels();
            
            if (characterSelectPanel != null)
            {
                characterSelectPanel.SetActive(true);
            }
        }

        /// <summary>
        /// Скрыть все панели
        /// </summary>
        private void HideAllPanels()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (victoryPanel != null) victoryPanel.SetActive(false);
            if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
            if (characterSelectPanel != null) characterSelectPanel.SetActive(false);
        }
        #endregion

        #region Button Callbacks
        /// <summary>
        /// Кнопка: Начать игру
        /// </summary>
        public void OnStartGameButton()
        {
            Managers.GameManager.Instance?.StartNewGame();
            HideAllPanels();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        /// <summary>
        /// Кнопка: Продолжить
        /// </summary>
        public void OnResumeButton()
        {
            Managers.GameManager.Instance?.ResumeGame();
        }

        /// <summary>
        /// Кнопка: В главное меню
        /// </summary>
        public void OnMainMenuButton()
        {
            Managers.GameManager.Instance?.ReturnToMenu();
            ShowMainMenu();
        }

        /// <summary>
        /// Кнопка: Рестарт
        /// </summary>
        public void OnRestartButton()
        {
            Managers.GameManager.Instance?.RestartLevel();
            HidePauseMenu();
        }

        /// <summary>
        /// Кнопка: Выход из игры
        /// </summary>
        public void OnQuitButton()
        {
            Managers.GameManager.Instance?.QuitGame();
        }

        /// <summary>
        /// Кнопка: Как играть
        /// </summary>
        public void OnHowToPlayButton()
        {
            ShowHowToPlay();
        }

        /// <summary>
        /// Кнопка: Выбор персонажа
        /// </summary>
        public void OnCharacterSelectButton()
        {
            ShowCharacterSelect();
        }

        /// <summary>
        /// Кнопка: Назад из подменю
        /// </summary>
        public void OnBackButton()
        {
            ShowMainMenu();
        }
        #endregion

        #region Input Handling
        /// <summary>
        /// Обработка ввода для открытия инвентаря
        /// </summary>
        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.I) && Managers.GameManager.Instance.CurrentState == Managers.GameManager.GameState.Playing)
            {
                ToggleInventory();
            }
        }

        /// <summary>
        /// Переключение инвентаря
        /// </summary>
        private void ToggleInventory()
        {
            // Здесь будет логика открытия/закрытия инвентаря
            Debug.Log("[UIManager] Inventory toggled");
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Показать сообщение о получении урона
        /// </summary>
        public void ShowDamageNumber(int damage, Vector3 position, bool isCritical = false)
        {
            // Здесь будет спавн префаба с цифрами урона
            Debug.Log($"[UIManager] Damage number: {damage} at {position}, Critical: {isCritical}");
        }

        /// <summary>
        /// Показать уведомление о получении предмета
        /// </summary>
        public void ShowItemPickupNotification(string itemName, Sprite itemIcon)
        {
            Debug.Log($"[UIManager] Picked up: {itemName}");
            // Здесь будет показ уведомления
        }
        #endregion
    }
}
