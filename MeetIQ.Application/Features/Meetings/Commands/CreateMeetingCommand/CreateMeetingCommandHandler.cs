using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.Commands.CreateMeetingCommand
{
    public class CreateMeetingCommandHandler : IRequestHandler<CreateMeetingCommand, Guid>
    {
        private readonly IMeetingRepository meetingRepository;
        private readonly IUserRepository userRepository;

        public CreateMeetingCommandHandler(
            IMeetingRepository meetingRepository,
            IUserRepository userRepository)
        {
            this.meetingRepository = meetingRepository;
            this.userRepository = userRepository;
        }

        public async Task<Guid> Handle(
            CreateMeetingCommand request,
            CancellationToken cancellationToken)
        {
            var host = await userRepository.GetAsync(x => x.Id == request.HostId);
            if (host == null)
                throw new NotFoundException("Host user not found");

            // Generate a unique Jitsi room id: slugified title + short guid
            var slug = new string(request.Title
                .ToLower()
                .Where(c => char.IsLetterOrDigit(c) || c == ' ')
                .ToArray())
                .Replace(' ', '-');

            var jitsiRoomId = $"meetiq-{slug}-{Guid.NewGuid().ToString("N")[..8]}";

            var meeting = new Meeting
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                JitsiRoomId = jitsiRoomId,
                ScheduledAt = request.ScheduledAt,
                Status = MeetingStatus.Scheduled,
                HostId = request.HostId,
                CreatedAt = DateTime.UtcNow
            };

            await meetingRepository.AddAsync(meeting);
            await meetingRepository.SaveChangesAsync();

            return meeting.Id;
        }
    }
}