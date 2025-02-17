using Microsoft.AspNetCore.Http.HttpResults;
using notebodia_api.Repositories;
using notebodia_api.Response;
using notebodia_api.Types;
using notebodia_api.Util;

namespace notebodia_api.Services
{
    public class NoteService
    {
        private readonly NoteRepository _noteRepository;
        private readonly UserService _userService;
        public NoteService(NoteRepository noteRepository, UserService userService)
        {
            _noteRepository = noteRepository;
            _userService = userService;
        }
        public async Task<List<Note>> GetAllAsync()
        {
            return await _noteRepository.GetAllAsync();
        }
        public async Task<List<Note>> GetAllByUserAsync(string? keyword)
        {
            var user = await _userService.GetMe();
            return await _noteRepository.GetAllById(user.Id, keyword);
        }
        public async Task<Note> CreateAsync(CreateNotePayload payload)
        {
            var user = await _userService.GetMe();
            return await _noteRepository.CreateAsync(user.Id, payload);
        }
        public async Task<Note> UpdateAsync(Guid NoteId, CreateNotePayload payload)
        {
            var note = await _noteRepository.GetById(NoteId);
            PrintUtils.PrettyPrint(note);
            if (note == null)
            {
                throw new NotFoundException();
            }
            var user = await _userService.GetMe();
            PrintUtils.PrettyPrint(user);
            if (note.UserId != user.Id)
            {
                throw new ForbiddenException("You have no edit access for this note");
            }
            return await _noteRepository.UpdateAsync(NoteId, payload);
        }
        public async Task DeleteAsync(Guid NoteId)
        {
            var note = await _noteRepository.GetById(NoteId);
            if (note == null)
            {
                throw new NotFoundException();
            }
            var user = await _userService.GetMe();
            PrintUtils.PrettyPrint(user);
            if (note.UserId != user.Id)
            {
                throw new ForbiddenException("You have no delete access for this note");
            }
            var isDeleted = await _noteRepository.DeleteAsync(NoteId);
            if (!isDeleted)
            {
                throw new InternalServerException();
            }
        }
    }

}