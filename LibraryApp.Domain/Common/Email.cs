namespace LibraryApp.Domain.Common;

/// <summary>
/// Email value object'i
/// Value object'ler immutable (değiştirilemez) nesnelerdir
/// Bu sınıf, email adreslerinin doğruluğunu garanti eder
/// </summary>
public record Email
{
    /// <summary>
    /// Email adresi
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Email constructor'ı
    /// </summary>
    /// <param name="value">Email adresi</param>
    /// <exception cref="ArgumentException">Geçersiz email formatı</exception>
    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Email oluşturur ve doğrular
    /// </summary>
    /// <param name="email">Email adresi</param>
    /// <returns>Doğrulanmış Email value object'i</returns>
    /// <exception cref="ArgumentException">Geçersiz email formatı</exception>
    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty or null", nameof(email));

        // Email formatını doğrula
        if (!IsValidEmail(email))
            throw new ArgumentException($"Invalid email format: {email}", nameof(email));

        // Email'i küçük harfe çevir (normalize et)
        return new Email(email.ToLowerInvariant().Trim());
    }

    /// <summary>
    /// Email formatını doğrular
    /// </summary>
    /// <param name="email">Kontrol edilecek email</param>
    /// <returns>True eğer geçerli format</returns>
    private static bool IsValidEmail(string email)
    {
        try
        {
            // Basit email format kontrolü
            var trimmedEmail = email.Trim();
            
            // @ işareti olmalı
            if (!trimmedEmail.Contains('@'))
                return false;

            // @ işaretinden önce ve sonra karakter olmalı
            var parts = trimmedEmail.Split('@');
            if (parts.Length != 2)
                return false;

            if (string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
                return false;

            // Domain kısmında nokta olmalı
            if (!parts[1].Contains('.'))
                return false;

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// String'den Email'e implicit conversion
    /// </summary>
    /// <param name="email">Email string'i</param>
    /// <returns>Email value object'i</returns>
    public static implicit operator Email(string email) => Create(email);

    /// <summary>
    /// Email'den string'e implicit conversion
    /// </summary>
    /// <param name="email">Email value object'i</param>
    /// <returns>Email string'i</returns>
    public static implicit operator string(Email email) => email.Value;

    /// <summary>
    /// String representation
    /// </summary>
    /// <returns>Email adresi</returns>
    public override string ToString() => Value;
}
