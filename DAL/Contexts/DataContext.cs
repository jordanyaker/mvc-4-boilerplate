namespace Boilerplate.Contexts {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Data.Objects;
    using System.Linq;
    using System.Reflection;
    using StackExchange.Profiling;
    using Polly;
    using System.Data.SqlClient;

    /// <summary>
    /// A specialized data context class to retrieve the domain objects for the OTD project.
    /// </summary>
    public class DataContext : DbContext {
        // -------------------------------------------------------------------------------------
        // Constructors
        // -------------------------------------------------------------------------------------
        /// <summary>
        /// Constructs a new <see cref="T:Supportify.Contexts.DataContext"/> instance using the given string as the name or connection string for the database to which a connection will be made.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        public DataContext(string nameOrConnectionString)
            : base(nameOrConnectionString) {
        }

        // -------------------------------------------------------------------------------------
        // Constants
        // -------------------------------------------------------------------------------------
        static Type EntityType = typeof(EntityTypeConfiguration<>);
        static Type ComplexType = typeof(ComplexTypeConfiguration<>);

        // -------------------------------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------------------------------
        /// <summary>
        /// Checks if a supplied type is one of the allowed mapping configuration types.
        /// </summary>
        /// <param name="matchingType">The <see cref="T:System.Type"/> to be examined.</param>
        /// <returns>True if the supplied type is inherited from <see cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<>"/> or  <see cref="System.Data.Entity.ModelConfiguration.ComplexTypeConfiguration<>"/>; otherwise false.</returns>
        private static bool IsMappingType(Type matchingType) {
            if (!matchingType.IsClass || matchingType.IsAbstract) {
                return false;
            }

            Type temp;

            return IsMatching(matchingType, out temp, t =>
                EntityType.IsAssignableFrom(t) || ComplexType.IsAssignableFrom(t));
        }
        /// <summary>
        /// Checks if a supplied type matches a supplied predicate matcher.
        /// </summary>
        /// <param name="matchingType">The <see cref="T:System.Type"/> to be examined.</param>
        /// <param name="modelType">The type of the model that matches the predicate condition.</param>
        /// <param name="matcher">A <see cref="T:System.Predicate<>"/> that provides the matching conditions for examination.</param>
        /// <returns>True if the supplied <see cref="T:System.Type"/> matches the supplied predicate conditions; otherwise false.</returns>
        private static bool IsMatching(Type matchingType, out Type modelType, Predicate<Type> matcher) {
            modelType = null;

            while (matchingType != null) {
                if (matchingType.IsGenericType) {
                    var definationType = matchingType.GetGenericTypeDefinition();

                    if (matcher(definationType)) {
                        modelType = matchingType.GetGenericArguments().First();
                        return true;
                    }
                }

                matchingType = matchingType.BaseType;
            }

            return false;
        }
        /// <summary>
        /// Called when the model for a derived context has been initialized, but before the model has been locked down and used to initialize the context.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            var addMethods = typeof(ConfigurationRegistrar).GetMethods()
                .Where(m => m.Name.Equals("Add"))
                .ToList();

            var entityTypeMethod = addMethods.First(m =>
                m.GetParameters()
                 .First()
                 .ParameterType
                 .GetGenericTypeDefinition()
                 .IsAssignableFrom(EntityType));

            var complexTypeMethod = addMethods.First(m =>
                m.GetParameters().First()
                 .ParameterType
                 .GetGenericTypeDefinition()
                 .IsAssignableFrom(ComplexType));

            var types = typeof(DataContext).Assembly
                .GetExportedTypes()
                .Where(IsMappingType)
                .ToList();

            foreach (var type in types) {
                MethodInfo typedMethod;
                Type modelType;

                if (IsMatching(type, out modelType, t => EntityType.IsAssignableFrom(t))) {
                    typedMethod = entityTypeMethod.MakeGenericMethod(modelType);
                } else if (IsMatching(type, out modelType, t => ComplexType.IsAssignableFrom(t))) {
                    typedMethod = complexTypeMethod.MakeGenericMethod(modelType);
                } else {
                    continue;
                }

                typedMethod.Invoke(modelBuilder.Configurations, new[] { Activator.CreateInstance(type) });
            }
        }

        public override int SaveChanges() {
            return Policy
                .Handle<SqlException>(ex =>
                    ex.Number == -2 ||
                    ex.Number == 20 ||
                    ex.Number == 64 ||
                    ex.Number == 233 ||
                    ex.Number == 10053 ||
                    ex.Number == 10054 ||
                    ex.Number == 10060 ||
                    ex.Number == 40143 ||
                    ex.Number == 40197 ||
                    ex.Number == 40501 ||
                    ex.Number == 40613)
                .Retry(3)
                .Execute(() => {
                    return base.SaveChanges();
                });
        }
    }
}