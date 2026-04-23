# 📚 ПОДРОБНАЯ ИНСТРУКЦИЯ ПО ЗАПУСКУ И РАБОТЕ С ПРОЕКТОМ

## Rogue Spirit - 2D Action Roguelike Game

---

## 🔰 ЧАСТЬ 1: ПЕРВОНАЧАЛЬНАЯ НАСТРОЙКА

### Шаг 1: Установка Unity Hub и Unity

1. **Скачайте Unity Hub:**
   - Перейдите на https://unity.com/download
   - Скачайте и установите Unity Hub

2. **Установите Unity версии 6000.4.0f1:**
   - Откройте Unity Hub
   - Перейдите во вкладку "Installs"
   - Нажмите "Install Editor"
   - Найдите версию **6000.4.0f1** (или новее)
   - При установке выберите компоненты:
     - ✅ Unity Editor
     - ✅ Visual Studio Editor (для интеграции с VS)
     - ✅ Documentation (опционально)

3. **Активируйте лицензию:**
   - Personal (бесплатно) - для личного использования
   - Follow the activation wizard

---

### Шаг 2: Установка Visual Studio 2022/2026

1. **Скачайте Visual Studio:**
   - Перейдите на https://visualstudio.microsoft.com/
   - Выберите **Visual Studio Community 2022** (бесплатно)
   - Или используйте **Visual Studio 2026** если доступен

2. **При установке выберите компоненты:**
   - ✅ **Разработка под Unity** (обязательно!)
   - ✅ **Рабочая нагрузка для классических .NET**
   - ✅ **C# и .NET**

3. **Завершите установку**

---

## 📂 ЧАСТЬ 2: СОХРАНЕНИЕ ПРОЕКТА НА КОМПЬЮТЕР

### Способ 1: Клонирование из GitHub (если проект уже на GitHub)

```bash
# Откройте терминал (Command Prompt или PowerShell)
cd C:\Users\ВашеИмя\Documents\UnityProjects

# Клонируйте репозиторий
git clone https://github.com/ваш-username/RogueSpirit.git

# Перейдите в папку проекта
cd RogueSpirit
```

### Способ 2: Ручное создание структуры проекта

1. **Создайте папку для проекта:**
   ```
   C:\Users\ВашеИмя\Documents\UnityProjects\RogueSpirit\
   ```

2. **Скопируйте все файлы проекта в эту папку:**
   - Папка `Assets/` со всеми скриптами
   - Папка `ProjectSettings/`
   - Файл `README.md`
   - Другие файлы проекта

3. **Проверьте структуру:**
   ```
   RogueSpirit/
   ├── Assets/
   │   ├── Scripts/
   │   ├── Scenes/
   │   ├── Prefabs/
   │   └── ...
   ├── ProjectSettings/
   ├── Packages/
   └── README.md
   ```

---

## 🎮 ЧАСТЬ 3: ОТКРЫТИЕ ПРОЕКТА В UNITY

### Шаг 1: Добавление проекта в Unity Hub

1. **Откройте Unity Hub**

2. **Нажмите кнопку "Add" (Добавить)**
   - Иконка: ➕
   - Или меню: File → Add project from disk...

3. **Выберите папку проекта:**
   - Найдите папку `RogueSpirit`
   - Выделите её и нажмите "Select Folder"

4. **Проект появится в списке Unity Hub**

### Шаг 2: Открытие проекта

1. **В Unity Hub найдите ваш проект "RogueSpirit"**

2. **Нажмите на название проекта**
   - Unity начнёт открывать проект
   - Первый запуск может занять 2-5 минут

3. **Дождитесь полной загрузки:**
   - Индикатор прогресса внизу
   - Консоль покажет сообщения об импорте ассетов

### Шаг 3: Настройка External Tools

1. **В Unity перейдите:**
   ```
   Edit → Preferences → External Tools
   ```

2. **External Script Editor:**
   - Выберите **Visual Studio 2022** из выпадающего списка
   - Если нет в списке, нажмите "Browse" и найдите:
     ```
     C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe
     ```

3. **Нажмите "Regenerate project files"**
   - Это создаст `.sln` файл для Visual Studio

4. **Закройте Preferences**

---

## 💻 ЧАСТЬ 4: ОТКРЫТИЕ ПРОЕКТА В VISUAL STUDIO

### Способ 1: Из Unity (рекомендуется)

1. **В Unity найдите любой скрипт в окне Project**
   - Например: `Assets/Scripts/Managers/GameManager.cs`

