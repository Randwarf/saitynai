using API.Data.Dtos;
using API.Data.Entities;
using API.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Auth;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace API.Controllers
{
    [ApiController]
    [Route("api/saves")]
    public class SavesController : ControllerBase
    {
        private ISaveRepository _repository;
        private readonly IAuthorizationService _authorizationService;

        public SavesController(ISaveRepository repository, IAuthorizationService authorizationService)
        {
            _repository = repository;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize(Roles = GameRoles.User)]
        public async Task<IEnumerable<SaveDTO>> GetAll()
        {
            var saves = await _repository.GetAllAsync();

            return saves.Where(save => save.OwnerId == User.FindFirstValue(JwtRegisteredClaimNames.Sub))
                        .Select(x => new SaveDTO(x.Id, x.Money, x.Created));
        }

        [HttpGet]
        [Authorize(Roles = GameRoles.User)]
        [Route("{id}", Name = "GetSave")]
        public async Task<ActionResult<SaveDTO>> Get(int id) 
        {
            var saveDb = await _repository.GetAsync(id);
            if (saveDb == null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, saveDb, PolicyNames.ResourceOwner);
            if (!authResult.Succeeded)
            {
                return NotFound();
            }

            return new SaveDTO(saveDb.Id, saveDb.Money, saveDb.Created);
        }

        [HttpPost]
        [Authorize(Roles = GameRoles.User)]
        public async Task<ActionResult> Create(PostSaveDTO save)
        {
            var saveDB = new Save()
            {
                Money = save.money,
                Created = DateTime.UtcNow,
                OwnerId = User.FindFirstValue(JwtRegisteredClaimNames.Sub),
            };

            await _repository.CreateAsync(saveDB);
            return Created("", new SaveDTO(saveDB.Id, saveDB.Money, saveDB.Created));
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = GameRoles.Tester)]
        public async Task<ActionResult<SaveDTO>> Update(int id, PostSaveDTO save)
        {
            var saveDB = await _repository.GetAsync(id);

            if (saveDB == null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, saveDB, PolicyNames.ResourceOwner);
            if (!authResult.Succeeded)
            {
                return NotFound();
            }

            saveDB.Money = save.money;
            saveDB.Created = DateTime.UtcNow;

            await _repository.UpdateAsync(saveDB);
            return Ok(new SaveDTO(saveDB.Id, saveDB.Money, saveDB.Created));
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = GameRoles.User)]
        public async Task<ActionResult> Delete(int id)
        {
            var saveDB = await _repository.GetAsync(id);

            if (saveDB == null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, saveDB, PolicyNames.ResourceOwner);
            if (!authResult.Succeeded)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(saveDB);

            return NoContent();
        }
    }
}
