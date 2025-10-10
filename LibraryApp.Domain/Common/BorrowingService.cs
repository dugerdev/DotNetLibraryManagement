using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Domain.Common;

/// <summary>
/// Kitap ödünç verme işlemlerini yöneten domain service implementasyonu
/// Bu sınıf, kitap ödünç verme ile ilgili tüm business logic'i içerir
/// Domain service'ler, birden fazla repository'yi koordine eder ve karmaşık iş kurallarını implement eder
/// </summary>
public class BorrowingService : IBorrowingService
{
    // Dependency Injection ile repository'leri alırız
    // Bu sayede domain service, concrete repository'lere bağımlı olmaz
    private readonly IMemberRepository _memberRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    /// <summary>
    /// BorrowingService constructor'ı
    /// Dependency Injection ile repository'leri alır
    /// </summary>
    /// <param name="memberRepository">Üye repository'si</param>
    /// <param name="bookRepository">Kitap repository'si</param>
    /// <param name="borrowRecordRepository">Ödünç verme kaydı repository'si</param>
    public BorrowingService(
        IMemberRepository memberRepository,
        IBookRepository bookRepository,
        IBorrowRecordRepository borrowRecordRepository)
    {
        _memberRepository = memberRepository;
        _bookRepository = bookRepository;
        _borrowRecordRepository = borrowRecordRepository;
    }

    /// <summary>
    /// Üyenin belirli bir kitabı ödünç alıp alamayacağını kontrol eder
    /// Bu method, ödünç verme öncesi tüm business rule'ları kontrol eder
    /// </summary>
    public async Task<bool> CanMemberBorrowBookAsync(Guid memberId, Guid bookId, CancellationToken cancellationToken = default)
    {
        // 1. Üyeyi bul ve kontrol et
        var member = await _memberRepository.GetByIdAsync(memberId, cancellationToken);
        if (member == null)
            return false; // Üye bulunamadı

        // 2. Üyelik geçerli mi kontrol et
        if (!member.IsMemberShipValid)
            return false; // Üyelik geçersiz

        // 3. Maksimum ödünç alma limitini kontrol et
        var activeBorrowCount = await _borrowRecordRepository.GetActiveBorrowCountAsync(memberId, cancellationToken);
        const int maxBorrowLimit = 5; // Maksimum 5 kitap ödünç alabilir
        if (activeBorrowCount >= maxBorrowLimit)
            return false; // Limit aşılmış

        // 4. Kitabı bul ve müsaitlik kontrolü yap
        var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);
        if (book == null)
            return false; // Kitap bulunamadı

