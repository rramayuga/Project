using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        // OLD EF VERSION
        /*
        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }
        */

        // SP VERSION
        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            var sql = "CALL sp_CreatePortfolio({0}, {1});";
            await _context.Database.ExecuteSqlRawAsync(sql,
                portfolio.AppUserId, portfolio.StockId);
            return portfolio;
        }

        // OLD EF VERSION
        /*
        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x =>
                x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());
            if (portfolioModel == null) return null;
            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }
        */

        // SP VERSION
        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var sql = "CALL sp_DeletePortfolio({0}, {1});";
            await _context.Database.ExecuteSqlRawAsync(sql,
                appUser.Id, symbol);
            return null;
        }

        // OLD EF VERSION
        /*
        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(u => u.AppUserId == user.Id)
                .Select(stock => new Stock
                {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap
                }).ToListAsync();
        }
        */

        // SP VERSION
        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            var sql = "CALL sp_GetUserPortfolio({0});";
            return await _context.Stocks.FromSqlRaw(sql, user.Id).ToListAsync();
        }
    }
}