2. **Дважды кликните на скрипт**
   - Visual Studio автоматически откроется
   - Загрузится решение `.sln`

3. **Вы увидите полную архитектуру проекта:**
   ```
   Solution 'RogueSpirit':
   ├── Assets
   │   ├── Scripts
   │   │   ├── Player
   │   │   ├── Enemies
   │   │   ├── Weapons
   │   │   ├── UI
   │   │   ├── Managers
   │   │   └── ...
   │   └── ...
   ```

### Способ 2: Прямое открытие

1. **Откройте Visual Studio**

2. **File → Open → Project/Solution...**

3. **Найдите файл решения:**
   ```
   RogueSpirit/Assembly-CSharp.sln
   ```
   или
   ```
   RogueSpirit/RogueSpirit.sln
   ```

4. **Откройте файл**

---

## ▶️ ЧАСТЬ 5: ЗАПУСК И ТЕСТИРОВАНИЕ ИГРЫ

### Шаг 1: Подготовка сцены

1. **В Unity откройте окно Scenes:**
   - Window → General → Scenes (если не видно)

2. **Найдите главную сцену:**
   - `Assets/Scenes/MainMenu.unity` - главное меню
   - `Assets/Scenes/Level1_Forest.unity` - первый уровень

3. **Дважды кликните на сцену для открытия**

### Шаг 2: Настройка игрока

1. **Найдите префаб игрока:**
   - `Assets/Prefabs/Player.prefab`

2. **Перетащите префаб на сцену** (если ещё не добавлен)

3. **Проверьте компоненты:**
   - ✅ PlayerController
   - ✅ PlayerStats
   - ✅ Rigidbody2D
   - ✅ Collider2D
   - ✅ Animator
   - ✅ SpriteRenderer

### Шаг 3: Запуск игры

1. **Нажмите кнопку Play (▶) вверху Unity**
   - Игра запустится в редакторе
   - Кнопка станет синей

2. **Управление в игре:**
   ```
   A/D - Движение влево/вправо
   W - Прыжок
   S - Приседание
   Left Shift - Бег
   ЛКМ - Атака
   R - Перезарядка
   E - Взаимодействие
   Esc - Пауза
   ```

3. **Остановка игры:**
   - Нажмите кнопку Play (▶) ещё раз
   - Или нажмите `Ctrl + P`

### Шаг 4: Просмотр логов

1. **Откройте консоль:**
   ```
   Window → General → Console
   ```
   или `Ctrl + Shift + C`

2. **Вы увидите логи:**
   ```
   [GameManager] Game initialized!
   [PlayerController] Jump!
   [Enemy] Forest Goblin took 25 damage!
   ```

---

## 🗂️ ЧАСТЬ 6: АРХИТЕКТУРА ПРОЕКТА

### Дерево файлов проекта:

