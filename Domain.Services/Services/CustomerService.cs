﻿using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.Repository.Extensions;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Services.Services
{
    public class CustomerService : ServiceBase, ICustomerService
    {
        public CustomerService(
            IUnitOfWork<DataContext> unitOfWork,
            IRepositoryFactory<DataContext> repositoryFactory)
            : base(repositoryFactory, unitOfWork) { }

        public IEnumerable<Customer> GetAll()
        {
            var repository = RepositoryFactory.Repository<Customer>();

            var query = repository.MultipleResultQuery();

            var result = repository.Search(query);

            return result;
        }

        public IEnumerable<Customer> GetAll(params Expression<Func<Customer, bool>>[] predicates)
        {
            var repository = RepositoryFactory.Repository<Customer>();

            var query = repository.MultipleResultQuery();

            foreach (var item in predicates)
            {
                query.AndFilter(item);
            }

            var result = repository.Search(query);

            return result;
        }

        public Customer Get(params Expression<Func<Customer, bool>>[] predicates)
        {
            var repository = RepositoryFactory.Repository<Customer>();

            var query = repository.SingleResultQuery();
            foreach (var item in predicates)
            {
                query.AndFilter(item);
            }

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public void Add(Customer newCustomer)
        {
            var repository = UnitOfWork.Repository<Customer>();

            repository.Add(newCustomer);
            UnitOfWork.SaveChanges();
        }

        public void Update(Customer customer)
        {
            var repository = UnitOfWork.Repository<Customer>();

            repository.Update(customer);
            UnitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            var customer = Get(x => x.Id.Equals(id));
            var repository = UnitOfWork.Repository<Customer>();
            repository.RemoveTracking(customer);

            repository.Remove(x => x.Id.Equals(id));
        }

        public bool ValidateAlreadyExists(Customer customer)
        {
            var repository = RepositoryFactory.Repository<Customer>();

            return repository.Any(x => x.Email.Equals(customer.Email) || x.Cpf.Equals(customer.Cpf));
        }

        public bool AnyForId(int id)
        {
            var repository = RepositoryFactory.Repository<Customer>();

            return repository.Any(x => x.Id.Equals(id));
        }
    }
}