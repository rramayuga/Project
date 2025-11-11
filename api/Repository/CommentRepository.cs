using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        // OLD EF VERSION
        /*
        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }
        */

        // SP VERSION
        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            var sql = "CALL sp_CreateComment({0}, {1}, {2}, {3}, {4});";
            await _context.Database.ExecuteSqlRawAsync(sql,
                commentModel.Title,
                commentModel.Content,
                commentModel.CreatedOn,
                commentModel.StockId,
                commentModel.AppUserId);

            return commentModel;
        }

        // OLD EF VERSION
        /*
        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (commentModel == null) return null;
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }
        */

        // SP VERSION
        public async Task<Comment?> DeleteAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("CALL sp_DeleteComment({0});", id);
            return null;
        }

        // OLD EF VERSION
        /*
        public async Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject)
        {
            var comments = _context.Comments.Include(a => a.AppUser).AsQueryable();
            if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
                comments = comments.Where(s => s.Stock.Symbol == queryObject.Symbol);
            if (queryObject.IsDecsending == true)
                comments = comments.OrderByDescending(c => c.CreatedOn);
            return await comments.ToListAsync();
        }
        */

        // SP VERSION
        public async Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject)
        {
            var sql = "CALL sp_GetAllComments({0}, {1});";
            var comments = await _context.Comments
                .FromSqlRaw(sql, queryObject.Symbol ?? "", queryObject.IsDecsending)
                .Include(c => c.AppUser)
                .ToListAsync();
            return comments;
        }

        // OLD EF VERSION
        /*
        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }
        */

        // SP VERSION
        public async Task<Comment?> GetByIdAsync(int id)
        {
            var sql = "CALL sp_GetCommentById({0});";
            return await _context.Comments
                .FromSqlRaw(sql, id)
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync();
        }

        // OLD EF VERSION
        /*
        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await _context.Comments.FindAsync(id);
            if (existingComment == null) return null;
            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;
            await _context.SaveChangesAsync();
            return existingComment;
        }
        */

        // SP VERSION
        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var sql = "CALL sp_UpdateComment({0}, {1}, {2});";
            await _context.Database.ExecuteSqlRawAsync(sql,
                id, commentModel.Title, commentModel.Content);
            return commentModel;
        }
    }
}
