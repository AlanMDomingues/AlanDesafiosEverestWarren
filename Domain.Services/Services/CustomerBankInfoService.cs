﻿using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System;

namespace Domain.Services.Services
{
    public class CustomerBankInfoService : ServiceBase, ICustomerBankInfoService
    {
        public CustomerBankInfoService(
            IRepositoryFactory repositoryFactory,
            IUnitOfWork unitOfWork)
            : base(repositoryFactory, unitOfWork) { }

        public CustomerBankInfo Get(int id)
        {
            var repository = RepositoryFactory.Repository<CustomerBankInfo>();

            var query = repository.MultipleResultQuery()
                                  .AndFilter(x => x.Id.Equals(id));

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public void Add(Customer customer)
        {
            var randomNumber = new Random();
            string accountNumber = "";
            for (int i = 0; i < 20; i++)
                accountNumber += randomNumber.Next(0, 9).ToString();

            var repository = UnitOfWork.Repository<CustomerBankInfo>();

            repository.Add(new CustomerBankInfo
            {
                Account = accountNumber,
                Customer = customer
            });
        }
    }
}
