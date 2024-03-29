﻿using Application.Interfaces;
using Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Alan_WarrenDesafio1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfoliosController : ControllersBase<PortfoliosController>
    {
        private readonly IPortfolioAppService _portfolioAppService;

        public PortfoliosController(
            IPortfolioAppService portfolioAppService,
            ILogger<PortfoliosController> logger)
            : base(logger)
            => _portfolioAppService = portfolioAppService ?? throw new ArgumentNullException(nameof(portfolioAppService));

        [HttpGet("customerId/{customerId}")]
        public IActionResult GetAll(int customerId)
        {
            return SafeAction(() =>
            {
                var results = _portfolioAppService.GetAll(customerId);

                return !results.Any()
                    ? NotFound($"Portfolios not found for Customer ID: {customerId}")
                    : Ok(results);
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return SafeAction(() =>
            {
                var result = _portfolioAppService.Get(id);

                return result is null
                    ? BadRequest($"Portfolio not found for ID: {id}")
                    : Ok(result);
            });
        }

        [HttpPost]
        public IActionResult Post(CreatePortfolioRequest portfolio)
        {
            return SafeAction(() =>
            {
                _portfolioAppService.Add(portfolio);

                return Created("~api/portfolio", default);
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdatePortfolioRequest portfolio)
        {
            return SafeAction(() =>
            {
                _portfolioAppService.Update(id, portfolio);

                return Ok();
            });
        }

        [HttpPatch("withdraw/{portfolioId}")]
        public IActionResult TransferMoneyToAccountBalance(int customerBankInfoId, int portfolioId, decimal cash)
        {
            return SafeAction(() =>
            {
                _portfolioAppService.TransferMoneyToAccountBalance(customerBankInfoId, portfolioId, cash);

                return Ok();
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return SafeAction(() =>
            {
                _portfolioAppService.Delete(id);

                return NoContent();
            });
        }

        [HttpPost("invest")]
        public IActionResult Invest(CreateOrderRequest orderRequest)
        {
            return SafeAction(() =>
            {
                _portfolioAppService.Invest(orderRequest);

                return Created("~api/portfolios", default);
            });
        }
    }
}
