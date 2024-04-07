using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

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
        /// Токен
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Идентификатор пользователя Ahead
        /// </summary>
        public long UserId { get; set; }

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
        /// <summary>
        /// Последняя геометрия пользователя
        /// </summary>
        public int? LastGeometryId { get; set; }
    }
}
