using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Features.Notes.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Notes.Queries.GetAllTagsQuery
{
    public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, List<TagDto>>
    {
        private readonly ITagRepository _tagRepository;

        public GetAllTagsQueryHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<List<TagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            return await _tagRepository.GetAllAsync();
        }
    }
}