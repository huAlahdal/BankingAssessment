using System;
using AutoMapper;
using banking.DTOs;
using banking.Entities;

namespace banking.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Client, ClientDto>();
        CreateMap<CreateClientDto, Client>();
        CreateMap<UpdateClientDto, Client>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Skip null values

        CreateMap<Account, AccountDto>();
        CreateMap<AccountDto, Account>();

        CreateMap<Address, AddressDto>();
        CreateMap<AddressDto, Address>();

        CreateMap<SearchHistory, SearchHistoryDto>();

    }
}
