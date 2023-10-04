using API.Data.Dtos;
using API.Data.Entities;
using API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/saves")]
    public class SavesController : ControllerBase
    {
        private ISaveRepository _repository;

        public SavesController(ISaveRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<SaveDTO>> GetAll()
        {
            var saves = await _repository.GetAllAsync();
            return saves.Select(x => new SaveDTO(x.Id, x.Money, x.Created));
        }

        [HttpGet]
        [Route("{id}", Name = "GetSave")]
        public async Task<ActionResult<SaveDTO>> Get(int id) 
        {
            var save = await _repository.GetAsync(id);
            if (save == null)
                return NotFound();

            return new SaveDTO(save.Id, save.Money, save.Created);
        }

        [HttpPost]
        public async Task<ActionResult> Create(PostSaveDTO save)
        {
            var saveDB = new Save()
            {
                Money = save.money,
                Created = DateTime.UtcNow
            };

            await _repository.CreateAsync(saveDB);
            return Created("", new SaveDTO(saveDB.Id, saveDB.Money, saveDB.Created));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<SaveDTO>> Update(int id, PostSaveDTO save)
        {
            var saveDB = await _repository.GetAsync(id);

            if (saveDB == null)
                return NotFound();

            saveDB.Money = save.money;
            saveDB.Created = DateTime.UtcNow;

            await _repository.UpdateAsync(saveDB);
            return Ok(new SaveDTO(saveDB.Id, saveDB.Money, saveDB.Created));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var saveDB = await _repository.GetAsync(id);

            if (saveDB == null)
                return NotFound();

            await _repository.DeleteAsync(saveDB);

            return NoContent();
        }
    }
}
