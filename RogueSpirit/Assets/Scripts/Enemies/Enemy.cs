using UnityEngine;

namespace RogueSpirit.Enemies
{
    /// <summary>
    /// Базовый класс для всех врагов. Содержит основную логику ИИ, здоровье и поведение.
    /// Наследуется от MonoBehaviour и использует паттерн State Machine для управления состояниями.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class Enemy : MonoBehaviour
    {
        #region Components
        protected Rigidbody2D rb;
        protected Animator animator;
        protected SpriteRenderer spriteRenderer;
        #endregion

        #region Enemy Stats
        [Header("Enemy Stats")]
        [SerializeField] protected string enemyName = "Enemy";
        [SerializeField] protected int maxHealth = 100;
        [SerializeField] protected float damage = 10f;
        [SerializeField] protected float armor = 0f;
        [SerializeField] protected int scoreValue = 10;
        
        [Header("Movement Settings")]
        [SerializeField] protected float moveSpeed = 3f;
        [SerializeField] protected float chaseSpeed = 5f;
        
        [Header("Detection Settings")]
        [SerializeField] protected float detectionRange = 7f;
        [SerializeField] protected float attackRange = 1.5f;
        [SerializeField] protected LayerMask playerLayer;
        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] protected Transform groundCheck;
        [SerializeField] protected Vector2 groundCheckSize = new Vector2(0.8f, 0.1f);
        #endregion

        #region State Variables
        protected int currentHealth;
        protected bool isDead;
        protected bool isGrounded;
        protected Transform target; // Игрок
        protected EnemyState currentState;
        protected float timeSinceLastAttack;
        protected float attackCooldown = 1.5f;
        protected Vector2 patrolPointA;
        protected Vector2 patrolPointB;
        protected Vector2 currentPatrolPoint;
        #endregion

        #region Enemy States
        protected enum EnemyState
        {
            Idle,
            Patrol,
            Alert,
            Chase,
            Attack,
            Retreat,
            Dead
        }
        #endregion

        #region Events
        public delegate void EnemyEvent(Enemy enemy);
        public static event EnemyEvent OnEnemyKilled;
        public static event EnemyEvent OnEnemyDamaged;
        #endregion

