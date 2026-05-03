using MediatR;
using MeetIQ.Application.Features.Notes.DTOs;

namespace MeetIQ.Application.Features.Notes.Queries.GetAllTagsQuery
{
    public class GetAllTagsQuery : IRequest<List<TagDto>>
    {
    }
}