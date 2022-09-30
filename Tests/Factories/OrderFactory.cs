﻿using Application.Models.Requests;
using Bogus;
using Domain.Models;
using System.Collections.Generic;

namespace Tests.Factories;

public static class OrderFactory
{
    public static Order CreateOrder()
    {
        var fakeOrder = new Faker<Order>()
            .CustomInstantiator(x => new Order(x.Random.Int(1, 100), x.Random.Int(1, 10), x.Random.Int(1, 1000)))
            .RuleFor(x => x.UnitPrice, x => x.Finance.Amount(0, 10000));

        var order = fakeOrder.Generate();
        order.SetNetValue();

        return order;
    }

    public static List<Order> CreateOrders()
    {
        var fakeOrder = new Faker<Order>()
            .CustomInstantiator(x => new Order(x.Random.Int(1, 100), x.Random.Int(1, 10), x.Random.Int(1, 1000)))
            .RuleFor(x => x.UnitPrice, x => x.Finance.Amount(0, 10000));

        var orders = fakeOrder.Generate(5);
        foreach (var item in orders)
        {
            item.SetNetValue();
        }

        return orders;
    }

    public static CreateOrderRequest CreateOrderRequest()
    {
        var fakeOrder = new Faker<CreateOrderRequest>()
            .RuleFor(x => x.Quotes, x => x.Random.Int(1, 100))
            .RuleFor(x => x.PortfolioId, x => x.Random.Int(1, 10))
            .RuleFor(x => x.ProductId, x => x.Random.Int(1, 1000));

        var order = fakeOrder.Generate();

        return order;
    }
}