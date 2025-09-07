# Архитектура SMS Service API

## 🏗️ Обзор архитектуры

Проект использует **многослойную архитектуру** с элементами **Clean Architecture** и **Vertical Slices**.

Presentation Layer (API Endpoints)  
↓  
Application Layer (Services, Validators)  
↓  
Domain Layer (Models, Interfaces)  
↓  
Infrastructure Layer (Implementations)

## 🎯 Цели архитектуры

- **Тестируемость** - изоляция бизнес-логики
    
- **Гибкость** - легкая замена реализаций
    
- **Масштабируемость** - подготовка к росту
    
- **Поддержка** - четкое разделение ответственности
    

## 📁 Структура проекта

### SmsService.Api/

├── Contracts/           # DTO модели
│   └── Requests/
├── Services/           # Сервисы приложения
│   └── RateLimiter/
├── Validators/         # Валидаторы
├── Config/            # Конфигурация
├── Endpoints/         # Обработчики эндпоинтов
└── Program.cs         # Точка входа


### SmsService.UnitTests/

├── Validators/         # Тесты валидаторов
├── Services/          # Тесты сервисов
├── Integration/       # Интеграционные тесты
└── TestWebApplicationFactory.cs

## 🔧 Ключевые компоненты

### 1. Rate Limiting System

- **IRateLimiter** - абстракция ограничителя запросов
    
- **InMemoryRateLimiter** - in-memory реализация
    
- **RateLimiterService** - фасад для бизнес-логики
    

### 2. Validation Layer

- **FluentValidation** - валидация входных данных
    
- **SendSmsRequestValidator** - правила валидации запросов
    

### 3. API Layer

- **Minimal API** - эндпоинты
    
- **SmsHandler** - обработчики запросов
    

## 🚀 Flow обработки запроса

1. **HTTP Request** → POST /api/sms/send
    
2. **Validation** → FluentValidation
    
3. **Rate Limit Check** → RateLimiterService
    
4. **Business Logic** → обработка и лимиты
    
5. **HTTP Response** → результат или ошибка
    

## 📦 Зависимости

### Основные пакеты:

- `ASP.NET Core 8.0` - веб-фреймворк
    
- `FluentValidation` - валидация данных
    
- `Swashbuckle` - Swagger документация
    
- `xUnit` - тестирование
    

## 🔮 Будущее расширение

### Планируемые улучшения:

- **RedisRateLimiter** - распределенный лимитер
    
- **Database Storage** - персистентное хранение
    
- **Background Services** - ежедневный сброс лимитов
    
- **Metrics** - Prometheus метрики
    
- **Caching** - кэширование запросов

## 🔄 Deployment

### Текущая конфигурация:

- **Single instance** - in-memory лимитирование
    
- **Docker support** - контейнеризация
    
- **Health checks** - мониторинг состояния
    

### Целевая конфигурация:

- **Multi-instance** с Redis
    
- **Kubernetes** оркестрация
    
- **Horizontal scaling** - горизонтальное масштабирование

## 👥 Ответственность компонентов

|Компонент|Ответственность|
|---|---|
|`SmsHandler`|Обработка HTTP запросов|
|`RateLimiterService`|Бизнес-логика лимитов|
|`InMemoryRateLimiter`|Хранение счетчиков|
|`SendSmsRequestValidator`|Валидация входных данных|

## 📝 Принятые решения

### 1. InMemory вместо Redis

**Решение**: Начать с простой реализации для MVP  
**Причина**: Быстрый старт, простота разработки  
**План**: Легкая миграция на Redis позже

### 2. Minimal API вместо Controllers

**Решение**: Использовать Minimal API  
**Причина**: Меньше boilerplate кода  
**Преимущество**: Простота и производительность

### 3. Vertical Slices организация

**Решение**: Группировка по функциональности  
**Причина**: Лучшая поддерживаемость  
**Результат**: Каждая функция - самостоятельный модуль