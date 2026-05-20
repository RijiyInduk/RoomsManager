# RoomsManager — Документация

## Обзор

`RoomsManager` — это центральный Unity-менеджер, реализованный как **Singleton** (`DontDestroyOnLoad`), который управляет всем жизненным циклом комнат (уровней) в игре: активацией нужного пола/локации, процедурной генерацией объектов, системой освещения в зависимости от времени суток, спауном врагов, боссов, сундуков и интерактивных объектов.

---

## Архитектура: Singleton

```csharp
public static RoomsManager ins;
```

Менеджер существует в единственном экземпляре и не уничтожается при загрузке новых сцен. При повторном создании старый экземпляр уничтожается и заменяется новым.

---

## Публичные поля (Inspector)

### Освещение

| Поле | Описание |
|------|----------|
| `mainDirLight` | Основной направленный свет сцены |
| `timeDay` | Текущее время суток: 0=Утро, 1=День, 2=Вечер, 3=Ночь |
| `inoutDoor` | Тип помещения: `"Indoor"` или `"Outdoor"` |
| `nameRoom` | Имя текущей комнаты (строка-идентификатор) |
| `lightsMainRooms[4]` | Источники света главных комнат по времени суток |
| `lightsFightRooms[4]` | Источники света боевых комнат по времени суток |
| `lightsOtherRooms[4]` | Особые источники: Дом, Пещера, Fiol, Вечерний свет |
| `lightsFOtherRooms[4]` | Дублирующие особые источники |

### Полы комнат (Floor GameObjects)

| Поле | Локации |
|------|---------|
| `forestFloors[]` | Лесные полы |
| `caveFloors[]` | Пещерные полы |
| `townFloors[]` | Городские полы |
| `towerFloors[]` | Полы башни |
| `cellarFloors[]` | Полы подвала |
| `campFloors[4]` | Лагерь: 0=Лес, 1=Пещера, 2=Город, 3=Башня |
| `otherFloors[10]` | Специальные: Лесопилка, Военный лагерь, Болото, Кладбище, Заброшенный дом (×2), Алтарь пещеры, Портал, Склад города, Логово разбойников |

### Объекты генерации

| Поле | Описание |
|------|----------|
| `positionsSpawn[]` | Точки спауна объектов в комнате |
| `forestTiles[] / caveTiles[] / townTiles[] / towerTiles[]` | Тайлы-подставки под объекты для каждой локации |
| `eliteTiles[4]` | Тайлы для элитных врагов: 0=Лес, 1=Пещера, 2=Город, 3=Башня |
| `bossTiles[]` | Тайлы для боссов |
| `enemyElites[14]` | Элитные враги: Крыса, Собака, Волк, Кабан, Гоблин с кинжалом, Гоблин с топором, Разбойник-кинжал, Разбойник-молот, Культист, Скелет, Зомби, Демон, Водный элементаль, Огненный элементаль |
| `bosses[6]` | Боссы: Лесной, Пещерный, Городской, Лич 1/2/3 этаж |
| `campChests[]` | Сундуки лагеря |
| `bossChests[]` | Сундуки после боссов |
| `dropBagCasual / dropBagElite` | Сумки дропа для обычных и элитных врагов |
| `roomsInteractives[3]` | Интерактивы: 0=Торговец, 1=Алтарь навыков, 2=Алтарь здоровья |
| `obelisks[3] / brokenObelisks[3]` | Обелиски: Лес, Пещера, Город |
| `brokenRes[7]` | Сломанные ресурсы: Дерево(×3), Камень(×3), Кожа |

---

## Методы инициализации

### `Start()`
При запуске устанавливает время суток в 0 (Утро), очищает лайтмапы, отключает все комнаты и туман, гасит основной свет.

### `ClearLightmaps()`
Полная очистка запечённых лайтмапов, light probes и сброс ambient-освещения. Вызывается при инициализации во избежание артефактов освещения.

### `OffAllSimpleRooms()`
Деактивирует все объекты полов всех типов (лес, пещера, город, башня, прочие, лагерь) и ночные стены. Используется перед активацией нужной комнаты.

---

## Активация комнат

### `CreateSimpleRoom()`
Активирует нужный GameObject-пол на основе значения `nameRoom`. Поддерживаемые имена:

**Лес:**
- `AmbushForest` — случайный лесной пол (`forestFloors`, Random)
- `SawmillForest` — Лесопилка
- `GoblinsCampForest` — Лагерь гоблинов
- `AbondHouseForest` — Заброшенный дом (Random из 2 вариантов)
- `SwampForest` — Болото
- `GraveyardForest` — Кладбище

**Пещера:**
- `StoneMineCave`, `GoldMineCave`, `CrystalMineCave`, `AmbushCave` — случайный пещерный пол
- `AltarCave` — Алтарь пещеры
- `PortalCave` — Портал пещеры

**Город:**
- `AmbushTown` — случайный городской пол
- `CapturedStorageTown` — Захваченный склад
- `LairBandits` — Логово разбойников
- `AbondHouseTown` — Заброшенный городской дом
- `GraveyardTown` — Городское кладбище

