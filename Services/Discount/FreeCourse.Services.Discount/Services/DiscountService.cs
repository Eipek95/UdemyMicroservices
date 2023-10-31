﻿using Dapper;
using FreeCourse.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new { Id = id });

            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Discount not found", 404);
        }

        public async Task<Response<List<Model.Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Model.Discount>("Select*from discount");

            return Response<List<Model.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<Model.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discounts = await _dbConnection.QueryAsync<Model.Discount>("select*from discount where userId=@UserId and code=@Code", new { UserId = userId, Code = code });

            var hasDiscount = discounts.FirstOrDefault();
            if (hasDiscount == null)
            {
                return Response<Model.Discount>.Fail("Discount not found", 404);
            }
            return Response<Model.Discount>.Success(hasDiscount, 200);
        }

        public async Task<Response<Model.Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Model.Discount>("select*from discount where id=@Id", new { Id = id })).SingleOrDefault();
            if (discount == null)
            {
                return Response<Model.Discount>.Fail("Discount not found", 404);
            }

            return Response<Model.Discount>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Save(Model.Discount discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("INSERT INTO discount(userid,rate,code) values(@UserId,@Rate,@Code)", discount);
            if (saveStatus > 0)//success
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("An error occourred while adding", 500);
        }

        public async Task<Response<NoContent>> Update(Model.Discount discount)
        {
            //id ye sahip data var mı kontrol et yoksa 404 dön
            var status = await _dbConnection.ExecuteAsync("update discount set userid=@UserId,code=@Code,rate=@Rate where id=@Id", new
            {
                Id = discount.Id,
                Code = discount.Code,
                UserId = discount.UserId,
                Rate = discount.Rate,
            });//istersek new yerine direk discount verebilirdik ama farklı kullanım şekillerini görmek için yaptık

            if (status > 0)//success
            {
                return Response<NoContent>.Success(200);
            }

            return Response<NoContent>.Fail("Discount not found", 404);
        }
    }
}