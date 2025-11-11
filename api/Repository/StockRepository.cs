using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        // OLD EF VERSION
        /*
        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }
        */

        // SP VERSION
        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            var sql = "CALL sp_CreateStock({0}, {1}, {2}, {3}, {4}, {5});";
            await _context.Database.ExecuteSqlRawAsync(sql,
                stockModel.Symbol,
                stockModel.CompanyName,
                stockModel.Purchase,
                stockModel.LastDiv,
                stockModel.Industry,
                stockModel.MarketCap);
            return stockModel;
        }

        // OLD EF VERSION
        /*
        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null) return null;
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }
        */

        // SP VERSION
        public async Task<Stock?> DeleteAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("CALL sp_DeleteStock({0});", id);
            return null;
        }

        // OLD EF VERSION
        /*
        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();
            // filtering and pagination...
        }
        */

        // SP VERSION
        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var sql = "CALL sp_GetAllStocks({0}, {1}, {2}, {3}, {4});";
            var stocks = await _context.Stocks
                .FromSqlRaw(sql,
                    query.CompanyName ?? "",
                    query.Symbol ?? "",
                    query.SortBy ?? "",
                    query.IsDecsending,
                    query.PageNumber)
                .Include(c => c.Comments)
                .ToListAsync();
            return stocks;
        }

        // OLD EF VERSION
        /*
        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }
        */

        // SP VERSION
        public async Task<Stock?> GetByIdAsync(int id)
        {
            var sql = "CALL sp_GetStockById({0});";
            return await _context.Stocks
                .FromSqlRaw(sql, id)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync();
        }

        // OLD EF VERSION
        /*
        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }
        */

        // SP VERSION
        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            var sql = "CALL sp_GetStockBySymbol({0});";
            return await _context.Stocks
                .FromSqlRaw(sql, symbol)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync();
        }

        // OLD EF VERSION
        /*
        public Task<bool> StockExists(int id)
        {
            return _context.Stocks.AnyAsync(s => s.Id == id);
        }
        */

        // SP VERSION
        public async Task<bool> StockExists(int id)
        {
            var sql = "CALL sp_StockExists({0});";
            var result = await _context.Stocks.FromSqlRaw(sql, id).ToListAsync();
            return result.Any();
        }

        // OLD EF VERSION
        /*
        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingStock == null) return null;
            existingStock.Symbol = stockDto.Symbol;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.Industry = stockDto.Industry;
            existingStock.MarketCap = stockDto.MarketCap;
            await _context.SaveChangesAsync();
            return existingStock;
        }
        */

        // SP VERSION
        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var sql = "CALL sp_UpdateStock({0}, {1}, {2}, {3}, {4}, {5}, {6});";
            await _context.Database.ExecuteSqlRawAsync(sql,
                id,
                stockDto.Symbol,
                stockDto.CompanyName,
                stockDto.Purchase,
                stockDto.LastDiv,
                stockDto.Industry,
                stockDto.MarketCap);
            return null;
        }
    }
}
