using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MSSPVirusSignatureDatabase.Models.Mapping;

namespace MSSPVirusSignatureDatabase.Models
{
    /// <summary>
    /// Class to establish a 'context' - an encapsulated
    /// connection to the database for the application
    /// </summary>
    public class VirusSignatureContext : DbContext
    {
        /// <summary>
        /// Class constructor, setup an initialiser against the database
        /// </summary>
        static VirusSignatureContext()
        {
            Database.SetInitializer<VirusSignatureContext>(null);
        }

        /// <summary>
        /// Empty constructor - seems to want one
        /// </summary>
        public VirusSignatureContext()
        {

        }

        public DbSet<VIRUS_SIGNATURE> VIRUS_SIGNATURE { get; set; }

        /// <summary>
        /// Add the mapping to the model builder, so that data 
        /// from the database is translatable into the .NET objects
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new VIRUS_SIGNATUREMap());
        }
    }
}
