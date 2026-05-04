using MediatR;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Domain.Enums;
using MeetIQ.Application.Common.Exceptions;

namespace MeetIQ.Application.Features.Feedback.Commands.CreateFeedbackCommand
{
    public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, Guid>
    {
        private readonly IFeedbackRepository feedbackRepository;
        private readonly IUserRepository userRepository;

        public CreateFeedbackCommandHandler(
            IFeedbackRepository feedbackRepository,
            IUserRepository userRepository)
        {
            this.feedbackRepository = feedbackRepository;
            this.userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(x => x.Id == request.ReporterId);

            if (user == null)
                throw new NotFoundException("User not found");

            var feedback = new FeedbackReport
            {
                Id = Guid.NewGuid(),
                Type = request.Type,
                Message = request.Message,
                Status = FeedbackStatus.Open,
                ReporterId = request.ReporterId,
                CreatedAt = DateTime.UtcNow
            };

            await feedbackRepository.AddAsync(feedback);
            await feedbackRepository.SaveChangesAsync();

            return feedback.Id;
        }
    }
}