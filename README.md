
# VZTest

VZTest - сайт написанный на ASP.NET и предлагающий фукнционал для проведения тестирований в онлайн формате.

При создании теста на данный момент можно сделать несколько типов вопросов:

- Текстовый (Ответ предоставлятьется в свободной форме в виде строки(или нескольких строк) текста. Будьте аккуратны с этим типом вопросов, лучше используйте другие типы)
- Числовой (Просто число, без дробной части)
- Значение с плавающей запятой (Десятичные дроби)
- Дата (Дата в формате дд.мм.гггг (Не знаю зачем, но может кому-нибудь понадобится))
- Радио-кнопки (Несколько вариантов ответа, только один правильный)
- Флажки (Несколько вариантов ответа, один или несколько правильные)

# В разработке

- Прохождение тестов
  - [x] Создание попытки
  - [x] Сохранение данных
  - [x] Проверка попытки
  - [ ] Установка ограничения на ввод
  - [ ] Добавить отслеживание вставки, так как она может содержать ненужные символы
  - [ ] Ограниченное время
  - [ ] Отображение картинок
  - [ ] Разделение на страницы и отображение сохранённых/несохранённых ответов

- Просмотр результатов
  - [x] Проверка только 1 раз, после прохождение, а не каждый раз при обращении к Result
  - [x] Просмотр содержания попыток создателем теста
  - [ ] Возможность сортировки попыток пользователей создателем теста
  - [ ] Возможность поиска попыток пользователей создателем теста
  - [ ] Возможность получения попыток пользователей в формате xlsx
  - [ ] Возможность отображений только лучших попыток пользователей создателем теста
  
- Создание тестов
  - [x] Добавление основной информации (Название, описание и тд.)
  - [x] Добавление текстовых, числовых, дробных и дата-вопросов
  - [x] Указание правльных ответов и баллов
  - [x] Создание радио и флажковых вопросов
  - [x] Обработка ошибок перед отправкой
  - [ ] Добавление картинки к тесту (Ссылка на картинку. У хостинга крайне мало места)
  - [ ] Добавление картинки к вопросу (Аналогично)
  
- Редактирование тестов
  - [x] Редактирование статуса (Открыт/Закрыт)
  - [x] Редактирование статуса (Публичный/Приватный)
  - [x] Редактирование названия, описания, пароля и тд.
  - [x] Редактирование названий вопросов и названий опций
  - [x] Редактирование баллов за вопрос и проследующей перепроверкой результатов
  - [x] Редактирование правильных ответов
  - [x] Добавление вопросов
  - [x] Удаление вопросов
  - [x] Добавление опций
  - [x] Удаление опций


- Дополнительно
  - [ ] Жалобы на тесты
  - [ ] Запрет на прохождение тестов для пользователей