```
RogueSpirit/
│
├── Assets/                              # Все ресурсы проекта
│   │
│   ├── Scripts/                         # C# скрипты
│   │   ├── Player/                      # Скрипты игрока
│   │   │   ├── PlayerController.cs      # Контроллер движения
│   │   │   ├── PlayerStats.cs           # Здоровье и статы
│   │   │   ├── PlayerInput.cs           # Система ввода
│   │   │   └── PlayerAnimation.cs       # Анимации
│   │   │
│   │   ├── Enemies/                     # Скрипты врагов
│   │   │   ├── Enemy.cs                 # Базовый класс врага
│   │   │   ├── EnemyAI.cs               # Искусственный интеллект
│   │   │   ├── EnemyStats.cs            # Статистика врагов
│   │   │   └── Boss/                    # Боссы
│   │   │       └── BossController.cs    # Контроллер босса
│   │   │
│   │   ├── Weapons/                     # Оружие
│   │   │   ├── Weapon.cs                # Базовый класс
│   │   │   ├── MeleeWeapon.cs           # Ближний бой
│   │   │   ├── RangedWeapon.cs          # Дальний бой
│   │   │   └── Projectile.cs            # Снаряды
│   │   │
│   │   ├── Level/                       # Уровни
│   │   │   ├── LevelManager.cs          # Менеджер уровней
│   │   │   ├── SpawnManager.cs          # Спавн врагов
│   │   │   └── Checkpoint.cs            # Чекпоинты
│   │   │
│   │   ├── UI/                          # Интерфейс
│   │   │   ├── UIManager.cs             # Главный UI контроллер
│   │   │   ├── HealthBar.cs             # Полоска здоровья
│   │   │   ├── ScoreManager.cs          # Система очков
│   │   │   └── MenuController.cs        # Меню
│   │   │
│   │   ├── Managers/                    # Глобальные менеджеры
│   │   │   ├── GameManager.cs           # Главное состояние игры
│   │   │   ├── AudioManager.cs          # Звук и музыка
│   │   │   ├── SaveManager.cs           # Сохранения
│   │   │   └── PoolManager.cs           # Пул объектов
│   │   │
│   │   ├── Items/                       # Предметы
│   │   │   ├── Item.cs                  # Базовый предмет
│   │   │   ├── Artifact.cs              # Артефакты
│   │   │   └── Consumable.cs            # Расходники
│   │   │
│   │   └── Utilities/                   # Утилиты
│   │       ├── Extensions.cs            # Расширения
│   │       ├── Constants.cs             # Константы
│   │       └── Helpers.cs               # Helper функции
│   │
│   ├── Scenes/                          # Сцены Unity
│   │   ├── MainMenu.unity               # Главное меню
│   │   ├── Level1_Forest.unity          # Уровень 1: Лес
│   │   ├── Level2_Valley.unity          # Уровень 2: Долина
│   │   ├── Level3_Mountains.unity       # Уровень 3: Горы
│   │   ├── Level4_Water.unity           # Уровень 4: Вода
│   │   └── Level5_Volcano.unity         # Уровень 5: Вулкан
│   │
│   ├── Prefabs/                         # Префабы (шаблоны объектов)
│   │   ├── Player.prefab                # Игрок
│   │   ├── Enemies/                     # Префабы врагов
│   │   ├── Weapons/                     # Префабы оружия
│   │   └── Items/                       # Префабы предметов
│   │
│   ├── Animations/                      # Анимации
│   │   ├── Player/                      # Анимации игрока
│   │   ├── Enemies/                     # Анимации врагов
│   │   └── Controllers/                 # Animator Controllers
│   │
│   ├── Sprites/                         # Графика (2D спрайты)
│   │   ├── Player/                      # Спрайты игрока
│   │   ├── Enemies/                     # Спрайты врагов
│   │   ├── Weapons/                     # Спрайты оружия
│   │   ├── Backgrounds/                 # Фоны уровней
│   │   └── Tilesets/                    # Тайлы для уровней
│   │
│   ├── Audio/                           # Аудио файлы
│   │   ├── Music/                       # Музыка (BGM)
│   │   │   ├── MainMenu.mp3
│   │   │   ├── Level1_Forest.mp3
│   │   │   ├── Level2_Valley.mp3
│   │   │   ├── Level3_Mountains.mp3
│   │   │   ├── Level4_Water.mp3
│   │   │   └── Level5_Volcano.mp3
│   │   └── SFX/                         # Звуковые эффекты
│   │       ├── Player/                  # Звуки игрока
│   │       ├── Weapons/                 # Звуки оружия
│   │       ├── Enemies/                 # Звуки врагов
│   │       └── UI/                      # Звуки интерфейса
│   │
│   └── Materials/                       # Материалы
│
├── ProjectSettings/                     # Настройки проекта Unity
│   ├── ProjectSettings.asset
│   ├── InputManager.asset               # Настройки ввода
│   ├── TagManager.asset                 # Теги и слои
│   └── ...
│
├── Packages/                            # Unity пакеты
│   └── manifest.json
│
├── Assembly-CSharp.sln                  # Решение Visual Studio
└── README.md                            # Документация
```

---

## 🔧 ЧАСТЬ 7: ДОБАВЛЕНИЕ КОНТЕНТА

### Как добавить нового врага:

1. **Создайте скрипт:**
   - В Visual Studio: Правой кнопкой на `Assets/Scripts/Enemies/`
   - Create → C# Script
   - Назовите: `ForestGoblin.cs`

2. **Напишите код:**
   ```csharp
   using UnityEngine;
   using RogueSpirit.Enemies;

   public class ForestGoblin : Enemy
   {
       protected override void Start()
       {
           enemyName = "Forest Goblin";
           maxHealth = 50;
           damage = 8f;
           moveSpeed = 4f;
           
           base.Start();
       }
       
       protected override void PerformAttack()
       {
           // Специальная атака гоблина
           base.PerformAttack();
       }
   }
   ```

3. **Сохраните в Visual Studio** (`Ctrl + S`)

4. **Вернитесь в Unity** - скрипт автоматически компилируется

