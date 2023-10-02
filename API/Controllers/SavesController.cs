using API.Data.Dtos;
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
        [Route("{id}")]
        public void Get(int id) 
        {
            
        }

        [HttpPost]
        public void Create(SaveDTO save)
        {

        }

        [HttpPut]
        [Route("{id}")]
        public void Update(int id, SaveDTO save)
        {

        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {

        }
    }
}
