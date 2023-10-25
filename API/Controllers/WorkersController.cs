using API.Auth;
using API.Data.Dtos;
using API.Data.Entities;
using API.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/saves/{saveId}/buildings/{buildingId}/workers")]
    public class WorkersController : ControllerBase
    {
        private IWorkerRepository _workerRep;
        private IBuildingRepository _buildingRep;
        private ISaveRepository _saveRep;
        private readonly IAuthorizationService _authorizationService;

        public WorkersController(IWorkerRepository wRep, IBuildingRepository bRep, ISaveRepository saveRep, IAuthorizationService authorizationService)
        {
            _workerRep = wRep;
            _buildingRep = bRep;
            _saveRep = saveRep;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize(Roles = GameRoles.User)]
        public async Task<ActionResult<IEnumerable<WorkerDTO>>> GetAll(int saveId, int buildingId)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var building = await _buildingRep.GetAsync(saveId, buildingId);
            if (building == null)
                return NotFound();

            var workers = await _workerRep.GetAllAsync(saveId, buildingId);
            return Ok(workers.Where(w => w.OwnerId == User.FindFirstValue(JwtRegisteredClaimNames.Sub))
                             .Select(w => new WorkerDTO(w.Id, w.Level, w.Name, w.Building.Id)));
        }

        [HttpGet]
        [Route("{workerId}")]
        [Authorize(Roles = GameRoles.User)]
        public async Task<ActionResult<WorkerDTO>> Get(int saveId, int buildingId, int workerId)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var building = await _buildingRep.GetAsync(saveId, buildingId);
            if (building == null)
                return NotFound();

            var worker = await _workerRep.GetAsync(saveId, buildingId, workerId);
            if (worker == null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, worker, PolicyNames.ResourceOwner);
            if (!authResult.Succeeded)
                return NotFound();

            return new WorkerDTO(worker.Id, worker.Level, worker.Name, worker.Building.Id);
        }

        [HttpPost]
        [Authorize(Roles = GameRoles.User)]
        public async Task<ActionResult> Create(int saveId, int buildingId, PostWorkerDTO workerDTO)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var building = await _buildingRep.GetAsync(saveId, buildingId);
            if (building == null)
                return NotFound();

            var worker = new Worker()
            {
                Level = workerDTO.level,
                Name = workerDTO.name,
                Building = building,
                OwnerId = User.FindFirstValue(JwtRegisteredClaimNames.Sub),
            };

            await _workerRep.CreateAsync(worker);
            return Created("", new WorkerDTO(worker.Id, worker.Level, worker.Name, worker.Building.Id));
        }

        [HttpPut]
        [Route("{workerId}")]
        [Authorize(Roles = GameRoles.Tester)]
        public async Task<ActionResult<WorkerDTO>> Update(int saveId, int buildingId, int workerId, PostWorkerDTO workerDTO)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var building = await _buildingRep.GetAsync(saveId, buildingId);
            if (building == null)
                return NotFound();

            var worker = await _workerRep.GetAsync(saveId, buildingId, workerId);
            if (worker == null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, worker, PolicyNames.ResourceOwner);
            if (!authResult.Succeeded)
                return NotFound();

            worker.Level = workerDTO.level;
            worker.Name = workerDTO.name;

            await _workerRep.UpdateAsync(worker);
            return Ok(new WorkerDTO(worker.Id, worker.Level, worker.Name, worker.Building.Id));
        }

        [HttpDelete]
        [Route("{workerId}")]
        [Authorize(Roles = GameRoles.User)]
        public async Task<ActionResult> Delete(int saveId, int buildingId, int workerId)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var building = await _buildingRep.GetAsync(saveId, buildingId);
            if (building == null)
                return NotFound();

            var worker = await _workerRep.GetAsync(saveId, buildingId, workerId);
            if (worker == null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, worker, PolicyNames.ResourceOwner);
            if (!authResult.Succeeded)
                return NotFound();

            await _workerRep.DeleteAsync(worker);
            return NoContent();
        }
    }
}
