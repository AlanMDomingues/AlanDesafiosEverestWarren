﻿using Domain.Models;

namespace Domain.Services.Interfaces
{
    public interface ICustomerBankInfoService : IServiceBase
    {
        CustomerBankInfo Get(int id);

        bool AccountBalanceIsBiggerThanZero(int customerId);

        bool AnyCustomerBankInfoForId(int customerId);

        void Add(CustomerBankInfo customerBankInfo);

        void Withdraw(int id, decimal amount);

        void Deposit(int id, decimal amount);
    }
}
