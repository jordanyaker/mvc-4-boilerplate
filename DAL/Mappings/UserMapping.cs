namespace Boilerplate.Mappings {
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Boilerplate.Domain;

    public class UserMapping : EntityTypeConfiguration<User> {
        public UserMapping() {
            ToTable("Users");

            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.AccountId)
                .IsRequired();
            Property(x => x.DateCreated)
                .IsRequired();
            Property(x => x.IsActive)
                .IsRequired();
            Property(x => x.Email)
                .IsRequired()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(254);
            Property(x => x.Name)
                .IsOptional()
                .IsUnicode()
                .IsVariableLength()
                .IsMaxLength();
            Property(x => x.PasswordSalt)
                .IsUnicode()
                .IsVariableLength()
                .IsRequired()
                .HasMaxLength(64);
            Property(x => x.PasswordValue)
                .IsUnicode()
                .IsVariableLength()
                .IsRequired()
                .HasMaxLength(64);

            HasRequired(x => x.WithAccount)
                .WithMany()
                .HasForeignKey(x => x.AccountId)
                .WillCascadeOnDelete(false);
        }
    }
}