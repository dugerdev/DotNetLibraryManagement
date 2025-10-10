using LibraryApp.Application.DTOs.Members;
using LibraryApp.Application.Interfaces;
using LibraryApp.Application.Mappers;
using LibraryApp.Domain.Common;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Application.Services;

/// <summary>
/// Üye application service implementasyonu
/// </summary>
public class MemberApplicationService : IMemberApplicationService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MemberApplicationService(
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateMemberAsync(CreateMemberDto dto, CancellationToken cancellationToken = default)
    {
        // Email benzersizlik kontrolü
        var existingMember = await _memberRepository.GetMemberByEmailAsync(dto.Email, cancellationToken);
        if (existingMember != null)
            throw new DuplicateEntityException("Member", "Email", dto.Email);

        // Telefon numarası benzersizlik kontrolü
        var existingPhone = await _memberRepository.GetMemberByPhoneAsync(dto.PhoneNumber, cancellationToken);
        if (existingPhone != null)
            throw new DuplicateEntityException("Member", "PhoneNumber", dto.PhoneNumber);

        // Entity oluştur
        var member = MemberMapper.ToEntity(dto);
        var createdMember = await _memberRepository.AddAsync(member, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return createdMember.Id;
    }

    public async Task<MemberDto?> GetMemberByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(id, cancellationToken);
        return member != null ? MemberMapper.ToDto(member) : null;
    }

    public async Task<IEnumerable<MemberDto>> GetAllMembersAsync(CancellationToken cancellationToken = default)
    {
        var members = await _memberRepository.GetAllAsync(cancellationToken);
        return members.Select(MemberMapper.ToDto);
    }

    public async Task<bool> UpdateMemberAsync(Guid id, UpdateMemberDto dto, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(id, cancellationToken);
        if (member == null)
            return false;

        // Email benzersizlik kontrolü (kendisi hariç)
        var existingMember = await _memberRepository.GetMemberByEmailAsync(dto.Email, cancellationToken);
        if (existingMember != null && existingMember.Id != id)
            throw new DuplicateEntityException("Member", "Email", dto.Email);

        // Telefon numarası benzersizlik kontrolü (kendisi hariç)
        var existingPhone = await _memberRepository.GetMemberByPhoneAsync(dto.PhoneNumber, cancellationToken);
        if (existingPhone != null && existingPhone.Id != id)
            throw new DuplicateEntityException("Member", "PhoneNumber", dto.PhoneNumber);

        // Güncelle
        MemberMapper.UpdateEntity(member, dto);
        await _memberRepository.UpdateAsync(member, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteMemberAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(id, cancellationToken);
        if (member == null)
            return false;

        await _memberRepository.SoftDeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<MemberDto>> SearchMembersAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var members = await _memberRepository.SearchMembersAsync(searchTerm, cancellationToken);
        return members.Select(MemberMapper.ToDto);
    }

    public async Task<IEnumerable<MemberDto>> GetActiveMembersAsync(CancellationToken cancellationToken = default)
    {
        var members = await _memberRepository.GetActiveMembersAsync(cancellationToken);
        return members.Select(MemberMapper.ToDto);
    }

    public async Task<MemberDto?> GetMemberByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetMemberByEmailAsync(email, cancellationToken);
        return member != null ? MemberMapper.ToDto(member) : null;
    }
}
