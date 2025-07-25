﻿using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MailKit.Search;
using MediatR;

namespace ConnectChain.Features.NotificationManagement.AddNotificaitonCommand
{
    public record AddNotificationCommand(string? Title, string? Body, string? Type,string SupplierId) : IRequest<RequestResult<bool>>;
    public class AddNotificationHandler(IRepository<Notification> repository) : IRequestHandler<AddNotificationCommand, RequestResult<bool>>
    {
        private readonly IRepository<Notification> repository = repository;

        public async Task<RequestResult<bool>> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new Notification()
            {
                Title = request.Title,
                Body = request.Body,
                UserId= request.SupplierId,
                Type = request.Type,
                IsRead = false,
            };
            await repository.AddAsync(notification);
            await repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "Notification Added");
        }
    }
}
