using Application.IServices;
using Common.Requests;
using Common.Responses;
using Common.Wrappers;
using Infrastructure.Constant;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BaseService<T>
    {
        protected readonly AppConfiguration _appConfiguration;
        protected readonly ApplicationDbContext _applicationDbContext;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger<T> _logger;


        public BaseService(AppConfiguration appConfiguration, ApplicationDbContext applicationDbContext,ILogger<T> logger)
        {
            _appConfiguration = appConfiguration;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }
    }
}