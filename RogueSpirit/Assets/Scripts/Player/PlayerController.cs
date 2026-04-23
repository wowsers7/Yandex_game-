using UnityEngine;

namespace RogueSpirit.Player
{
    /// <summary>
    /// Контроллер игрока. Обрабатывает движение, прыжки, приседание и бег.
    /// Использует Rigidbody2D для физики и Animator для анимаций.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        #region Components
        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        #endregion

        #region Movement Settings
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 8f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 8f;
        
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private float gravityScale = 2.5f;
        [SerializeField] private float variableJumpMultiplier = 0.5f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Vector2 groundCheckSize = new Vector2(0.8f, 0.1f);
        
        [Header("Crouch Settings")]
        [SerializeField] private float crouchSpeedMultiplier = 0.7f;
        [SerializeField] private float normalColliderHeight = 2f;
        [SerializeField] private float crouchColliderHeight = 1f;
        
        [Header("Stamina Settings")]
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float staminaDrainRate = 20f;
        [SerializeField] private float staminaRegenRate = 15f;
        [SerializeField] private float sprintStaminaCost = 20f;
        #endregion

        #region State Variables
        private float currentSpeed;
        private float horizontalInput;
        private float verticalInput;
        private bool isGrounded;
        private bool isCrouching;
        private bool isRunning;
        private bool isJumping;
        private float currentStamina;
        private Collider2D playerCollider;
        
        // Для проверки земли
        private Transform groundCheck;
        #endregion

        #region Properties
        public float CurrentStamina => currentStamina;
        public float MaxStamina => maxStamina;
        public bool IsCrouching => isCrouching;
        public bool IsRunning => isRunning;
        public bool IsGrounded => isGrounded;
        #endregion

        #region Events
        public delegate void StaminaEvent(float current, float max);
        public static event StaminaEvent OnStaminaChanged;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerCollider = GetComponent<Collider2D>();
            
            // Создаём точку проверки земли
            groundCheck = new GameObject("GroundCheck").transform;
            groundCheck.SetParent(transform);
            groundCheck.localPosition = new Vector3(0, -1f, 0);
        }

        private void Start()
        {
            currentStamina = maxStamina;
            rb.gravityScale = gravityScale;
            currentSpeed = walkSpeed;
        }

        private void Update()
        {
            HandleInput();
            CheckGround();
            UpdateAnimator();
            RegenerateStamina();
        }

        private void FixedUpdate()
        {
            Move();
            ApplyCrouch();
        }
        #endregion

        #region Input Handling
        /// <summary>
        /// Обработка ввода с клавиатуры
        /// </summary>
        private void HandleInput()
        {
            // Горизонтальное движение (A/D)
            horizontalInput = Input.GetAxisRaw("Horizontal");
            
            // Вертикальное движение (W для прыжка, S для приседания)
            verticalInput = Input.GetAxisRaw("Vertical");
            
            // Прыжок (W)
            if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
            {
                Jump();
            }
            
            // Приседание (S)
            isCrouching = Input.GetKey(KeyCode.S);
            
            // Бег (Left Shift)
            isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0;
        }
        #endregion

        #region Movement
        /// <summary>
        /// Основная логика движения
        /// </summary>
        private void Move()
        {
            // Определение целевой скорости
            float targetSpeed = horizontalInput * GetMovementSpeed();
            
            // Плавное ускорение/замедление
            if (Mathf.Abs(targetSpeed) > 0.1f)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
            }
            
            // Применение скорости
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
            
            // Ограничение максимальной скорости падения
            if (rb.velocity.y < -15f)
            {
                rb.velocity = new Vector2(rb.velocity.x, -15f);
            }
        }

        /// <summary>
        /// Получение текущей скорости движения
        /// </summary>
        private float GetMovementSpeed()
        {
            if (isCrouching)
            {
                return walkSpeed * crouchSpeedMultiplier;
            }
            
            return isRunning ? runSpeed : walkSpeed;
        }
        #endregion

        #region Jump System
        /// <summary>
        /// Выполнение прыжка
        /// </summary>
        private void Jump()
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            isGrounded = false;
            
            Debug.Log("[PlayerController] Jump!");
        }

        /// <summary>
        /// Проверка переменой высоты прыжка
        /// </summary>
        private void Update()
        {
            // Если игрок отпустил кнопку прыжка рано - уменьшаем вертикальную скорость
            if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpMultiplier);
            }
        }
        #endregion

        #region Crouch System
        /// <summary>
        /// Применение приседания (изменение хитбокса)
        /// </summary>
        private void ApplyCrouch()
        {
            if (playerCollider == null) return;
            
            BoxCollider2D boxCollider = playerCollider as BoxCollider2D;
            if (boxCollider == null) return;
            
            float targetHeight = isCrouching ? crouchColliderHeight : normalColliderHeight;
            boxCollider.size = new Vector2(boxCollider.size.x, Mathf.Lerp(boxCollider.size.y, targetHeight, Time.fixedDeltaTime * 10f));
            
            // Смещение центра коллайдера при приседании
            float yOffset = isCrouching ? -0.5f : 0f;
            boxCollider.offset = new Vector2(boxCollider.offset.x, Mathf.Lerp(boxCollider.offset.y, yOffset, Time.fixedDeltaTime * 10f));
        }
        #endregion

        #region Ground Check
        /// <summary>
        /// Проверка нахождения на земле
        /// </summary>
        private void CheckGround()
        {
            isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
            
            // Сброс флага прыжка при приземлении
            if (isGrounded && isJumping)
            {
                isJumping = false;
            }
        }
        
        // Отладочная визуализация проверки земли
        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
            }
        }
        #endregion

        #region Stamina System
        /// <summary>
        /// Регенерация выносливости
        /// </summary>
        private void RegenerateStamina()
        {
            if (!isRunning && currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);
                
                OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            }
            else if (isRunning)
            {
                currentStamina -= staminaDrainRate * Time.deltaTime;
                currentStamina = Mathf.Max(currentStamina, 0f);
                
                if (currentStamina <= 0f)
                {
                    isRunning = false;
                }
                
                OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            }
        }
        #endregion

        #region Animation
        /// <summary>
        /// Обновление параметров аниматора
        /// </summary>
        private void UpdateAnimator()
        {
            if (animator == null) return;
            
            // Скорость движения
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            
            // Направление спрайта
            if (horizontalInput > 0.1f)
            {
                spriteRenderer.flipX = false;
            }
            else if (horizontalInput < -0.1f)
            {
                spriteRenderer.flipX = true;
            }
            
            // Вертикальная скорость (для анимации прыжка/падения)
            animator.SetFloat("VerticalSpeed", rb.velocity.y);
            
            // На земле ли
            animator.SetBool("IsGrounded", isGrounded);
            
            // Приседание
            animator.SetBool("IsCrouching", isCrouching);
            
            // Бег
            animator.SetBool("IsRunning", isRunning);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Восстановление выносливости (например, от артефакта)
        /// </summary>
        public void RestoreStamina(float amount)
        {
            currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }

        /// <summary>
        /// Увеличение максимальной выносливости
        /// </summary>
        public void IncreaseMaxStamina(float amount)
        {
            maxStamina += amount;
            currentStamina = maxStamina;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }
        #endregion

        #region Collision Detection
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Можно добавить обработку специфических столкновений
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Обработка триггеров (лестницы, предметы и т.д.)
        }
        #endregion
    }
}
