﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ConnectChain.Helpers;

namespace ConnectChain.Middlewares
{
    public class GlobalErrorHandlerMiddleware 






    {
        RequestDelegate _nextAction;
        ILogger<GlobalErrorHandlerMiddleware> _logger;
        public GlobalErrorHandlerMiddleware(RequestDelegate nextAction,
            ILogger<GlobalErrorHandlerMiddleware> logger)
        {
            _nextAction = nextAction;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _nextAction(context);
            }
            catch (Exception ex)
            {
                File.WriteAllText(@"F:\\log.txt", $"error{ex.Message}");
                var response = EndpointResponse<bool>.Failure(ErrorCode.BadRequest);
                await context.Response.WriteAsJsonAsync(response);
            }

        }
    }
    }
