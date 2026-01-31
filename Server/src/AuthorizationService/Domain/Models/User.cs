using Domain.Enums;

namespace Domain.Models
{
    public class User : BaseModel
    {
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? AvatarUrl { get; private set; }

        public AuthProvider Provider { get; private set; }
        public string ProviderId { get; private set; }

        public Role Role { get; private set; }
        public string? PasswordHash { get; private set; }

        protected User() { }

        private User(string email, string firstName, string lastName, string? avatarUrl, AuthProvider provider, string providerId, Role initialRole, string passwordHash)
        {
            if (initialRole == Role.Admin)
                throw new ArgumentException("Admin role cannot be assigned on registration");

            Validate(email, firstName, lastName, avatarUrl, providerId, provider, passwordHash);

            Email = email;
            FirstName = firstName;
            LastName = lastName;
            AvatarUrl = avatarUrl;
            Provider = provider;
            ProviderId = providerId;

            Role = initialRole;
            PasswordHash = provider == AuthProvider.Local ? passwordHash : null;

            Touch();
        }

        public void PromoteToAdmin()
        {
            if (Role == Role.Admin)
                return;

            Role = Role.Admin;
            Touch();
        }

        private static void Validate(string email, string firstName, string lastName, string? avatarUrl, string providerId, AuthProvider provider, string? passwordHash)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (!email.Contains("@"))
                throw new ArgumentException("Invalid email format", nameof(email));

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException(nameof(lastName));

            if (provider == AuthProvider.Google &&
                string.IsNullOrWhiteSpace(avatarUrl))
                throw new ArgumentNullException(nameof(avatarUrl));

            if (string.IsNullOrWhiteSpace(providerId))
                throw new ArgumentNullException(nameof(providerId));

            if (provider == AuthProvider.Local && string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password is required for local users");
        }

        public static User CreateLocal(string email, string firstName, string lastName, string passwordHash, Role role)
        {
            return new User(
                email,
                firstName,
                lastName,
                avatarUrl: null,
                AuthProvider.Local,
                providerId: email,
                role,
                passwordHash
            );
        }

        public static User CreateGoogle(string email, string firstName, string lastName, string avatarUrl, string googleSub, Role role)
        {
            return new User(
                email,
                firstName,
                lastName,
                avatarUrl,
                AuthProvider.Google,
                googleSub,
                role,
                passwordHash: null
            );
        }
    }
}
