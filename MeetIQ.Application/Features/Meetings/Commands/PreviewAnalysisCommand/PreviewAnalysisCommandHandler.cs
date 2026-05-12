using MediatR;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Interfaces.Services;
using MeetIQ.Application.Features.Meetings.DTOs;
using Microsoft.Extensions.Logging;

namespace MeetIQ.Application.Features.Meetings.Commands.PreviewAnalysisCommand
{
    public class PreviewAnalysisCommandHandler
        : IRequestHandler<PreviewAnalysisCommand, PreviewAnalysisResult>
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IAnalysisService _analysisService;
        private readonly ILogger<PreviewAnalysisCommandHandler> _logger;

        public PreviewAnalysisCommandHandler(
            IMeetingRepository meetingRepository,
            IAnalysisService analysisService,
            ILogger<PreviewAnalysisCommandHandler> logger)
        {
            _meetingRepository = meetingRepository;
            _analysisService = analysisService;
            _logger = logger;
        }

        public async Task<PreviewAnalysisResult> Handle(
            PreviewAnalysisCommand request, CancellationToken ct)
        {
            var meeting = await _meetingRepository
                .GetMeetingWithTranscription(request.MeetingId, ct);

            if (meeting is null)
                return new(false, "Meeting not found.", null);

            if (meeting.HostId != request.RequestedByUserId)
                return new(false, "Only the host can analyze.", null);

            if (meeting.Transcript is null ||
                string.IsNullOrWhiteSpace(meeting.Transcript.Text))
                return new(false, "Transcript not available.", null);

            try
            {
                var analysis = await _analysisService.AnalyzeTranscriptAsync(
                    meeting.Transcript.Text, meeting.Title, ct);

                var preview = new AnalysisPreviewDto
                {
                    Summary = analysis.Summary,
                    KeyInsights = analysis.KeyInsights,
                    KeyDecisions = analysis.KeyDecisions ?? [],
                    Tasks = analysis.Tasks.Select((t, i) => new PreviewTaskDto
                    {
                        TempId = i,
                        Title = t.Title,
                        Description = t.Description,
                        Priority = t.Priority,
                        DueDate = t.DueDate,
                    }).ToList(),
                    Notes = analysis.Notes.Select((n, i) => new PreviewNoteDto
                    {
                        TempId = i,
                        Title = n.Title,
                        Content = n.Content,
                    }).ToList(),
                };

                return new(true, null, preview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Preview analysis failed for meeting {Id}", meeting.Id);
                return new(false, ex.Message, null);
            }
        }
    }
}