**Башня / Подвал:**
- `FirstFloorRoomTower`, `SecondFloorRoomTower`, `ThirdFloorRoomTower` — случайный пол башни
- `FirstFloorCellar`, `SecondFloorCellar` — случайный пол подвала

**Комната босса:**
- Активирует `otherFloors` в зависимости от `mapIndex`

### `CreateBossRoom()`
Активирует пол боссовой комнаты по `mapIndex`:
- 1–3 (Лес) → `otherFloors[1]` (Военный лагерь)
- 4–6 (Пещера) → `otherFloors[6]` (Алтарь)
- 7–9 (Город) → `otherFloors[9]` (Логово)
- 10–12 (Башня) / 98–99 (Подвал) → случайный `towerFloors`

---

## Точки входа в комнаты (Enter-методы)

Каждая комната имеет соответствующий Enter-метод, который:
1. Вызывает `CreateSimpleRoom()` для активации пола
2. Делегирует создание объектов специализированному менеджеру локации

| Метод | Делегат |
|-------|---------|
| `EnterSawmill()` | `RForMan.ins.CreateSawmillObjs()` |
| `EnterGoblinsCamp()` | `RForMan.ins.CreateGoblinsCampObjs()` |
| `EnterAmbushForest()` | `RForMan.ins.CreateAmbushForestObjs()` |
| `EnterAbondHouse()` | `RForMan.ins.CreateAbondHouseObjs()` |
| `EnterGraveyard()` | `RForMan.ins.CreateGraveyardObjs()` |
| `EnterSwamp()` | `RForMan.ins.CreateSwampObjs()` |
| `EnterStoneMine()` | `RCaveMan.ins.CreateStoneMineObjs()` |
| `EnterGoldMine()` | `RCaveMan.ins.CreateGoldMineObjs()` |
| `EnterCrystalMineCave()` | `RCaveMan.ins.CreateCrystalMineObjs()` |
| `EnterAmbushCave()` | `RCaveMan.ins.CreateAmbushCaveObjs()` |
| `EnterAltarCave()` | `RCaveMan.ins.CreateAltarCaveObjs()` |
| `EnterPortalCave()` | `RCaveMan.ins.CreatePortalCaveObjs()` |
| `EnterCapturedStorageTown()` | `RTownMan.ins.CreateCapturedStorageTownObjs()` |
| `EnterLairBanditsTown()` | `RTownMan.ins.CreateLairBanditsTownObjs()` |
| `EnterAbondHouseTown()` | `RTownMan.ins.CreateAbondHouseTownObjs()` |
| `EnterAmbushTown()` | `RTownMan.ins.CreateAmbushTownObjs()` |
| `EnterGraveyardTown()` | `RTownMan.ins.CreateGraveyardTownObjs()` |
| `EnterFirstFloorRoomTower()` | `RTowerMan.ins.CreateFirstFloorRoomTowerObjs()` |
| `EnterSecondFloorRoomTower()` | `RTowerMan.ins.CreateSecondFloorRoomTowerObjs()` |
| `EnterThirdFloorRoomTower()` | `RTowerMan.ins.CreateThirdFloorRoomTowerObjs()` |
| `EnterFirstFloorRoomCellar()` | `RTowerMan.ins.CreateFirstFloorRoomCellarObjs()` |
| `EnterSecondFloorRoomCellar()` | `RTowerMan.ins.CreateSecondFloorRoomCellarObjs()` |

---

## Процедурная генерация объектов

Все генерирующие методы размещают соответствующий **тайл-подставку** (зависит от `mapIndex`) в позицию спауна, а затем инстанциируют игровой объект над ней со смещением по Y.

### `GenerateRoomEnemy(int rnd, int i, int iE, enemies[], enemiesE[])`
Генерирует врага или элитного врага на позиции `positionsSpawn[i]`:
- Бросает `rr = Random.Range(1, 101)`
- Если `rr < rnd` И ещё не было элитки (`eliteNumber == 1`) → спаун элитного врага + элитный тайл
- Иначе → случайный обычный враг + тайл локации

### `GenerateRoomChest(int i, chests[])`
Размещает тайл локации и случайный сундук из переданного массива на позиции `positionsSpawn[i]`.

### `GenerateRoomRes(int i, res[])`
Размещает тайл локации и случайный ресурс (дерево/камень/кожа) на позиции `positionsSpawn[i]`.

### `GenerateRoomInteractive(int i)`
Размещает тайл и случайный интерактивный объект из списка `timeIO`. После спауна объект удаляется из списка (без повторов).

### `GeneratePresset(int a1, int a2)`
Генерирует случайный пресет `pr` в диапазоне [a1, a2] — используется для определения раскладки врагов в комнате.

### `GenerateTileSupplies()`
Спаунит тайл локации в позицию `positionsSpawn[7]` (позиция лагеря).

---

## Спаун боссов

### `BossSpawn()`
Активирует боссовую комнату, инстанциирует случайный `bossTile` и соответствующего босса по `mapIndex`:

| mapIndex | Босс |
|----------|------|
| 1–3 | `bosses[0]` — Лесной босс |
| 4–6 | `bosses[1]` — Пещерный босс |
| 7–9 | `bosses[2]` — Городской босс |
| 10 | `bosses[3]` — Лич, 1 этаж |
| 11 | `bosses[4]` — Лич, 2 этаж |
| 12 | `bosses[5]` — Лич, 3 этаж |
| 98–99 | Случайный из первых 3 боссов |

### `SpawnBossTreasure()`
После победы над боссом удаляет все сундуки и спаунит:
- Обычные карты: `eliteTile` + `bossChests[0]`
- Башня/Подвал (10–12, 98–99): 3 тайла + `bossChests[1]` + алтарь навыков + алтарь здоровья
- Лес при активном квесте (`mapIndex == 2`, квест убийства): `smallMonolythForest` вместо сундука
- Башня 12 этаж (финал): `finalBook` вместо сундука

---

## Лагерь (Camp)

### `CreateSupplies()`
Активирует пол и фон лагеря (`campFloors`, `campBG`) по `mapIndex`, вызывает `GenerateTileSupplies()` и инстанциирует `campChests[0]` на позиции 7.

### `ButNewGear()`
Кнопка "Новое снаряжение": уничтожает все сундуки и модели врагов, обнуляет инвентарь, пересоздаёт лагерь.

---

## Система времени суток

### `CheckTimeDay()`
Случайно выбирает время суток: `timeDay = Random.Range(0, 4)`.

### `SelectTimeDay()`
Применяет освещение в зависимости от `inoutDoor` и `timeDay`:

**Indoor — внутри помещения:**
- Заброшенные дома, Склад, Логово, Башня 1 → `lightsOtherRooms[0]` (тёплый свет)
- Пещеры, Шахты, Башня 2, Подвал 1 → `lightsOtherRooms[1]` (холодный пещерный)
- Алтари, Порталы, Башня 3, Подвал 2, BossRoom → `lightsOtherRooms[1]` + `lightsOtherRooms[2]` (мистический)
- Основной свет всегда выключен для Indoor

**Outdoor — снаружи:**

| timeDay | Освещение | Поворот mainDirLight | Цвет |
|---------|-----------|----------------------|------|
| 0 — Утро | `lightsMainRooms[0]` | X=10° | Белый |
| 1 — День | `lightsMainRooms[1]` | X=60° | Тёплый жёлтый |
| 2 — Вечер | `lightsMainRooms[2]` | X=160° | Оранжевый |
| 3 — Ночь | `lightsMainRooms[3]` | X=200° | Свет выкл. |

### `ChangeColorMainLight()`
Меняет цвет направленного света:
- Утро: `RGB(255,255,255)` — нейтральный белый
- День: `RGB(255,235,185)` — тёплый
- Вечер: `RGB(255,160,50)` — оранжевый
- Ночь: `RGB(255,255,255)` — свет отключён

### `ChangeFog()`
Настраивает туман `RenderSettings`:

| Условие | Цвет тумана | Плотность |
|---------|------------|-----------|
| Indoor — дома/склады | Оранжевый | 0.001 |
| Indoor — шахты | Мятный | 0.001 |
| Indoor — алтари/порталы | Фиолетовый | 0.003 |
| Outdoor — утро | Золотистый | 0.002 |
| Outdoor — день | Голубой | 0.001 |
| Outdoor — вечер | Тёмно-коричневый | 0.001 |
| Outdoor — ночь | Тёмно-синий | 0.002 |

### `OnWalls()`
Активирует ночные стены (`nightWallsMR`, `frWallsNight`) только при `timeDay == 3`.

---

## Уничтожение объектов

| Метод | Описание |
|-------|----------|
| `Destr()` | Уничтожает объекты a1–a6 (боссы, сундуки, интерактивы) |
| `Destr0()` | Уничтожает все объекты из массива `aa[]` (процедурно сгенерированные) |

---

## Индексы карты (mapIndex)

| mapIndex | Биом | Сложность |
|----------|------|-----------|
| 0 | Стартовая сцена | — |
| 1–3 | Лес | Easy / Normal / Hard |
| 4–6 | Пещера | Easy / Normal / Hard |
| 7–9 | Город | Easy / Normal / Hard |
| 10–12 | Башня | Easy / Normal / Hard |
| 98–99 | Подвал | — |

---

## Взаимодействие с другими менеджерами

| Менеджер | Роль |
|----------|------|
| `GumeManager` | Предоставляет `mapIndex`, управляет сундуками и врагами |
| `EnemyManager` | Контролирует `eliteNumber`, имена мобов |
| `InventoryManager` | Сброс инвентаря при рестарте лагеря |
| `UIManager` | Закрытие панелей интерфейса |
| `QuestsManager` | Проверка активности квестов при спауне наград |
| `StoryManager` | Сюжетные объекты: монолит, финальная книга |
| `RForMan` | Создание объектов лесных комнат |
| `RCaveMan` | Создание объектов пещерных комнат |
| `RTownMan` | Создание объектов городских комнат |
| `RTowerMan` | Создание объектов башни и подвала |

