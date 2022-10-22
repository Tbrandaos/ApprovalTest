using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsAPI.Models;
using NewsAPI;
using NewsAPI.Constants;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using AutoMapper;
using ApprovalTest.Domain.Dto;
using ApprovalTest.Domain.Service;
using ApprovalTest.Infra.Service;

namespace ApprovalTest.Test
{
    [TestClass]
    public class NewsServiceTest
    {
        private static IMapper _mapper;
        private static IConfigurationRoot _config;
        private INewsService _service;

        public NewsServiceTest()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"ApiKey", "7adcd4c4dd744dadb6a3fff4dc9f6878"}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.CreateMap<Article, ArticleDto>().ReverseMap();
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _service = new NewsService(_mapper, _config);
        }

        [Fact]
        public async void BasicEverythingRequest_Works()
        {
            var everythingRequest = new EverythingRequest
            {
                Q = "bitcoin"
            };

            var result = await _service.GetEverything(everythingRequest);

            Assert.IsTrue(result.Count > 0);
        }

        [Fact]
        public async void ComplexEverythingRequest_Works()
        {
            var everythingRequest = new EverythingRequest
            {
                Q = "apple",
                SortBy = SortBys.PublishedAt,
                Language = Languages.EN
            };

            var result = await _service.GetEverything(everythingRequest);

            Assert.IsTrue(result.Count > 0);
        }

        [Fact]
        public async void BadEverythingRequest_ReturnsError()
        {
            var everythingRequest = new EverythingRequest
            {
                Q = "bitcoin"
            };

            var myConfiguration = new Dictionary<string, string>
            {
                {"ApiKey", "nokey"}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            _service = new NewsService(_mapper, _config);


            var result = await Record.ExceptionAsync(() => _service.GetEverything(everythingRequest));

            Assert.AreEqual("ApiKeyInvalid : Your API key is invalid or incorrect. Check your key, or go to https://newsapi.org to create a free API key.", result.Message);
        }

        [Fact]
        public async void BasicTopHeadlinesRequest_Works()
        {
            var topHeadlinesRequest = new TopHeadlinesRequest();

            topHeadlinesRequest.Sources.Add("techcrunch");

            var result = await _service.GetTopHeadlines(topHeadlinesRequest);

            Assert.IsTrue(result.Count > 0);
        }

        [Fact]
        public async void BadTopHeadlinesRequest_ReturnsError()
        {
            var topHeadlinesRequest = new TopHeadlinesRequest();

            topHeadlinesRequest.Sources.Add("techcrunch");

            var myConfiguration = new Dictionary<string, string>
            {
                {"ApiKey", "nokey"}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            _service = new NewsService(_mapper, _config);

            var result = await Record.ExceptionAsync(() => _service.GetTopHeadlines(topHeadlinesRequest));

            Assert.AreEqual("ApiKeyInvalid : Your API key is invalid or incorrect. Check your key, or go to https://newsapi.org to create a free API key.", result.Message);
        }

        [Fact]
        public async void BadTopHeadlinesRequest_ReturnsError2()
        {
            var topHeadlinesRequest = new TopHeadlinesRequest();

            topHeadlinesRequest.Sources.Add("techcrunch");
            topHeadlinesRequest.Country = Countries.AU;
            topHeadlinesRequest.Language = Languages.EN;

            var result = await Record.ExceptionAsync(() => _service.GetTopHeadlines(topHeadlinesRequest));

            Assert.AreEqual("ParametersIncompatible : You cannot mix the sources parameter with the country or category parameters.", result.Message);
        }
    }
}