        // 5. Kitap ödünç alınabilir mi kontrol et
        return book.CanBeBorrowed; // Domain entity'deki business rule
    }

    /// <summary>
    /// Üyeye kitap ödünç verir
    /// Bu method, ödünç verme işlemini tamamen yönetir
    /// </summary>
    public async Task<BorrowRecord> BorrowBookAsync(Guid memberId, Guid bookId, CancellationToken cancellationToken = default)
    {
        // 1. Ödünç alma koşullarını kontrol et
        if (!await CanMemberBorrowBookAsync(memberId, bookId, cancellationToken))
        {
            // Detaylı hata mesajı için ek kontroller
            var member = await _memberRepository.GetByIdAsync(memberId, cancellationToken);
            var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);
            
            if (member == null)
                throw new EntityNotFoundException("Member", memberId);
            
            if (book == null)
                throw new EntityNotFoundException("Book", bookId);
            
            if (!member.IsMemberShipValid)
                throw new MemberCannotBorrowException("Membership is not valid or expired");
            
            if (!book.IsAvailable)
                throw new BookNotAvailableException(book.Title, "No available copies");
            
            var activeCount = await _borrowRecordRepository.GetActiveBorrowCountAsync(memberId, cancellationToken);
            throw new MemberCannotBorrowException("Maximum borrow limit reached", activeCount, 5);
        }

        // 2. Kitabı tekrar kontrol et (concurrency için)
        var bookToBorrow = await _bookRepository.GetByIdAsync(bookId, cancellationToken);
        if (bookToBorrow == null || !bookToBorrow.IsAvailable)
            throw new BookNotAvailableException(bookToBorrow?.Title ?? "Unknown");

        // 3. Yeni ödünç verme kaydı oluştur
        var borrowRecord = new BorrowRecord
        {
            BookId = bookId,
            MemberId = memberId,
            BorrowDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(14), // 14 gün süre
            Status = BorrowStatus.Borrowed,
            Notes = "Book borrowed successfully"
        };

        // 4. Kitabın müsait kopya sayısını azalt
        bookToBorrow.AvailableCopies--;
        await _bookRepository.UpdateAsync(bookToBorrow, cancellationToken);

        // 5. Ödünç verme kaydını kaydet
        return await _borrowRecordRepository.AddAsync(borrowRecord, cancellationToken);
    }

    /// <summary>
    /// Ödünç verilen kitabı geri alır
    /// Bu method, kitap iade işlemini yönetir
    /// </summary>
    public async Task ReturnBookAsync(Guid borrowRecordId, CancellationToken cancellationToken = default)
    {
        // 1. Ödünç verme kaydını bul
        var borrowRecord = await _borrowRecordRepository.GetByIdAsync(borrowRecordId, cancellationToken);
        if (borrowRecord == null)
            throw new EntityNotFoundException("BorrowRecord", borrowRecordId);

        // 2. Zaten iade edilmiş mi kontrol et
        if (borrowRecord.Status == BorrowStatus.Returned)
            throw new System.InvalidOperationException("Book is already returned");

        // 3. İade tarihini set et
        borrowRecord.ReturnDate = DateTime.Now;
        borrowRecord.Status = BorrowStatus.Returned;
        borrowRecord.Notes = "Book returned successfully";

        // 4. Kitabın müsait kopya sayısını artır
        var book = await _bookRepository.GetByIdAsync(borrowRecord.BookId, cancellationToken);
        if (book != null)
        {
            book.AvailableCopies++;
            await _bookRepository.UpdateAsync(book, cancellationToken);
        }

        // 5. Ödünç verme kaydını güncelle
        await _borrowRecordRepository.UpdateAsync(borrowRecord, cancellationToken);
    }

    /// <summary>
    /// Ödünç verme kaydı için ceza miktarını hesaplar
    /// Bu method, gecikme cezası hesaplaması yapar
    /// </summary>
    public async Task<decimal> CalculateFineAsync(Guid borrowRecordId, CancellationToken cancellationToken = default)
    {
        // 1. Ödünç verme kaydını bul
        var borrowRecord = await _borrowRecordRepository.GetByIdAsync(borrowRecordId, cancellationToken);
        if (borrowRecord == null)
            return 0; // Kayıt bulunamadı, ceza yok

        // 2. Zaten iade edilmiş mi kontrol et
        if (borrowRecord.Status == BorrowStatus.Returned)
            return 0; // İade edilmiş, ceza yok

        // 3. Süre geçmiş mi kontrol et
        if (DateTime.Now <= borrowRecord.DueDate)
            return 0; // Süre geçmemiş, ceza yok

        // 4. Gecikme gün sayısını hesapla
        var daysOverdue = (DateTime.Now - borrowRecord.DueDate).Days;
        
        // 5. Ceza miktarını hesapla (günde 1 TL)
        const decimal dailyFineRate = 1.0m;
        var fineAmount = daysOverdue * dailyFineRate;

        // 6. Status'u güncelle (eğer henüz güncellenmemişse)
        if (borrowRecord.Status != BorrowStatus.Overdue)
        {
            borrowRecord.Status = BorrowStatus.Overdue;
            borrowRecord.FineAmount = fineAmount;
            await _borrowRecordRepository.UpdateAsync(borrowRecord, cancellationToken);
        }

        return fineAmount;
    }
}
