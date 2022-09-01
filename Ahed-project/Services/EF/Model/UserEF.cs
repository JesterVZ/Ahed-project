using System.ComponentModel.DataAnnotations;

namespace Ahed_project.Services.EF.Model
{
    /// <summary>
    /// Пользователь для БД
    /// </summary>
    public class UserEF
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Email/логин
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Зайдено или нет
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Id последнего проекта
        /// </summary>
        public int? LastProjectId { get; set; }

        /// <summary>
        /// Последний рассчет пользователя
        /// </summary>
        public int? LastCalculationId { get; set; }
    }
}
