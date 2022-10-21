using ApprovalTest.Domain.Dto;
using NewsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApprovalTest.Domain.Service
{
    public interface INewsService
    {
        Task<List<ArticleDto>> GetEverything(EverythingRequest request);
        Task<List<ArticleDto>> GetTopHeadlines(TopHeadlinesRequest request);
    }
}
