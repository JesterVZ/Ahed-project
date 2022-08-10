using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
