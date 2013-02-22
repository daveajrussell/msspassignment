using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MSSPVirusSignatureDatabase.Models.Mapping;

namespace MSSPVirusSignatureDatabase.Models
{
    public class VirusSignatureContext : DbContext
    {
        static VirusSignatureContext()
        {
            Database.SetInitializer<VirusSignatureContext>(null);
        }

		public VirusSignatureContext()
            : base("Name=VirusSignatureContext")
		{
		}

        public DbSet<VIRUS_SIGNATURE> VIRUS_SIGNATURE { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new VIRUS_SIGNATUREMap());
        }
    }
}
