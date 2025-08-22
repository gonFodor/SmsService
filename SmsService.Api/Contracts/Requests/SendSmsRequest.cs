namespace SmsService.Api.Contracts.Requests
{
    /// <summary>
    /// Запрос на отправку SMS сообщения
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя для отслеживания лимитов</param>
    /// <param name="message">Текст сообщения для отправки. Должен быть не пустым и не превышать 500 символов</param>
    /// <param name="phoneNumbers">Массив телефонных номеров в международном формате E.164 для отправки сообщения. Не может быть пустым</param>
    /// <example>
    /// Пример валидного запроса:
    /// <code>
    /// {
    ///     "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///     "message": "Ваш код подтверждения: 123456",
    ///     "phoneNumbers": ["+79001234567", "+380441234567"]
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Каждый номер телефона в массиве считается за одно SMS сообщение
    /// при расчете дневного лимита пользователя
    /// </remarks>
    public record SendSmsRequest(Guid userId, string message, string[] phoneNumbers);
}