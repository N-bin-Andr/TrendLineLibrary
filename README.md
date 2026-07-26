# 📈 TrendLine Library for Tiger Trade Platform

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-6.0-blue.svg)](https://dotnet.microsoft.com/download)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](http://makeapullrequest.com)

## 1. 🎯 О проекте

Библиотека для построения, отображения и интерактивного управления **графическими объектами** на графиках котировок в **Tiger Trade Platform**. Разработана с учетом требований платформы и полностью интегрируется с её API.

### Основные инструменты

#### 📈 Трендовая линия

- Построение линии по двум точкам
- Интерактивное редактирование (перетаскивание, изменение точек)
- Удлинение концов влево/вправо
- Примагничивание к свечам
- Настройка стилей (цвет, толщина, тип линии)
- Текст с настройками шрифта и позиционирования
- Интеграция с системой оповещений Tiger Trade
- Сохранение и загрузка конфигураций

#### 📐 Горизонтальный отрезок (НОВИНКА!)

- **Назначение:** Рисование горизонтальных уровней (отрезков) с возможностью изменения длины
- **Особенности:**
  - Строго горизонтальное выравнивание (Y фиксирован)
  - Перетаскивание концов для изменения длины
  - Поддержка текста и алертов
- **Отличие от стандартной горизонтальной линии:** Можно менять длину, а не бесконечная линия

---

## 2. ⚙️ Технологии

- **Язык:** C#
- **Тип проекта:** Class Library (.dll)
- **Платформа:** .NET 6+ / .NET Framework
- **Среда разработки:** Visual Studio Code
- **Интеграция:** через API/SDK Tiger Trade Platform
- **Сборка:** `dotnet build`
- **Тестирование:** xUnit / NUnit

---

## 3. 🏗️ Архитектура

Проект построен на основе объектной модели Tiger Trade Platform и использует:

- **ObjectBase** — базовый класс всех графических объектов
- **DxVisualQueue** — система отрисовки через DirectX
- **ChartAlertSettings** — система оповещений
- **Атрибуты [DataContract]** для сериализации
- **Атрибут [ChartObject]** для регистрации в платформе

### 📁 Структура проекта

TrendLineLibrary/
├── .gitignore # Игнорируемые файлы Git
├── README.md # Документация проекта
├── global.json # Глобальные настройки .NET
├── TrendLineLibrary.sln # Solution файл
├── lib/ # Внешние зависимости (DLL Tiger Trade)
├── docs/ # Документация
├── scripts/ # Вспомогательные скрипты
├── src/ # Исходный код
│ └── TrendLineLibrary/
│ ├── TrendLineLibrary.csproj
│ └── Objects/ # Классы графических объектов
│ ├── TrendLineObject.cs
│ └── HorizontalSegmentObject.cs
└── tests/ # Unit-тесты
└── TrendLineLibrary.Tests/

---

## 4. 🚀 Начало работы

### Предварительные требования

- .NET 6.0 SDK или новее
- Visual Studio Code с установленным C# расширением
- Установленная Tiger Trade Platform (для доступа к сборкам)

### Установка зависимостей

1. Скопируйте необходимые сборки Tiger Trade в папку `lib/`:
   - `TigerTrade.Chart.dll`
   - `TigerTrade.Dx.dll`
   - `TigerTrade.Core.dll`
   - `TigerTrade.Chart.Alerts.dll`

2. Используйте скрипты из папки `scripts/` для автоматического копирования:

   ```bash
   # На Linux/Mac
   ./scripts/copy-dependencies.sh

   # На Windows (PowerShell)
   ./scripts/copy-dependencies.ps1


3. Восстановите зависимости:
bash: dotnet restore

### 📦 Сборка и публикация

#### Сборка проекта

##### Сборка в режиме Release

dotnet build -c Release

##### Сборка с указанием выходной папки

dotnet build -c Release -o bin/Release/

#### Установка в Tiger Trade

1. Найдите собранную DLL:
src/TrendLineLibrary/bin/Release/net6.0/TrendLineLibrary.dll

2. Скопируйте DLL в одну из папок:
C:\Program Files\TigerTrade\Objects\
C:\Users\<User>\Documents\TigerTrade\CustomObjects\

3. Перезапустите Tiger Trade Platform
4. Объекты появятся в панели инструментов:
"Трендовая линия"
"Горизонтальный отрезок"

#### Публикация

GitHub Releases
Внутренний NuGet (опционально)

### 🧪 Тестирование

#### Unit-тесты

Геометрия и расчёты
Алгоритмы построения
Рендеринг и взаимодействие

#### Запуск тестов

dotnet test

## 🔐 Безопасность и производительность

Потокобезопасность
Минимизация аллокаций
Проверка входных данных
Логирование ошибок

## ⚠️ Важные уроки при разработке

### Критические ошибки, которые ломают платформу Tiger Trade

1. НЕ переопределять ControlPointEditing

- Этот метод управляет созданием и редактированием точек.
- Переопределение нарушает внутреннюю логику платформы.
- Ошибка остаётся даже после удаления DLL.

1. НЕ переопределять ControlPointsChanged

- Нарушает уведомления об изменении точек.
- Может вызвать бесконечные циклы обновления.

1. НЕ использовать Keyboard.IsKeyDown

- В контексте Tiger Trade работает некорректно.
- Использовать System.Windows.Forms.Control.ModifierKeys.

1. НЕ изменять ControlPoints в Prepare()

- Prepare() вызывается часто, изменение ControlPoints приводит к крашу.
- Использовать вспомогательные поля (_startScreen,_endScreen).

1. НЕ использовать System.Windows.Media

- В Tiger Trade используются свои типы цветов (XColor), а не WPF (Colors).

1. НЕ использовать OnPropertyChanged(nameof(...))

- В Tiger Trade работает OnPropertyChanged() (без параметров).

### Безопасный подход для добавления функциональности

✅ Использовать Prepare() для вычислений и сохранения в локальные поля.

✅ Использовать Draw() для отрисовки из локальных полей.

✅ Добавлять логирование для отладки.

✅ Всегда проверять длину ControlPoints перед использованием.

✅ Добавлять protected override int PenWidth => LineWidth;.

### Если платформа сломалась

1. Удалить библиотеку из всех папок:
rm -f "/c/Users/<User>/Documents/TigerTrade/Objects/*.dll"
rm -f "/c/Program Files/TigerTrade/Objects/*.dll"
2. Очистить кеш:
rm -rf /c/Users/<User>/AppData/Local/TigerTrade/Cache/*
rm -rf /c/Users/<User>/AppData/Local/TigerTrade/Temp/*
rm -rf /c/Users/<User>/AppData/Roaming/TigerTrade/Cache/*
3. Переустановить Tiger Trade (если ошибка осталась).

## 📄 Лицензия

Этот проект распространяется под лицензией MIT. См. файл LICENSE для получения дополнительной информации.

## 🤝 Участие в разработке

1. Форкните репозиторий
2. Создайте ветку для новой функциональности:
git checkout -b feature/amazing-feature
3. Зафиксируйте изменения:
git commit -m 'Add some amazing feature'
4. Отправьте изменения в репозиторий:
git push origin feature/amazing-feature
5. Откройте Pull Request

## 📞 Контакты

Разработчик: Ahim
Email: <n.bin.andr@gmail.com>

## 🙏 Благодарности

Tiger Trade Capital AG за предоставленный API
Сообществу Tiger Trade за обратную связь и тестирование

## 🚦 Статус проекта

<https://img.shields.io/badge/build-passing-brightgreen>
<https://img.shields.io/badge/tests-10%2520passed-blue>
<https://img.shields.io/badge/version-1.0.0--beta-orange>

В разработке. Планируется публикация через NuGet и GitHub Releases.
