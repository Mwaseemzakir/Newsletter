using Dapper;
using DapperDemo.Contexts;
using DapperDemo.Contracts;
using DapperDemo.DTOs;
using DapperDemo.Entities;
using System.Data;

namespace DapperDemo.Repositories
{
    public class NewsletterRepository : INewsletterRepository
    {
        private readonly DapperContext _context;
        public NewsletterRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(NewsletterDTO newsletter)
        {
            var query = @"INSERT INTO Newsletter(Type,Name,Url,About)
                          VALUES(@Type,@Name,@Url,@About)
                        ";
            var parameters = new DynamicParameters();
            parameters.Add("Url", newsletter.Url, DbType.String);
            parameters.Add("Name", newsletter.Name, DbType.String);
            parameters.Add("Type", newsletter.Type, DbType.String);
            parameters.Add("About", newsletter.About, DbType.String);

            using var connection = _context.Connect();
            return await connection.ExecuteAsync(query, parameters) > 0;
        }

        public async Task<bool> Delete(int Id)
        {
            var query = "DELETE FROM Newsletter WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", Id, DbType.Int32);

            using var connection = _context.Connect();
            return await connection.ExecuteAsync(query, parameters) > 0;
        }

        public async Task<List<NewsletterDTO>> GetAll()
        {
            var query = @"SELECT * FROM Newsletter";

            using var connection = _context.Connect();

            var newsletters = await connection.QueryAsync<NewsletterDTO>(query);
            return newsletters.ToList();
        }

        public async Task<Newsletter> GetById(int Id)
        {
            var query = @"SELECT * FROM Newsletter WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", Id, DbType.Int32);

            using var connection = _context.Connect();
            return await connection.QuerySingleOrDefaultAsync<Newsletter>(query, parameters);
        }

        public async Task<bool> Update(int Id, NewsletterDTO newsletter)
        {
            var query = @" UPDATE Newsletter
                           SET Url = @Url, Name = @Name, Type = @Type , About = @About
                           WHERE Id = @Id
                        ";
            var parameters = new DynamicParameters();
            parameters.Add("Id", Id, DbType.Int32);
            parameters.Add("Url", newsletter.Url, DbType.String);
            parameters.Add("Name", newsletter.Name, DbType.String);
            parameters.Add("Type", newsletter.Type, DbType.String);
            parameters.Add("About", newsletter.About, DbType.String);

            using var connection = _context.Connect();
            return await connection.ExecuteAsync(query, parameters) > 0;
        }
    }
}
