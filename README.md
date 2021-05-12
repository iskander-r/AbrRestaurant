# AbrRestaurant
Abr Restaurant Menu API

API для создания заказов в ресторане, поддерживает аутентификацию через JWT Bearer Token (aka ROPC)
Используется ASP.NET 5, EF 5 и PostgreSQL. Написано с использованием Clean Architecture

Полная документация к проекту в виде Swagger Open API доступна по http://localhost:5000/swagger
Также, при запуске проекта в docker-контейнере поднимется seq-интерфейс для просмотра логов. Он доступен по http://localhost:5340

Шаги для запуска и тестирования проекта:
- Склонировать проект к себе на рабочую машину
- Убедиться, что у вас установлен docker 3.5+
- В корневой папке проекта выполнить следующие скрипты:
  - docker-compose build
  - docker-compose up
  - swagger-api: http://localhost:5000/swagger, seq-interface: http://localhost:5340
  
Примеры интеграционных тестов находятся в проекте AbrRestaurant.MenuApi.IntegrationTests
