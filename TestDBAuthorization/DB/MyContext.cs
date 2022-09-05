using Microsoft.EntityFrameworkCore;


namespace TestDBAuthorization.DB
{
    public class MyContext :DbContext
    {
        private string conectString =
    "server=192.168.10.160;" +
            "database=Ahtmov_IS_20_03_05_09;" + // база у
            //всех своя 
            " user id=stud; password=stud";

        protected override 
            void OnConfiguring(DbContextOptionsBuilder 
            optionsBuilder)
        {
            optionsBuilder.UseSqlServer(conectString); 
            // паттерн
        }


       public DbSet<AuthorizationUser>
            Authorizations { get; set; }

    }
}
