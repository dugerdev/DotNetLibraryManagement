namespace LibraryApp.Domain.Common;

/// <summary>
/// Para miktarı value object'i
/// Bu sınıf, para miktarlarının doğruluğunu garanti eder
/// Ceza hesaplamaları ve para işlemleri için kullanılır
/// </summary>
public record Money
{
    /// <summary>
    /// Para miktarı (TL cinsinden)
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Para birimi (varsayılan: TL)
    /// </summary>
    public string Currency { get; }

    /// <summary>
    /// Money constructor'ı
    /// </summary>
    /// <param name="amount">Para miktarı</param>
    /// <param name="currency">Para birimi</param>
    private Money(decimal amount, string currency = "TL")
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Money oluşturur ve doğrular
    /// </summary>
    /// <param name="amount">Para miktarı</param>
    /// <param name="currency">Para birimi (opsiyonel, varsayılan: TL)</param>
    /// <returns>Doğrulanmış Money value object'i</returns>
    /// <exception cref="ArgumentException">Geçersiz para miktarı</exception>
    public static Money Create(decimal amount, string currency = "TL")
    {
        // Negatif para miktarı kontrolü
        if (amount < 0)
            throw new ArgumentException("Money amount cannot be negative", nameof(amount));

        // Para birimi kontrolü
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty or null", nameof(currency));

        // Para miktarını 2 ondalık basamağa yuvarla
        var roundedAmount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);

        return new Money(roundedAmount, currency.ToUpperInvariant());
    }

    /// <summary>
    /// Sıfır para miktarı oluşturur
    /// </summary>
    /// <param name="currency">Para birimi (opsiyonel, varsayılan: TL)</param>
    /// <returns>Sıfır Money value object'i</returns>
    public static Money Zero(string currency = "TL") => Create(0, currency);

    /// <summary>
    /// İki para miktarını toplar
    /// </summary>
    /// <param name="other">Toplanacak para miktarı</param>
    /// <returns>Toplam para miktarı</returns>
    /// <exception cref="InvalidOperationException">Farklı para birimleri</exception>
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException($"Cannot add different currencies: {Currency} and {other.Currency}");

        return Create(Amount + other.Amount, Currency);
    }

    /// <summary>
    /// İki para miktarını çıkarır
    /// </summary>
    /// <param name="other">Çıkarılacak para miktarı</param>
    /// <returns>Fark para miktarı</returns>
    /// <exception cref="InvalidOperationException">Farklı para birimleri</exception>
    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException($"Cannot subtract different currencies: {Currency} and {other.Currency}");

        return Create(Amount - other.Amount, Currency);
    }

    /// <summary>
    /// Para miktarını bir sayı ile çarpar
    /// </summary>
    /// <param name="multiplier">Çarpan</param>
    /// <returns>Çarpım sonucu para miktarı</returns>
    public Money Multiply(decimal multiplier)
    {
        return Create(Amount * multiplier, Currency);
    }

    /// <summary>
    /// Para miktarını bir sayı ile böler
    /// </summary>
    /// <param name="divisor">Bölen</param>
    /// <returns>Bölüm sonucu para miktarı</returns>
    /// <exception cref="ArgumentException">Sıfıra bölme</exception>
    public Money Divide(decimal divisor)
    {
        if (divisor == 0)
            throw new ArgumentException("Cannot divide by zero", nameof(divisor));

        return Create(Amount / divisor, Currency);
    }

    /// <summary>
    /// Para miktarının sıfır olup olmadığını kontrol eder
    /// </summary>
    /// <returns>True eğer sıfırsa</returns>
    public bool IsZero() => Amount == 0;

    /// <summary>
    /// Para miktarının pozitif olup olmadığını kontrol eder
    /// </summary>
    /// <returns>True eğer pozitifse</returns>
    public bool IsPositive() => Amount > 0;

    /// <summary>
    /// Para miktarının negatif olup olmadığını kontrol eder
    /// </summary>
    /// <returns>True eğer negatifse</returns>
    public bool IsNegative() => Amount < 0;

    /// <summary>
    /// Decimal'den Money'e implicit conversion
    /// </summary>
    /// <param name="amount">Para miktarı</param>
    /// <returns>Money value object'i</returns>
    public static implicit operator Money(decimal amount) => Create(amount);

    /// <summary>
    /// Money'den decimal'e implicit conversion
    /// </summary>
    /// <param name="money">Money value object'i</param>
    /// <returns>Para miktarı</returns>
    public static implicit operator decimal(Money money) => money.Amount;

    /// <summary>
    /// String representation
    /// </summary>
    /// <returns>Formatlanmış para miktarı</returns>
    public override string ToString() => $"{Amount:F2} {Currency}";
}
