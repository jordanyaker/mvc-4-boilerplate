namespace Boilerplate.Test.EntityFramework {
    using System;
    using System.Linq;
    using Boilerplate.Domain;
    using System.Collections.Generic;

    /// <summary>
    /// A utility class for generating data to be stored in an EntityFramework <see cref="T:System.Data.Objects.ObjectContext"/>.
    /// </summary>
    public class EFTestDataActions {
        // -------------------------------------------------------------------------------------
        // Constructors
        // -------------------------------------------------------------------------------------
        /// <summary>
        /// The default constructor for the <see cref="T:Supportify.Test.EntityFramework.EFTestDataActions"/> instance.
        /// </summary>
        /// <param name="generator">The <see cref="T:Supportify.Test.EntityFramework.EFTestData"/> instance that commands will be executed against.</param>
        public EFTestDataActions(EFTestDataFactory generator) {
            _generator = generator;
        }

        // -------------------------------------------------------------------------------------
        // Fields
        // -------------------------------------------------------------------------------------
        readonly EFTestDataFactory _generator;
        readonly Random _random = new Random();

        // -------------------------------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes and returns a new <see cref="T:Supportify.Domain.Account"/> instance and adds it to the current EntityFramework <see cref="T:System.Data.Objects.ObjectContext"/>.
        /// </summary>
        /// <returns>The initialized and tracked <see cref="T:Supportify.Domain.Account"/> instance.</returns>
        public Account CreateAccount() {
            return CreateAccount(x => { });
        }
        /// <summary>
        /// Initializes and returns a new <see cref="T:Supportify.Domain.Account"/> instance and adds it to the current EntityFramework <see cref="T:System.Data.Objects.ObjectContext"/>.
        /// </summary>
        /// <param name="customize">Custom actions that will be executed for modifying the <see cref="T:Supportify.Domain.Account"/> instance.</param>
        /// <returns>The initialized and tracked <see cref="T:Supportify.Domain.Account"/> instance.</returns>
        public Account CreateAccount(Action<Account> customize) {
            var account = new Account();
            customize(account);

            if (string.IsNullOrWhiteSpace(account.Name)) {
                account.Name = "Company " + RandomString();
            }

            _generator.Context.Set<Account>().Add(account);

            return account;
        }
        /// <summary>
        /// Initializes and returns a new <see cref="T:Supportify.Domain.User"/> instance and adds it to the current EntityFramework <see cref="T:System.Data.Objects.ObjectContext"/>.
        /// </summary>
        /// <returns>The initialized and tracked <see cref="T:Supportify.Domain.User"/> instance.</returns>
        public User CreateUser() {
            return CreateUser(x => { });
        }
        /// <summary>
        /// Initializes and returns a new <see cref="T:Supportify.Domain.User"/> instance and adds it to the current EntityFramework <see cref="T:System.Data.Objects.ObjectContext"/>.
        /// </summary>
        /// <param name="customize">Custom actions that will be executed for modifying the <see cref="T:Supportify.Domain.User"/> instance.</param>
        /// <returns>The initialized and tracked <see cref="T:Supportify.Domain.User"/> instance.</returns>
        public User CreateUser(Action<User> customize) {
            var user = new User();
            customize(user);

            if (user.WithAccount == null) {
                user.WithAccount = CreateAccount();
            }
            if (string.IsNullOrWhiteSpace(user.Name)) {
                user.Name = "User " + RandomString();
            }
            if (string.IsNullOrWhiteSpace(user.Email)) {
                user.Email = "name" + RandomString() + "@company.com";
            }
            user.Email = user.Email.ToLower();
            if (string.IsNullOrWhiteSpace(user.PasswordValue)) {
                user.SetPassword(GetSumIpsum(8, 16));
            }

            _generator.Context.Set<User>().Add(user);

            return user;
        }

        /// <summary>
        /// Generates a random number using the supplied parameters.
        /// </summary>
        /// <param name="minValue">The minimum value possible for the random value.</param>
        /// <param name="maxValue">The maximum value possible for the random value.</param>
        /// <returns>A randomly generated number between the max and min values specified.</returns>
        int RandomNumber(int minValue = 0, int maxValue = int.MaxValue) {
            return _random.Next(minValue, maxValue);
        }
        /// <summary>
        /// Generates a random numerical string.
        /// </summary>
        /// <returns>A random numeric string between 0 and Int.Max.</returns>
        string RandomString() {
            return _random.Next(maxValue: int.MaxValue).ToString();
        }
        /// <summary>
        /// Generates a random Lorem Ipsum string of an optionally specified length.
        /// </summary>
        /// <param name="minLength">The minimum length possible for the string.</param>
        /// <param name="maxLength">The maximum allowed length for the string.</param>
        /// <returns>A random string between the min and max lengths specified.</returns>
        string GetSumIpsum(int minLength = 0, int maxLength = 2825) {
            if (minLength < 0) {
                throw new ArgumentException("The minimum length of the string cannot be less than zero.");
            }

            string ipsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla faucibus iaculis erat ac luctus. Integer sit amet magna tortor, vel adipiscing lectus. Duis nec justo leo. In hac habitasse platea dictumst. Ut orci felis, bibendum vel imperdiet at, condimentum eu sem. Nunc dignissim porta velit sed congue. Integer vel lectus sed ante gravida consequat. Pellentesque quis pulvinar mauris. Sed interdum, nunc et faucibus tincidunt, odio elit pellentesque augue, vel scelerisque orci nisl non lectus.\r\n\r\nCurabitur nec arcu id elit aliquet tempor eget sed odio. Etiam id pharetra odio. Duis tortor nunc, vulputate vel dictum vel, porta id enim. Vivamus sit amet nibh vel mauris placerat rutrum. Nunc iaculis ullamcorper turpis, vitae accumsan nulla ultrices placerat. Fusce felis mauris, congue a mattis et, sagittis et lorem. Curabitur fringilla, velit et convallis vulputate, elit libero mattis justo, vel venenatis augue turpis et sem. Proin pulvinar, elit sed posuere ullamcorper, sapien nisl interdum dui, id interdum lectus nisl ut elit. Maecenas at magna dolor, vitae vehicula dolor. Ut tristique sapien quis sem aliquam scelerisque. Nam eu dui mi. Nullam tempus vulputate leo, a tincidunt erat mollis eget. Aenean sed magna nisl.\r\n\r\nDuis adipiscing suscipit elit, sed interdum magna lacinia sit amet. Nulla facilisi. Nam gravida est adipiscing diam eleifend bibendum. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Aenean dignissim sapien a massa mollis tempor. Vivamus nec velit neque. Ut vulputate porttitor diam id ultrices. Pellentesque sodales tempor diam, sed laoreet arcu feugiat id. Etiam pellentesque placerat mattis. Quisque lorem massa, congue sed feugiat non, auctor scelerisque sapien.\r\n\r\nSuspendisse sapien mi, ultricies quis aliquet et, cursus non mauris. Duis quis porta urna. Donec orci orci, tincidunt vel lobortis aliquet, convallis non nibh. Nam ullamcorper, purus aliquet sollicitudin feugiat, quam eros sollicitudin nibh, eu pretium risus odio eu risus. Aliquam tincidunt tincidunt viverra. Donec porta nisi sed neque rhoncus vehicula. Suspendisse laoreet dignissim est a mollis. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla pharetra scelerisque nisi quis semper. Fusce nisl magna, mollis quis viverra suscipit, volutpat sed lorem. Morbi sit amet lorem ac mauris viverra accumsan. Morbi ut nulla ante. Mauris varius, augue ut tincidunt hendrerit, mauris sapien auctor tortor, egestas fringilla nisi mi eget nunc.\r\nSuspendisse ullamcorper adipiscing neque, nec auctor neque pulvinar sit amet. Duis at porttitor diam. Donec ornare adipiscing mauris, in sodales dui dapibus ut. Sed ut nulla arcu, volutpat laoreet libero. Nullam aliquet dapibus ligula. Aenean fringilla congue magna vitae tristique. Curabitur ut porttitor leo. Sed in arcu odio.";
            int length = RandomNumber(minLength, maxLength);
            return ipsum.Substring(0, length);
        }
    }
}