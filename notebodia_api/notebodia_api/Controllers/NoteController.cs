using Microsoft.AspNetCore.Mvc;
using notebodia_api.Response;
using notebodia_api.Services;
using notebodia_api.Types;

namespace notebodia_api.Controllers
{

    [Route("api/notes")]
    [ApiController]
    public class NoteController : ApiControllerBase
    {
        private readonly NoteService _noteService;

        public NoteController(NoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Note>>>> GetAllByUserAsync([FromQuery] string? keyword = null)
        {
            var notes = await _noteService.GetAllByUserAsync(keyword);
            return ApiOk(notes);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<Note>>> CreateAsync(CreateNotePayload payload)
        {
            var notes = await _noteService.CreateAsync(payload);
            return ApiOk(notes);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Note>>> UpdateAsync(Guid id, CreateNotePayload payload)
        {
            var notes = await _noteService.UpdateAsync(id, payload);
            return ApiOk(notes);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateAsync(Guid id)
        {
            await _noteService.DeleteAsync(id);
            return ApiSimpleSuccess();
        }
    }
}
