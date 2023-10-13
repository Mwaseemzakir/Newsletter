using DapperDemo.DTOs;
using DapperDemo.Entities;

namespace DapperDemo.Contracts
{
    public interface INewsletterRepository
    {
        Task<bool> Delete(int Id);
        Task<List<NewsletterDTO>> GetAll();
        Task<Newsletter> GetById(int Id);
        Task<bool> Create(NewsletterDTO newsletter);
        Task<bool> Update(int Id, NewsletterDTO newsletter);
    }
}
