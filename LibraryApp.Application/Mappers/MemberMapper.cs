using LibraryApp.Application.DTOs.Members;

namespace LibraryApp.Application.Mappers;

/// <summary>
/// Üye mapper'ı
/// Member entity'si ile Member DTO'ları arasında dönüşüm yapar
/// </summary>
public static class MemberMapper
{
    /// <summary>
    /// Member entity'sini MemberDto'ya çevirir
    /// </summary>
    /// <param name="member">Member entity'si</param>
    /// <returns>MemberDto</returns>
    public static MemberDto ToDto(Domain.Entities.Member member)
    {
        return new MemberDto
        {
            Id = member.Id,
            FirstName = member.FirstName,
            LastName = member.LastName,
            FullName = member.FullName,
            Email = member.Email,
            PhoneNumber = member.PhoneNumber,
            Address = member.Address,
            MembershipDate = member.MembershipDate,
            ExpirationDate = member.ExpirationDate,
            IsActive = member.IsActive,
            IsMembershipValid = member.IsMemberShipValid,
            ActiveBorrowCount = member.ActiveBorrowCount
        };
    }

    /// <summary>
    /// CreateMemberDto'yu Member entity'sine çevirir
    /// </summary>
    /// <param name="dto">CreateMemberDto</param>
    /// <returns>Member entity'si</returns>
    public static Domain.Entities.Member ToEntity(CreateMemberDto dto)
    {
        return new Domain.Entities.Member
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            MembershipDate = dto.MembershipDate,
            ExpirationDate = dto.ExpirationDate,
            IsActive = true // Varsayılan olarak aktif
        };
    }

    /// <summary>
    /// UpdateMemberDto'yu mevcut Member entity'sine uygular
    /// </summary>
    /// <param name="member">Mevcut Member entity'si</param>
    /// <param name="dto">UpdateMemberDto</param>
    public static void UpdateEntity(Domain.Entities.Member member, UpdateMemberDto dto)
    {
        member.FirstName = dto.FirstName;
        member.LastName = dto.LastName;
        member.Email = dto.Email;
        member.PhoneNumber = dto.PhoneNumber;
        member.Address = dto.Address;
        member.MembershipDate = dto.MembershipDate;
        member.ExpirationDate = dto.ExpirationDate;
        member.IsActive = dto.IsActive;
    }
}