5. **Создайте префаб:**
   - Создайте пустой GameObject на сцене
   - Добавьте компонент `ForestGoblin`
   - Настройте параметры в Inspector
   - Перетащите в папку `Prefabs/`

### Как добавить новое оружие:

1. **Создайте скрипт в `Assets/Scripts/Weapons/`:**
   ```csharp
   using UnityEngine;
   using RogueSpirit.Weapons;

   public class AssaultRifle : RangedWeapon
   {
       void Start()
       {
           weaponName = "Assault Rifle";
           damage = 15f;
           fireRate = 10f;
           ammoCapacity = 30;
           reloadTime = 2f;
       }
   }
   ```

2. **Сохраните и вернитесь в Unity**

---

## 🌐 ЧАСТЬ 8: ЗАГРУЗКА НА GITHUB

### Шаг 1: Создание репозитория

1. **Перейдите на https://github.com/**

2. **Войдите в свой аккаунт**

3. **Нажмите "+" → "New repository"**

4. **Заполните информацию:**
   - Repository name: `RogueSpirit`
   - Description: "2D Action Roguelike Game inspired by Soul Knight"
   - Public или Private (на ваш выбор)
   - ❌ Не ставьте галочку "Initialize with README"

5. **Нажмите "Create repository"**

### Шаг 2: Создание .gitignore

1. **Создайте файл `.gitignore` в корне проекта:**
   ```
   # Unity generated
   [Ll]ibrary/
   [Tt]emp/
   [Oo]bj/
   [Bb]uild/
   [Bb]uilds/
   
   # Visual Studio
   [Vv][Ss]/
   *.userprefs
   *.pidb
   *.suo
   *.db
   *.user
   *.log
   
   # OS generated
   .DS_Store
   Thumbs.db
   
   # Unity specific
   *.apk
   *.aab
   *.unitypackage
   ```

2. **Сохраните файл**

### Шаг 3: Инициализация Git

**Вариант A: Через командную строку**

```bash
# Откройте терминал в папке проекта
cd C:\Users\ВашеИмя\Documents\UnityProjects\RogueSpirit

# Инициализация git
git init

# Добавление всех файлов
git add .

# Первый коммит
git commit -m "Initial commit: Rogue Spirit game project"

# Добавление удалённого репозитория
git remote add origin https://github.com/ВАШ_USERNAME/RogueSpirit.git

# Загрузка на GitHub
git push -u origin main
```

**Вариант B: Через Visual Studio**

1. **В Visual Studio:**
   - View → Git Changes
   - Или: Git → Create Git Repository

2. **Выберите "Push to a new remote repository"**

3. **Введите URL вашего репозитория:**
   ```
   https://github.com/ВАШ_USERNAME/RogueSpirit.git
   ```

4. **Нажмите "Create and Push"**

**Вариант C: Через GitHub Desktop (проще всего)**

1. **Скачайте GitHub Desktop:**
   - https://desktop.github.com/

2. **Установите и войдите в аккаунт**

3. **File → Add Local Repository**

4. **Выберите папку проекта**

5. **Нажмите "Publish repository"**

### Шаг 4: Проверка на GitHub

1. **Обновите страницу репозитория**

2. **Вы должны увидеть все файлы проекта**

3. **Проверьте что:**
   - ✅ Все `.cs` файлы загружены
   - ✅ `README.md` отображается красиво
   - ✅ Нет больших файлов (>100MB)

---

## 👥 ЧАСТЬ 9: КОМАНДНАЯ РАБОТА

### Как команде работать с проектом:

#### Для клонирования проекта:

```bash
# Ваша команда должна выполнить:
git clone https://github.com/ВАШ_USERNAME/RogueSpirit.git
cd RogueSpirit
```

#### Для внесения изменений:

1. **Получить последние изменения:**
   ```bash
   git pull origin main
   ```

2. **Внести изменения в код**

3. **Создать коммит:**
   ```bash
   git add .
   git commit -m "Description of changes"
   ```

4. **Отправить изменения:**
   ```bash
   git push origin main
   ```

#### Ветки для функций:

```bash
# Создать новую ветку для фичи
git checkout -b feature/new-weapon

# После завершения:
git checkout main
git merge feature/new-weapon
git push origin main
```

---

## 🎵 ЧАСТЬ 10: ДОБАВЛЕНИЕ ЗВУКОВ И АНИМАЦИЙ

### Добавление музыки:

1. **Подготовьте аудио файлы:**
   - Формат: `.mp3` или `.wav`
   - Для фона: 128-192 kbps
   - Для эффектов: 44.1kHz, 16-bit

