using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Services;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Infrastructure.Jobs
{
    public class MeetingStartingSoonJob
    {
        private readonly IMeetingRepository meetingRepository;
        private readonly INotificationService notificationService;
        private readonly ILogger<MeetingStartingSoonJob> logger;

        public MeetingStartingSoonJob(
            IMeetingRepository meetingRepository,
            INotificationService notificationService,
            ILogger<MeetingStartingSoonJob> logger)
        {
            this.meetingRepository = meetingRepository;
            this.notificationService = notificationService;
            this.logger = logger;
        }

        // Runs every 15 minutes
        // Catches meetings starting in the next 14-16 min window
        public async Task RunAsync()
        {
            logger.LogInformation("[MeetingStartingSoonJob] Starting at {Time}", DateTime.UtcNow);

            var windowStart = DateTime.UtcNow.AddMinutes(14);
            var windowEnd = DateTime.UtcNow.AddMinutes(16);

            var meetings = await meetingRepository
                .GetMeetingsStartingBetweenAsync(windowStart, windowEnd);

            logger.LogInformation(
                "[MeetingStartingSoonJob] Found {Count} meetings starting soon", meetings.Count);

            int notified = 0;

            foreach (var meeting in meetings)
            {
                // don't send twice for same meeting 
                bool alreadySent = await meetingRepository
                    .WasNotifiedRecentlyAsync(
                        meeting.Id, NotificationType.MeetingStartingSoon, withinHours: 1);

                if (alreadySent) continue;

                var allUserIds = meeting.ParticipantIds
                    .Append(meeting.HostId)
                    .Distinct();

                // Notify everyone (host + all participants)
                var notifications = allUserIds.Select(userId =>
                    notificationService.NotifyAsync(
                        userId: userId,
                        type: NotificationType.MeetingStartingSoon,
                        title: "Meeting Starting Soon",
                        message: $"\"{meeting.Title}\" starts in ~15 minutes.",
                        actionUrl: $"/Meetings/Details/{meeting.Id}",
                        referenceId: meeting.Id,
                        referenceType: NotificationReferenceType.Meeting
                    ));

                await Task.WhenAll(notifications);
                notified++;
            }

            logger.LogInformation(
                "[MeetingStartingSoonJob] Processed {Count} meetings", notified);
        }
    }
}