using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace MSSPVirusSignatureDatabase.Models.Mapping
{
    public class VIRUS_SIGNATUREMap : EntityTypeConfiguration<VIRUS_SIGNATURE>
    {
        public VIRUS_SIGNATUREMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SIGNATURE_ID, t.SIGNATURE_LOCATION, t.SIGNATURE_STRING, t.SIGNATURE_NAME });

            // Properties
            this.Property(t => t.SIGNATURE_ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.SIGNATURE_LOCATION)
                .IsRequired();

            this.Property(t => t.SIGNATURE_STRING)
                .IsRequired();

            this.Property(t => t.SIGNATURE_NAME)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("VIRUS_SIGNATURE");
            this.Property(t => t.SIGNATURE_ID).HasColumnName("SIGNATURE_ID");
            this.Property(t => t.SIGNATURE_LOCATION).HasColumnName("SIGNATURE_LOCATION");
            this.Property(t => t.SIGNATURE_STRING).HasColumnName("SIGNATURE_STRING");
            this.Property(t => t.SIGNATURE_NAME).HasColumnName("SIGNATURE_NAME");
        }
    }
}
