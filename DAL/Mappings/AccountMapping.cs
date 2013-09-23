namespace Boilerplate.Mappings {
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Boilerplate.Domain;

    public class AccountMapping : EntityTypeConfiguration<Account> {
        public AccountMapping() {
            ToTable("Accounts");

            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.DateCreated)
                .IsRequired();
            Property(x => x.IsActive)
                .IsRequired();
            Property(x => x.Key)
                .IsRequired()
                .IsUnicode()
                .IsFixedLength()
                .HasMaxLength(32);
            Property(x => x.Name)
                .IsOptional()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(128);
        }
    }
}
