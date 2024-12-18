using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminNotificationService : IAdminNotificationService
    {
        private readonly IAdminNotificationRepository _notificationRepository;
        private readonly IRepository<Notification> _repository;
        private readonly IMapper _mapper;

        public AdminNotificationService(IAdminNotificationRepository bannerRepository, IRepository<Notification> repository, IMapper mapper)
        {
            _notificationRepository = bannerRepository;
            _repository = repository;
            _mapper = mapper;
        }

        public Task<PagedResponse<SearchNotificationDetail>> SearchNotificationsAsync(AdminSearchNotificationRequest request)
            => _notificationRepository.SearchNotificationsAsync(request, () => DateTime.Today);

        public async Task<GetNotificationResponse> GetNotificationAsync(int notificationId)
        {
            DateTime today = DateTime.Today;

            Notification notification = await _repository.GetSingleOrDefaultAsync(b => b.Id == notificationId);
            return _mapper.Map<GetNotificationResponse>(notification, opt => opt.Items["today"] = today);
        }

        public async Task<AddNotificationResponse> AddNotificationAsync(AddNotificationRequest request)
        {
            Notification notification = _mapper.Map<Notification>(request);
            bool success = await _repository.CreateAsync(notification) > 0;

            return new AddNotificationResponse
            {
                Success = success,
                NotificationId = notification.Id
            };
        }

        public async Task<bool> UpdateNotificationAsync(UpdateNotificationRequest request, Func<DateTime> getNow)
        {
            Notification notification = await _repository.GetSingleOrDefaultAsync(b => b.Id == request.NotificationId);

            if (notification == null)
            {
                return false;
            }

            notification.Title = request.Title;
            notification.Content = request.Content;
            notification.Target = request.Target;
            notification.Start = request.Start;
            notification.End = request.End;
            notification.ModifiedBy = request.ModifiedBy;
            notification.ModifiedOn = getNow();

            return await _repository.UpdateAsync(notification) > 0;
        }
    }
}