2. **Импортируйте в Unity:**
   - Перетащите файлы в `Assets/Audio/Music/`
   - Или: Assets → Import New Asset...

3. **Настройте импорт:**
   - Выберите файл в Project окне
   - В Inspector:
     - Force To Mono: ✅ (для фоновой музыки)
     - Load Type: Streaming (для длинных треков)
     - Compression Format: Vorbis

4. **Добавьте AudioSource на сцену:**
   - Создайте пустой GameObject "MusicManager"
   - Добавьте компонент AudioSource
   - Assign AudioClip
   - ✅ Play On Awake
   - ✅ Loop

### Добавление звуковых эффектов:

1. **Импортируйте в `Assets/Audio/SFX/`**

2. **Настройте:**
   - Force To Mono: ✅
   - Load Type: Decompress On Load
   - Compression Format: PCM (для коротких звуков)

3. **Создайте префаб для звука:**
   - GameObject → Create Empty
   - Добавьте AudioSource
   - Настройте Spatial Blend: 2D или 3D
   - Сохраните как префаб

### Создание анимаций:

1. **Подготовьте спрайт-лист:**
   - Последовательность кадров
   - Одинаковый размер каждого кадра

2. **Импортируйте в Unity:**
   - Перетащите в `Assets/Sprites/`
   - Texture Type: Sprite (2D and UI)
   - Sprite Mode: Multiple
   - Click "Sprite Editor"

3. **Нарежьте спрайты:**
   - Slice → Grid By Cell Size
   - Укажите размер ячейки
   - Apply

4. **Создайте анимацию:**
   - Выделите все кадры
   - Перетащите на сцену
   - Unity создаст Animation Clip и Animator Controller

---

## ⚙️ ЧАСТЬ 11: НАСТРОЙКА ФИЗИКИ И КОЛЛИЗИЙ

### Настройка слоёв коллизий:

1. **Edit → Project Settings → Tags and Layers**

2. **Добавьте слои:**
   ```
   Layer 6: Player
   Layer 7: Enemy
   Layer 8: Ground
   Layer 9: Projectile
   Layer 10: Pickup
   ```

3. **Настройте матрицу коллизий:**
   - Physics 2D Settings
   - Layer Collision Matrix
   - Отключите ненужные коллизии

### Настройка тегов:

1. **Tags and Layers → Tags**

2. **Добавьте теги:**
   ```
   - Player
   - Enemy
   - Boss
   - Projectile
   - Pickup
   - Checkpoint
   - Trap
   ```

3. **Присвойте теги объектам:**
   - Выделите объект
   - В Inspector: Tag → выберите нужный

---

## 🏗️ ЧАСТЬ 12: СОЗДАНИЕ БИЛДА (СБОРКИ)

### Для Windows:

1. **File → Build Settings**

2. **Выберите платформу:**
   - Platform: Windows, Mac, Linux
   - Target Platform: Windows

3. **Добавьте сцены:**
   - Нажмите "Add Open Scenes"
   - Или перетащите сцены из Project окна
   - Порядок важен! (0: MainMenu, 1: Level1, ...)

4. **Настройте Player Settings:**
   - Company Name: Ваше имя
   - Product Name: Rogue Spirit
   - Icon: перетащите иконку
   - Resolution: 1920x1080
   - Fullscreen Mode: Fullscreen Window

5. **Нажмите "Build"**

6. **Выберите папку для экспорта**

7. **Дождитесь завершения**

8. **В папке будет .exe файл для запуска**

### Для других платформ:

- **Mac:** Выберите macOS в Build Settings
- **Linux:** Выберите Linux
- **WebGL:** Выберите WebGL (для браузера)

---

## 🐛 ЧАСТЬ 13: ОТЛАДКА И ДЕБАГГИНГ

### Использование Debug.Log:

```csharp
// В любом месте кода:
Debug.Log("Сообщение");
Debug.LogWarning("Предупреждение");
Debug.LogError("Ошибка");

// С переменными:
Debug.Log($"Здоровье: {currentHealth}/{maxHealth}");

// С цветом:
Debug.Log("<color=red>Красное сообщение</color>");
```

### Breakpoints в Visual Studio:

1. **Откройте скрипт в VS**

2. **Кликните на поле слева от номера строки**
   - Появится красный кружок (breakpoint)

3. **Запустите игру в Unity**

4. **При достижении breakpoint игра остановится**

