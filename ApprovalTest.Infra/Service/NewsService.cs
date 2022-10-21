using ApprovalTest.Domain.Dto;
using ApprovalTest.Domain.Service;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using NewsAPI;
using NewsAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApprovalTest.Infra.Service
{
    public class NewsService : INewsService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public NewsService(IMapper mapper, IConfiguration config)
        {
            _mapper = mapper;
            _config = config;
        }

        public async Task<List<ArticleDto>> GetEverything(EverythingRequest request)
        {
            try
            {
                string apiKey = _config.GetValue<string>("ApiKey");

                var newsApiClient = new NewsApiClient(apiKey);
                var articlesResponse = await newsApiClient.GetEverythingAsync(request);

                if (articlesResponse.Error != null)
                    throw new Exception($"{articlesResponse.Error.Code} : {articlesResponse.Error.Message}");

                return _mapper.Map<List<ArticleDto>>(articlesResponse.Articles);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ArticleDto>> GetTopHeadlines(TopHeadlinesRequest request)
        {
            try
            {
                string apiKey = _config.GetValue<string>("ApiKey");

                var newsApiClient = new NewsApiClient(apiKey);
                var articlesResponse = await newsApiClient.GetTopHeadlinesAsync(request);

                if (articlesResponse.Error != null)
                    throw new Exception($"{articlesResponse.Error.Code} : {articlesResponse.Error.Message}");

                return _mapper.Map<List<ArticleDto>>(articlesResponse.Articles);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void ValidateResult()
        {

        }
    }
}
