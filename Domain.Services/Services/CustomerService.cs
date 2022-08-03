﻿using Domain.Models;
using Domain.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryFactory _repositoryFactory;

        public CustomerService(IUnitOfWork unitOfWork, IRepositoryFactory repositoryFactory)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        }

        public IEnumerable<Customer> GetAll()
        {
            var repository = _repositoryFactory.Repository<Customer>();

            var query = repository.MultipleResultQuery();

            var result = repository.Search(query);

            return result;
        }

        public IEnumerable<Customer> GetAll(params Expression<Func<Customer, bool>>[] predicates)
        {
            var repository = _repositoryFactory.Repository<Customer>();

            var query = repository.MultipleResultQuery();
            foreach (var item in predicates)
            {
                query.AndFilter(item);
            }

            var result = repository.Search(query);

            return result;
        }


        public Customer GetBy(params Expression<Func<Customer, bool>>[] predicates)
        {
            var repository = _repositoryFactory.Repository<Customer>();

            var query = repository.SingleResultQuery();
            foreach (var item in predicates)
            {
                query.AndFilter(item);
            }

            var result = repository.FirstOrDefault(query);

            return result;
        }

        public (bool exists, string message) Create(Customer newCustomer)
        {
            (bool exists, string message) = ValidateAlreadyExists(newCustomer);
            if (exists) return (false, message);

            var repository = _unitOfWork.Repository<Customer>();

            repository.Add(newCustomer);
            _unitOfWork.SaveChanges();

            return (true, newCustomer.Id.ToString());
        }

        public (bool status, string messageResult) Update(Customer customer)
        {
            (bool exists, string message) = ValidateAlreadyExists(customer);
            if (exists) return (false, message);

            var repository = _unitOfWork.Repository<Customer>();
            repository.Update(customer);
            _unitOfWork.SaveChanges();

            return (true, $"Customer for ID: {customer.Id} updated successfully");
        }

        public bool Delete(int id)
        {
            var repository = _unitOfWork.Repository<Customer>();

            var customerToDelete = GetBy(x => x.Id == id);
            if (customerToDelete is null) return false;

            repository.Remove(customerToDelete);
            _unitOfWork.SaveChanges();

            return true;
        }

        private (bool exists, string message) ValidateAlreadyExists(Customer customer)
        {
            var repository = _repositoryFactory.Repository<Customer>();

            if (repository.Any(x => x.Id != customer.Id && (x.Email.Equals(customer.Email) || x.Cpf.Equals(customer.Cpf))))
            {
                return (true, "Customer already exists, please insert a new customer");
            }

            return (default, customer.Id.ToString());
        }
    }
}