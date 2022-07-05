using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        /// <summary>
        /// id
        /// </summary>
        public int user_id { get; set; }
        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// TO DO понять зачем
        /// </summary>
        public int rool_id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Занимаемая должность
        /// </summary>
        public string post { get; set; }
        /// <summary>
        /// TO DO понять зачем
        /// </summary>
        public int iat { get; set; }
        /// <summary>
        /// TO DO понять зачем
        /// </summary>
        public int exp { get; set; }

    }
}