        #region Properties
        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public bool IsDead => isDead;
        public int ScoreValue => scoreValue;
        public float Damage => damage;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (groundCheck == null)
            {
                groundCheck = new GameObject("GroundCheck").transform;
                groundCheck.SetParent(transform);
                groundCheck.localPosition = new Vector3(0, -0.9f, 0);
            }
        }

        protected virtual void Start()
        {
            currentHealth = maxHealth;
            currentState = EnemyState.Patrol;
            
            // Инициализация точек патрулирования
            patrolPointA = transform.position;
            patrolPointB = transform.position + Vector3.right * 5f;
            currentPatrolPoint = patrolPointB;
            
            // Поиск игрока при старте
            FindTarget();
        }

        protected virtual void Update()
        {
            if (isDead) return;
            
            CheckGround();
            UpdateSpriteDirection();
            HandleStateBehavior();
            UpdateAnimator();
            
            timeSinceLastAttack += Time.deltaTime;
        }

        protected virtual void FixedUpdate()
        {
            if (isDead) return;
            
            // Физика обрабатывается в подклассах
        }
        #endregion

        #region Target Detection
        /// <summary>
        /// Поиск цели (игрока)
        /// </summary>
        protected virtual void FindTarget()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        /// <summary>
        /// Проверка видимости игрока
        /// </summary>
        protected virtual bool CanSeePlayer()
        {
            if (target == null)
            {
                FindTarget();
                return false;
            }
            
            float distance = Vector2.Distance(transform.position, target.position);
            
            if (distance > detectionRange) return false;
            
            // Raycast для проверки линии видимости
            RaycastHit2D hit = Physics2D.Linecast(
                transform.position,
                target.position,
                ~(playerLayer | gameObject.layer)
            );
            
            return hit.collider == null || hit.collider.CompareTag("Player");
        }

        /// <summary>
        /// Проверка дистанции до игрока
        /// </summary>
        protected virtual float GetDistanceToPlayer()
        {
            if (target == null) return float.MaxValue;
            return Vector2.Distance(transform.position, target.position);
        }
        #endregion

        #region State Machine
        /// <summary>
        /// Обработка поведения в зависимости от состояния
        /// </summary>
        protected virtual void HandleStateBehavior()
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    IdleBehavior();
                    break;
                    
                case EnemyState.Patrol:
                    PatrolBehavior();
                    break;
                    
                case EnemyState.Alert:
                    AlertBehavior();
                    break;
                    
                case EnemyState.Chase:
                    ChaseBehavior();
                    break;
                    
                case EnemyState.Attack:
                    AttackBehavior();
                    break;
                    
                case EnemyState.Retreat:
                    RetreatBehavior();
                    break;
                    
                case EnemyState.Dead:
                    DeadBehavior();
                    break;
            }
            
            UpdateState();
        }

        /// <summary>
        /// Обновление состояния на основе условий
        /// </summary>
        protected virtual void UpdateState()
        {
            if (isDead)
            {
                ChangeState(EnemyState.Dead);
                return;
            }
            
            bool canSeePlayer = CanSeePlayer();
            float distanceToPlayer = GetDistanceToPlayer();
            
            if (canSeePlayer)
            {
                if (distanceToPlayer <= attackRange)
                {
                    ChangeState(EnemyState.Attack);
                }
                else
                {
                    ChangeState(EnemyState.Chase);
                }
            }
            else
            {
                if (currentState == EnemyState.Chase || currentState == EnemyState.Attack)
                {
                    // Потерял игрока - вернуться к патрулированию
                    ChangeState(EnemyState.Patrol);
                }
                else if (currentState == EnemyState.Idle)
                {
                    // Из idle перейти к патрулированию
                    ChangeState(EnemyState.Patrol);
                }
            }
        }

        /// <summary>
        /// Смена состояния
        /// </summary>
        protected virtual void ChangeState(EnemyState newState)
        {
            if (currentState == newState) return;
            
            ExitState(currentState);
            currentState = newState;
            EnterState(newState);
            
            Debug.Log($"[Enemy] {enemyName} changed state to {newState}");
        }

        protected virtual void EnterState(EnemyState state)
        {
            // Переопределяется в подклассах для специфичного поведения
        }

        protected virtual void ExitState(EnemyState state)
        {
            // Переопределяется в подклассах для очистки
        }
        #endregion

        #region State Behaviors
        protected virtual void IdleBehavior()
        {
            // Простой на месте
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        protected virtual void PatrolBehavior()
        {
            // Движение между точками патрулирования
            Vector2 direction = (currentPatrolPoint - (Vector2)transform.position).normalized;
            
            if (Vector2.Distance(transform.position, currentPatrolPoint) < 0.5f)
            {
                // Достиг точки - выбрать следующую
                currentPatrolPoint = (currentPatrolPoint == patrolPointA) ? patrolPointB : patrolPointA;
            }
            
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }

        protected virtual void AlertBehavior()
        {
            // Враг насторожен, смотрит в сторону игрока
            if (target != null)
            {
                FaceTarget();
            }
        }

        protected virtual void ChaseBehavior()
        {
            if (target == null) return;
            
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);
            
            FaceTarget();
        }

        protected virtual void AttackBehavior()
        {
            if (target == null) return;
            
            FaceTarget();
            
            if (timeSinceLastAttack >= attackCooldown)
            {
                PerformAttack();
                timeSinceLastAttack = 0f;
            }
        }

        protected virtual void RetreatBehavior()
        {
            if (target == null) return;
            
            // Отступление от игрока
            Vector2 direction = (transform.position - target.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }

        protected virtual void DeadBehavior()
        {
            // Враг мёртв - ничего не делает
            rb.velocity = Vector2.zero;
        }
        #endregion

        #region Combat System
        /// <summary>
        /// Получение урона
        /// </summary>
        public virtual void TakeDamage(float amount)
        {
            if (isDead) return;
            
            // Применение брони
            float actualDamage = Mathf.Max(1f, amount - armor);
            currentHealth -= Mathf.CeilToInt(actualDamage);
            
            Debug.Log($"[Enemy] {enemyName} took {actualDamage} damage! Health: {currentHealth}/{maxHealth}");
            
            OnEnemyDamaged?.Invoke(this);
            
            // Отталкивание при получении урона
            ApplyKnockback();
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Выполнение атаки
        /// </summary>
        protected virtual void PerformAttack()
        {
            // Базовая атака - переопределяется в подклассах
            Debug.Log($"[Enemy] {enemyName} performs basic attack!");
            
            // Анимация атаки
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
            
            // Проверка попадания по игроку
            if (target != null && GetDistanceToPlayer() <= attackRange)
            {
                Player.PlayerStats playerStats = target.GetComponent<Player.PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(damage);
                }
            }
        }

        /// <summary>
        /// Применение отталкивания
        /// </summary>
        protected virtual void ApplyKnockback()
        {
            // Реализация отталкивания при получении урона
            // Может быть переопределена в подклассах
        }

        /// <summary>
        /// Смерть врага
        /// </summary>
        protected virtual void Die()
        {
            isDead = true;
            currentState = EnemyState.Dead;
            
            Debug.Log($"[Enemy] {enemyName} died!");
            
            // Начисление очков игроку
            Managers.GameManager.Instance?.RegisterKill(scoreValue);
            
            // Событие смерти
            OnEnemyKilled?.Invoke(this);
            
            // Запуск анимации смерти
            if (animator != null)
            {
                animator.SetTrigger("Death");
            }
            
            // Уничтожение после анимации
            Invoke(nameof(DestroyEnemy), 2f);
        }

        /// <summary>
        /// Уничтожение объекта врага
        /// </summary>
        protected virtual void DestroyEnemy()
        {
            // Здесь можно добавить спавн лута
            Destroy(gameObject);
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Разворот в сторону цели
        /// </summary>
        protected virtual void FaceTarget()
        {
            if (target == null) return;
            
            Vector2 direction = (target.position - transform.position).normalized;
            
            if (direction.x > 0.1f)
            {
                spriteRenderer.flipX = false;
            }
            else if (direction.x < -0.1f)
            {
                spriteRenderer.flipX = true;
            }
        }

        /// <summary>
        /// Проверка нахождения на земле
        /// </summary>
        protected virtual void CheckGround()
        {
            isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        }

        /// <summary>
        /// Обновление направления спрайта
        /// </summary>
        protected virtual void UpdateSpriteDirection()
        {
            if (rb.velocity.x > 0.1f)
            {
                spriteRenderer.flipX = false;
            }
            else if (rb.velocity.x < -0.1f)
            {
                spriteRenderer.flipX = true;
            }
        }

        /// <summary>
        /// Обновление параметров аниматора
        /// </summary>
        protected virtual void UpdateAnimator()
        {
            if (animator == null) return;
            
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            animator.SetFloat("VerticalSpeed", rb.velocity.y);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetBool("IsDead", isDead);
        }
        #endregion

        #region Visualization
        private void OnDrawGizmosSelected()
        {
            // Радиус обнаружения
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            
            // Радиус атаки
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            
            // Точки патрулирования
            if (patrolPointA != Vector2.zero && patrolPointB != Vector2.zero)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(patrolPointA, patrolPointB);
                Gizmos.DrawWireSphere(patrolPointA, 0.3f);
                Gizmos.DrawWireSphere(patrolPointB, 0.3f);
            }
            
            // Проверка земли
            if (groundCheck != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
            }
        }
        #endregion
    }
}
