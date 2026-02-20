# 📈 TrendLine Library for Tiger Trade Platform

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-6.0-blue.svg)](https://dotnet.microsoft.com/download)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](http://makeapullrequest.com)

## 1. 🎯 О проекте

Библиотека для построения, отображения и интерактивного управления **трендовыми линиями** на графиках котировок в **Tiger Trade Platform**. Разработана с учетом требований платформы и полностью интегрируется с её API.

## 2. ⚙️ Технологии
Язык: C#
Тип проекта: Class Library (.dll)
Платформа: .NET 6+ (или .NET Framework при необходимости)
Среда разработки: Visual Studio Code
Интеграция: через API/SDK Tiger Trade Platform
Сборка: dotnet build
Тестирование: xUnit / NUnit

## 3. Основные возможности (кратко)

- 📊 Построение линии по двум точкам
- 🔧 Интерактивное редактирование (перетаскивание, изменение точек)
- 📏 Удлинение концов влево/вправо
- 🧲 Примагничивание к свечам
- 🎨 Гибкая настройка стилей (цвет, толщина, тип линии)
- 📝 Добавление текста с настройками шрифта и позиционирования
- ⚠️ Интеграция с системой оповещений Tiger Trade
- 💾 Сохранение и загрузка конфигураций

## 4. 🔧 Основной функционал (расширено)
### 4.1 Геометрия и построение
- Построение линии по двум точкам
- Горизонтальное выравнивание при равенстве цен
- Растягивание/сжатие линии
- Удлинение концов (влево/вправо)
- Примагничивание к свечам

### 4.2 Взаимодействие
- Перетаскивание и редактирование точек
- Меню настроек по двойному щелчку
- Автоматическое выравнивание при удержании Shift

### 4.3 Отображение
- На графике котировок
- Поддержка таймфреймов
- Угол, длина, даты
- Ценовые уровни в точках

### 4.4 Стилизация
- Цвет, тип линии (сплошная, пунктир, точечная, тире-точка)
- Прозрачность, тени, маркеры

### 4.5 Текст
- Добавление текста по checkbox
- Настройка шрифта, цвета, размера, начертания
- Расположение: вертикально (выше, ниже, по центру), горизонтально (слева, по центру, справа)
- Линия не перечёркивает текстовое поле (под текстом линия скрывается)

### 4.6 Alerts и конфигурации
- Интеграция с системой оповещений Tiger Trade
- Сохранение и загрузка конфигураций
- Поддержка нескольких линий и группировка

## 🏗️ Архитектура

Проект построен на основе объектной модели Tiger Trade Platform и использует:
- **ObjectBase** - базовый класс всех графических объектов
- **DxVisualQueue** - система отрисовки через DirectX
- **ChartAlertSettings** - система оповещений
- **Атрибуты [DataContract]** для сериализации
- **Атрибут [ChartObject]** для регистрации в платформе

### 📁 Структура проекта
#### 📐 Общая схема компонентов:
TrendLineLibrary/
├── .gitignore # Игнорируемые файлы Git
├── README.md # Документация проекта
├── global.json # Глобальные настройки .NET
├── TrendLineLibrary.sln # Solution файл
├── lib/ # Внешние зависимости (DLL Tiger Trade)
├── docs/ # Документация
├── scripts/ # Вспомогательные скрипты
├── src/ # Исходный код
│ └── TrendLineLibrary/ # Основная библиотека
│ ├── TrendLineLibrary.csproj
│ └── Objects/ # Классы графических объектов
│ └── TrendLineObject.cs
└── tests/ # Unit-тесты
└── TrendLineLibrary.Tests/
├── TrendLineLibrary.Tests.csproj
└── UnitTest1.cs

## 🚀 Начало работы

### Предварительные требования

- .NET 6.0 SDK или новее
- Visual Studio Code с установленным C# расширением
- Установленная Tiger Trade Platform (для доступа к сборкам)

### Установка зависимостей

1. Скопируйте необходимые сборки Tiger Trade в папку `lib/`:
   - TigerTrade.Chart.dll
   - TigerTrade.Dx.dll
   - TigerTrade.Core.dll
   - TigerTrade.Chart.Alerts.dll

2. Используйте скрипты из папки `scripts/` для автоматического копирования:
   ```bash
   # На Linux/Mac
   ./scripts/copy-dependencies.sh
   
   # На Windows (PowerShell)
   ./scripts/copy-dependencies.ps1

   # Восстановление зависимостей
dotnet restore

## 📦 Сборка и публикация
### Сборка проекта
dotnet build
### Публикация:
- GitHub Releases
- Внутренний NuGet (опционально)
### Документация:
- XML-комментарии
- README.md

## 🔐 Безопасность и производительность
- Потокобезопасность
- Минимизация аллокаций
- Проверка входных данных
- Логирование ошибок

## 🧪 Тестирование
### Unit-тесты:
- Геометрия и расчёты
- Алгоритмы построения
- Рендеринг и взаимодействие
###  Запуск тестов
dotnet test


## 📦 Установка
### 📁 Куда положить DLL
Скомпилированную сборку с TrendLineObject нужно положить в одну из папок:
- C:\Program Files\TigerTrade\Objects\
или
- C:\Users\<User>\Documents\TigerTrade\CustomObjects\
После перезапуска платформы объект появится в списке графических инструментов.

> В разработке. Планируется публикация через NuGet и GitHub Releases.

## 📄 Лицензия
Проект распространяется под лицензией MIT. См. файл `LICENSE`.

## 🤝 Вклад
Добро пожаловать к участию! Открыты PR и обсуждения по улучшению функциональности, архитектуры и совместимости.




## 📋 Статус разработки

### ✅ Завершено
- [x] Инициализация Git репозитория
- [x] Создание структуры проекта
- [x] Настройка .NET 8.0 проектов (библиотека и тесты)
- [x] Добавление зависимостей Tiger Trade
- [x] Базовый класс `TrendLineObject` с атрибутами
- [x] Свойства для настройки линии (цвет, толщина, стиль)
- [x] Свойства для настройки текста (текст, выравнивание, размер шрифта)
- [x] Успешная компиляция проекта

### 🚧 В процессе разработки
- [ ] Реализация метода `Draw()` - отрисовка линии
- [ ] Реализация метода `InObject()` - определение попадания курсора
- [ ] Контрольные точки для редактирования
- [ ] Удлинение концов (влево/вправо)
- [ ] Примагничивание к свечам
- [ ] Отрисовка текста с фоном (линия под текстом скрывается)

### ⏳ В планах
- [ ] Система алертов (интеграция с ChartAlertSettings)
- [ ] Сохранение и загрузка конфигураций
- [ ] Группировка нескольких линий
- [ ] Unit-тесты для геометрии
- [ ] Unit-тесты для алертов
- [ ] Документация по использованию
- [ ] Примеры интеграции

## 🏗️ Архитектура
TrendLineLibrary/
├── .gitignore # Игнорируемые файлы Git
├── README.md # Документация и план разработки
├── global.json # Глобальные настройки .NET
├── TrendLineLibrary.sln # Solution файл
├── lib/ # Внешние зависимости (DLL Tiger Trade)
│ ├── .gitkeep
│ ├── TigerTrade.Chart.dll
│ ├── TigerTrade.Dx.dll
│ ├── TigerTrade.Core.dll
│ └── TigerTrade.Tc.dll
├── docs/ # Документация
│ └── .gitkeep
├── scripts/ # Вспомогательные скрипты
│ └── .gitkeep
├── src/ # Исходный код
│ └── TrendLineLibrary/ # Основная библиотека
│ ├── TrendLineLibrary.csproj
│ └── Objects/ # Классы графических объектов
│ └── TrendLineObject.cs
└── tests/ # Unit-тесты
└── TrendLineLibrary.Tests/
├── TrendLineLibrary.Tests.csproj
└── UnitTest1.cs # Базовые тесты


## 🔧 Технические детали

### Целевая платформа
- .NET 8.0-windows
- WPF интеграция (UseWPF=true)

### Зависимости Tiger Trade
- TigerTrade.Chart.dll
- TigerTrade.Dx.dll
- TigerTrade.Core.dll
- TigerTrade.Tc.dll

### NuGet пакеты (тесты)
- xUnit
- Moq
- FluentAssertions

## 🚀 Сборка и тестирование

```bash
# Восстановление пакетов
dotnet restore

# Сборка проекта
dotnet build

# Запуск тестов
dotnet test

# Очистка
dotnet clean

📝 Класс TrendLineObject
Текущая реализация
[DataContract(Name = "TrendLineObject")]
[ChartObject("X_TrendLine", "Трендовая линия", 2)]
public sealed class TrendLineObject : ObjectBase
{
    // Свойства линии
    public XColor LineColor { get; set; }
    public int LineWidth { get; set; }
    public XDashStyle LineStyle { get; set; }
    
    // Свойства текста
    public string Text { get; set; }
    public ObjectTextAlignment TextAlignment { get; set; }
    public int FontSize { get; set; }
    
    // TODO: Реализовать методы
    - Draw()
    - InObject()
    - GetControlPoint()
    - DrawControlPoints()
    - CheckAlert()
}

План реализации методов
Draw() - отрисовка линии с учетом стилей

InObject() - проверка попадания курсора (с допуском)

GetControlPoint() - определение активной контрольной точки

DrawControlPoints() - отрисовка точек редактирования

Prepare() - подготовка данных перед отрисовкой

ControlPointChanged() - обработка изменения точек (примагничивание)

CheckAlert() - проверка алертов

🌿 Ветки
main - стабильная версия (защищена)

feature/trend-line-object - активная разработка

📊 Прогресс реализации
Дата начала: 20 февраля 2024
Текущий этап: Начальная реализация класса
Следующая задача: Реализация метода Draw()