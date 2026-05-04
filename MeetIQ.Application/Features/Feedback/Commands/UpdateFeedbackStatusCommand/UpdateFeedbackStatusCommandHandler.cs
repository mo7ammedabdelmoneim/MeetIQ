using MediatR;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Common.Exceptions;

namespace MeetIQ.Application.Features.Feedback.Commands.UpdateFeedbackStatusCommand
{
    public class UpdateFeedbackStatusCommandHandler : IRequestHandler<UpdateFeedbackStatusCommand, bool>
    {
        private readonly IFeedbackRepository feedbackRepository;

        public UpdateFeedbackStatusCommandHandler(IFeedbackRepository feedbackRepository)
        {
            this.feedbackRepository = feedbackRepository;
        }

        public async Task<bool> Handle(UpdateFeedbackStatusCommand request, CancellationToken cancellationToken)
        {
            var feedback = await feedbackRepository.GetAsync(x => x.Id == request.FeedbackId);

            if (feedback == null)
                throw new NotFoundException("Feedback report not found");

            feedback.Status = request.NewStatus;

            feedbackRepository.Update(feedback);
            await feedbackRepository.SaveChangesAsync();

            return true;
        }
    }
}