using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Features.Notes.Commands.UpdateNoteCommand
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, bool>
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITagRepository _tagRepository;

        public UpdateNoteCommandHandler(INoteRepository noteRepository, ITagRepository tagRepository)
        {
            _noteRepository = noteRepository;
            _tagRepository = tagRepository;
        }

        public async Task<bool> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            var note = await _noteRepository.GetWithNoteTagsAsync(request.Id);
            if (note is null)
                throw new NotFoundException("Note not found");

            if (note.AuthorId != request.RequesterId)
                throw new BadRequestException("You are not allowed to edit this note");

            note.Title = request.Title;
            note.Content = request.Content;
            note.MeetingId = request.MeetingId;
            note.CalendarEventId = request.CalendarEventId;
            note.IsEdited = true;
            note.UpdatedAt = DateTime.UtcNow;

            // Re-sync tags: clear old ones, add new ones
            note.NoteTags.Clear();
            foreach (var tagName in request.Tags.Distinct())
            {
                var tag = await _tagRepository.GetOrCreateAsync(tagName);
                note.NoteTags.Add(new NoteTag { NoteId = note.Id, TagId = tag.Id });
            }

            _noteRepository.Update(note);
            await _noteRepository.SaveChangesAsync();

            return true;
        }
    }
}