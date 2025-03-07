using Microsoft.EntityFrameworkCore;

namespace Dodem_Zadanie.Entities
{
    public class DodemDbContext: DbContext
    {
        public DodemDbContext(DbContextOptions<DodemDbContext> options): base(options) { }

        public DbSet<MailTemplate> MailTemplates { get; set; }
    }
}
