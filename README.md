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
#### 1. Соберите проект в режиме Release:
dotnet build -c Release
#### 2. Найдите собранную DLL 
в src/TrendLineLibrary/bin/Release/net6.0/TrendLineLibrary.dll
#### 3. Скопируйте DLL в папку плагинов Tiger Trade:
Windows: %APPDATA%\TigerTrade\Plugins\
Mac: ~/Library/Application Support/TigerTrade/Plugins/
#### 4. Перезапустите Tiger Trade Platform

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
Этот проект распространяется под лицензией MIT. См. файл LICENSE для получения дополнительной информации.

## 🤝 Вклад
Добро пожаловать к участию! Открыты PR и обсуждения по улучшению функциональности, архитектуры и совместимости.
 
## 🤝 Участие в разработке
- Форкните репозиторий
- Создайте ветку для новой функциональности (git checkout -b feature/amazing-feature)
- Зафиксируйте изменения (git commit -m 'Add some amazing feature')
- Отправьте изменения в репозиторий (git push origin feature/amazing-feature)
- Откройте Pull Request 

## 📞 Контакты
Разработчик: Ahim
Email: n.bin.andr@gmail.com

Tiger Trade Community: [ссылка на форум/чат]

## 🙏 Благодарности
- Tiger Trade Capital AG за предоставленный API
- Сообществу Tiger Trade за обратную связь и тестирование

## 🚦 Статус проекта
https://img.shields.io/badge/build-passing-brightgreen
https://img.shields.io/badge/tests-10%2520passed-blue
https://img.shields.io/badge/version-1.0.0--beta-orange