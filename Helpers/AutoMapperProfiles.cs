using AutoMapper;
using banking.DTOs;
using banking.Entities;

namespace banking.Helpers;

// Define AutoMapper profiles for object-object mapping
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // Map Client to ClientDto and vice versa
        CreateMap<Client, ClientDto>();
        CreateMap<CreateClientDto, Client>();

        // Map UpdateClientDto to Client, skipping null values
        CreateMap<UpdateClientDto, Client>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Map Account and its DTOs bidirectionally
        CreateMap<Account, AccountDto>();
        CreateMap<AccountDto, Account>();
        

        // Map Address and its DTO bidirectionally
        CreateMap<Address, AddressDto>();
        CreateMap<AddressDto, Address>();
        CreateMap<UpdateAddressDto, Address>()
           .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Map SearchHistory to SearchHistoryDto unidirectionally (only SearchHistory -> SearchHistoryDto)
        CreateMap<SearchHistory, SearchHistoryDto>();
    }
}
