
using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupons> GetDiscount(string productId);
        Task<bool> CreateDiscount(Coupons Coupons);
        Task<bool> UpdateDiscount(Coupons Coupons);
        Task<bool> DeleteDiscount(string productId);
    }

    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Coupons> GetDiscount(string ProductId)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var Coupons = await connection.QueryFirstOrDefaultAsync<Coupons>
                ("SELECT * FROM Coupons WHERE ProductId = @ProductId", new { ProductId = ProductId });

            if (Coupons == null)
                return new Coupons
                { ProductId = "No Discount", Amount = 0, Description = "No Discount Desc" };

            return Coupons;
        }

        public async Task<bool> CreateDiscount(Coupons Coupons)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                    ("INSERT INTO Coupons (ProductId, Description, Amount) VALUES (@ProductId, @Description, @Amount)",
                            new { ProductId = Coupons.ProductId, Description = Coupons.Description, Amount = Coupons.Amount });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateDiscount(Coupons Coupons)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                    ("UPDATE Coupons SET ProductId=@ProductId, Description = @Description, Amount = @Amount WHERE Id = @Id",
                            new { ProductId = Coupons.ProductId, Description = Coupons.Description, Amount = Coupons.Amount, Id = Coupons.Id });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string ProductId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM Coupons WHERE ProductId = @ProductId",
                new { ProductId = ProductId });

            if (affected == 0)
                return false;

            return true;
        }
    }
}