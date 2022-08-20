using Ahed_project.Services.EF.Configuration;
using Ahed_project.Services.EF.Model;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;

namespace Ahed_project.Services.EF
{
    public class EFContext : DbContext
    {
        public EFContext()
        {

        }

        /// <summary>
        /// Конфигуратор бд
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UsersConfguration());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            optionsBuilder.UseSqlite($"DataSource ={Path.GetDirectoryName(assembly.Location)}\\Services\\EF\\DB\\AHED.db");
        }

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            return result;
        }

        /// <summary>
        /// Пользователи
        /// </summary>
        public DbSet<UserEF> Users { get; set; }
    }
}
