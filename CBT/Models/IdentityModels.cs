using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CBT.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public string Surname { get; set; }

        public ICollection<Event> CreatedEvents { get; set; }
        public ICollection<EventUserRelationship> EventUsersSigned { get; set; }

        public ApplicationUser()
        {
            this.EventUsersSigned = new List<EventUserRelationship>();
            this.CreatedEvents = new List<Event>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<EventUserRelationship> EventUserRelationship { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Field> Fields { get; set; }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Event>()
              .HasRequired<ApplicationUser>(s => s.UserCreator)
              .WithMany(g => g.CreatedEvents)
              .WillCascadeOnDelete(false);

            builder.Entity<EventUserRelationship>()
               .HasKey(t => new
               {
                   t.EventId,
                   t.UserId
               });

            builder.Entity<EventUserRelationship>()
                .HasRequired(sc => sc.Event)
                .WithMany(s => s.EventUsersSigned)
                .HasForeignKey(sc => sc.EventId);

            builder.Entity<EventUserRelationship>()
                .HasRequired(sc => sc.User)
                .WithMany(c => c.EventUsersSigned)
                .HasForeignKey(sc => sc.UserId);
        }
    }
}