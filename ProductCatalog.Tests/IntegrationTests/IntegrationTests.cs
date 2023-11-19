using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductCatalog.Contract;
using Xunit;

namespace ProductCatalog.Tests.IntegrationTests;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ApiFixture _apiFixture;
        
        public IntegrationTests(WebApplicationFactory<Program> factory) =>
            _apiFixture = new ApiFixture(factory);

        [Fact]
        public async Task ProductSearch()
        {
            var product1 = new ProductBuilder().WithName("product 1").WithRate(4).Build();
            var product2 = new ProductBuilder().WithName("product 11").WithRate(2).Build();
            var product3 = new ProductBuilder().WithName("product 2").WithRate(4).Build();

            await new DatabaseFixture()
                .WithProducts(product1, product2, product3)
                .ClearAndStore();

            var (response, statusCode) = await _apiFixture.SearchProducts("1");

            statusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            response!.Results.Length.Should().Be(2);
            response.Results[0].Id.Should().Be(product1.Id);
            response.Results[1].Id.Should().Be(product2.Id);
        }


        [Fact]
        public async Task ProductRegistration()
        {
            await new DatabaseFixture().ClearDatabase();
            
            var registerProductCommand = new Commands.RegisterProduct(
                "Sample name",
                "Sample description",
                12m
            );

            var (createResponse, createStatusCode) = await _apiFixture.RegisterProduct(registerProductCommand);

            createStatusCode.Should().Be(HttpStatusCode.OK);
            createResponse.Should().NotBeNull();
            createResponse!.Id.Should().NotBe(default);

            var products = await _apiFixture.GetAllProductsFromDatabase();

            products.Length.Should().Be(1);
            products.Single().Should().Be(new Product(
                createResponse.Id,
                registerProductCommand.Name,
                registerProductCommand.Description,
                registerProductCommand.Price,
                null,
                0
            ));
        }
        
        [Fact]
        public async Task CannotCreateProductWhenNameIsEmpty()
        {
            await new DatabaseFixture().ClearDatabase();
            
            var command = new Commands.RegisterProduct(
                string.Empty,
                "Sample description",
                12m
            );

            var (createResponse, createStatusCode) = await _apiFixture.RegisterProduct(command);

            createStatusCode.Should().Be(HttpStatusCode.BadRequest);
            createResponse.Should().BeNull();
        }
        
        [Fact]
        public async Task GetProduct()
        {
            var product = new ProductBuilder().Build();

            await new DatabaseFixture()
                .WithProducts(product)
                .ClearAndStore();

            var (response, statusCode) = await _apiFixture.GetProduct(product.Id);

            statusCode.Should().Be(HttpStatusCode.OK);
            response.Should().Be(
                new Queries.GetProduct.Response(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price
                )
            );
        }

    }