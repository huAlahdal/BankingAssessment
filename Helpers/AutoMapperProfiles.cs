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
        CreateMap<ClientDto, Client>();

        CreateMap<Account, AccountDto>();
        CreateMap<AccountDto, Account>();

        CreateMap<Address, AddressDto>();
        CreateMap<AddressDto, Address>();

    }
}
