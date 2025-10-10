namespace LibraryApp.Domain.Common;

/// <summary>
/// ISBN (International Standard Book Number) value object'i
/// ISBN, kitapların benzersiz tanımlayıcısıdır
/// Bu sınıf, ISBN formatını doğrular ve normalize eder
/// </summary>
public record ISBN
{
    /// <summary>
    /// ISBN değeri
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// ISBN constructor'ı
    /// </summary>
    /// <param name="value">ISBN değeri</param>
    private ISBN(string value)
    {
        Value = value;
    }

    /// <summary>
    /// ISBN oluşturur ve doğrular
    /// </summary>
    /// <param name="isbn">ISBN string'i</param>
    /// <returns>Doğrulanmış ISBN value object'i</returns>
    /// <exception cref="ArgumentException">Geçersiz ISBN formatı</exception>
    public static ISBN Create(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN cannot be empty or null", nameof(isbn));

        // ISBN'i temizle (tire, boşluk vb. kaldır)
        var cleanedIsbn = CleanISBN(isbn);

        // ISBN formatını doğrula
        if (!IsValidISBN(cleanedIsbn))
            throw new ArgumentException($"Invalid ISBN format: {isbn}", nameof(isbn));

        return new ISBN(cleanedIsbn);
    }

    /// <summary>
    /// ISBN'i temizler (tire, boşluk vb. kaldırır)
    /// </summary>
    /// <param name="isbn">Ham ISBN</param>
    /// <returns>Temizlenmiş ISBN</returns>
    private static string CleanISBN(string isbn)
    {
        return isbn.Replace("-", "").Replace(" ", "").Trim();
    }

    /// <summary>
    /// ISBN formatını doğrular
    /// ISBN-10 veya ISBN-13 formatını destekler
    /// </summary>
    /// <param name="isbn">Kontrol edilecek ISBN</param>
    /// <returns>True eğer geçerli format</returns>
    private static bool IsValidISBN(string isbn)
    {
        // Sadece rakam ve X karakteri olmalı
        if (!System.Text.RegularExpressions.Regex.IsMatch(isbn, @"^[0-9X]+$"))
            return false;

        // ISBN-10 kontrolü (10 karakter)
        if (isbn.Length == 10)
        {
            return IsValidISBN10(isbn);
        }

        // ISBN-13 kontrolü (13 karakter)
        if (isbn.Length == 13)
        {
            return IsValidISBN13(isbn);
        }

        return false;
    }

    /// <summary>
    /// ISBN-10 formatını doğrular
    /// </summary>
    /// <param name="isbn">ISBN-10 string'i</param>
    /// <returns>True eğer geçerli ISBN-10</returns>
    private static bool IsValidISBN10(string isbn)
    {
        if (isbn.Length != 10)
            return false;

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            if (!char.IsDigit(isbn[i]))
                return false;
            sum += (isbn[i] - '0') * (10 - i);
        }

        char checkDigit = isbn[9];
        if (checkDigit == 'X')
        {
            sum += 10;
        }
        else if (char.IsDigit(checkDigit))
        {
            sum += checkDigit - '0';
        }
        else
        {
            return false;
        }

        return sum % 11 == 0;
    }

    /// <summary>
    /// ISBN-13 formatını doğrular
    /// </summary>
    /// <param name="isbn">ISBN-13 string'i</param>
    /// <returns>True eğer geçerli ISBN-13</returns>
    private static bool IsValidISBN13(string isbn)
    {
        if (isbn.Length != 13)
            return false;

        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            if (!char.IsDigit(isbn[i]))
                return false;
            sum += (isbn[i] - '0') * (i % 2 == 0 ? 1 : 3);
        }

        int checkDigit = (10 - (sum % 10)) % 10;
        return checkDigit == (isbn[12] - '0');
    }

    /// <summary>
    /// String'den ISBN'e implicit conversion
    /// </summary>
    /// <param name="isbn">ISBN string'i</param>
    /// <returns>ISBN value object'i</returns>
    public static implicit operator ISBN(string isbn) => Create(isbn);

    /// <summary>
    /// ISBN'den string'e implicit conversion
    /// </summary>
    /// <param name="isbn">ISBN value object'i</param>
    /// <returns>ISBN string'i</returns>
    public static implicit operator string(ISBN isbn) => isbn.Value;

    /// <summary>
    /// String representation
    /// </summary>
    /// <returns>ISBN değeri</returns>
    public override string ToString() => Value;
}