5. **Используйте инструменты отладки:**
   - F10: Step Over
   - F11: Step Into
   - Shift+F11: Step Out
   - F5: Continue

6. **Наводите курсор на переменные для просмотра значений**

### Профилирование:

1. **Window → Analysis → Profiler**

2. **Запустите игру**

3. **Смотрите:**
   - CPU Usage
   - Memory
   - Rendering
   - Physics

4. **Ищите узкие места**

---

## ✅ ЧАСТЬ 14: ЧЕКЛИСТ ПЕРЕД СДАЧЕЙ

### Код:
- [ ] Все скрипты компилируются без ошибок
- [ ] Нет Warning'ов в консоли
- [ ] Код прокомментирован
- [ ] Используются region'ы для организации
- [ ] Следование naming convention (PascalCase для классов, camelCase для переменных)

### Геймплей:
- [ ] Игрок двигается (WASD)
- [ ] Прыжок работает (W)
- [ ] Приседание работает (S)
- [ ] Бег работает (Shift)
- [ ] Атака работает (ЛКМ)
- [ ] Здоровье отображается (сердечки)
- [ ] Очки считаются
- [ ] Враги спавнятся и атакуют
- [ ] Босс имеет несколько фаз

### UI:
- [ ] Главное меню работает
- [ ] Меню паузы работает
- [ ] Экран Game Over работает
- [ ] Экран Victory работает
- [ ] Меню "How to Play" есть

### Аудио:
- [ ] Фоновая музыка играет
- [ ] Звуки выстрелов есть
- [ ] Звуки шагов есть
- [ ] Звуки получения урона есть

### Оптимизация:
- [ ] FPS стабильный (60+)
- [ ] Нет утечек памяти
- [ ] Объекты уничтожаются после смерти
- [ ] Используется Object Pooling для пуль

### Документация:
- [ ] README.md заполнен
- [ ] Архитектура описана
- [ ] Инструкция по запуску есть
- [ ] Управление описано

---

## 🆘 ЧАСТЬ 15: ЧАСТЫЕ ПРОБЛЕМЫ И РЕШЕНИЯ

### Проблема: Скрипты не компилируются

**Решение:**
1. Проверьте namespace в начале файла
2. Убедитесь что все `using` директивы есть
3. Проверьте имена классов (должны совпадать с именем файла)
4. В Unity: Assets → Reimport All

### Проблема: Игра лагает

**Решение:**
1. Откройте Profiler
2. Проверьте количество объектов на сцене
3. Используйте Object Pooling
4. Уменьшите количество частиц
5. Оптимизируйте спрайты (размер, формат)

### Проблема: Звук не работает

**Решение:**
1. Проверьте AudioListener на камере (должен быть один!)
2. Проверьте громкость в Audio Mixer
3. Убедитесь что AudioSource настроен правильно
4. Проверьте что AudioClip назначен

### Проблема: Коллизии не работают

**Решение:**
1. Проверьте что у объектов есть Collider2D
2. Проверьте что есть Rigidbody2D (хотя бы на одном объекте)
3. Проверьте Layer Collision Matrix
4. Убедитесь что теги настроены правильно

### Проблема: Git не загружает файлы

**Решение:**
1. Проверьте .gitignore
2. Файлы >100MB нельзя загрузить на GitHub
3. Используйте Git LFS для больших файлов
4. Проверьте интернет соединение

---

## 📞 ПОДДЕРЖКА И РЕСУРСЫ

### Полезные ссылки:

- **Unity Documentation:** https://docs.unity3d.com/
- **Unity Learn:** https://learn.unity.com/
- **C# Documentation:** https://docs.microsoft.com/dotnet/csharp/
- **GitHub Docs:** https://docs.github.com/

### Сообщества:

- **Unity Forum:** https://forum.unity.com/
- **Stack Overflow:** https://stackoverflow.com/questions/tagged/unity3d
- **Reddit r/Unity3D:** https://reddit.com/r/Unity3D
- **Discord Unity сервера**

---

## 🎉 ЗАКЛЮЧЕНИЕ

Поздравляем! Теперь вы можете:

✅ Сохранить проект на компьютер  
✅ Открыть в Unity и Visual Studio  
✅ Запустить и протестировать игру  
✅ Видеть полную архитектуру проекта  
✅ Загрузить на GitHub для командной работы  
✅ Добавлять новый контент  
✅ Собирать билд для распространения  

**Удачи в разработке Rogue Spirit! 🎮✨**

---

*Документ создан для версии Unity 6000.4.0f1 и Visual Studio 2022/2026*
