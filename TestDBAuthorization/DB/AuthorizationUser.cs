using System.ComponentModel.DataAnnotations;

namespace TestDBAuthorization.DB
{
    public class AuthorizationUser
    {
        [Key]
        public  int AuthorizationUserId { get; set; }
        public string UserName { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public string Accounting { get; set; }  

        /// в консоль  диспетчера  пакетов  -> команды 
        /// add-migration  +  // -> имя миграции 
        /// update-database // 
        /// ->  в  бд  должна создаться таблица 
    }
}