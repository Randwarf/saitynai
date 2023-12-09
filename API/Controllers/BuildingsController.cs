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
    [Route("api/saves/{saveId}/buildings")]
    public class BuildingsController : ControllerBase
    {
        private IBuildingRepository _buildingRep;
        private ISaveRepository _saveRep;
        private readonly IAuthorizationService _authorizationService;

        public BuildingsController(IBuildingRepository repository, ISaveRepository saveRep, IAuthorizationService authorizationService)
        {
            _buildingRep = repository;
            _saveRep = saveRep;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize(Roles = GameRoles.User)]
        public async Task<ActionResult<IEnumerable<BuildingDTO>>> GetAll(int saveId)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var buildings = await _buildingRep.GetAllAsync(saveId);
            return Ok(buildings.Where(building => building.OwnerId == User.FindFirstValue(JwtRegisteredClaimNames.Sub))
                .Select(x => new BuildingDTO(x.Id, x.Level, x.Name, x.Save.Id, x.X, x.Y)));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = GameRoles.User)]
        public async Task<ActionResult<BuildingDTO>> Get(int saveId, int id)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var building = await _buildingRep.GetAsync(saveId, id);
            if (building == null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, building, PolicyNames.ResourceOwner);
            if (!authResult.Succeeded)
                return NotFound();

            return new BuildingDTO(building.Id, building.Level, building.Name, building.Save.Id, building.X, building.Y);
        }

        [HttpPost]
        [Authorize(Roles = GameRoles.User)]
        public async Task<ActionResult> Create(int saveId, PostBuildingDTO buildingDTO)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var buildingDB = new Building()
            {
                Name = buildingDTO.name,
                Level = buildingDTO.level,
                Save = save,
                OwnerId = User.FindFirstValue(JwtRegisteredClaimNames.Sub),
                X= buildingDTO.X,
                Y= buildingDTO.Y
            };

            await _buildingRep.CreateAsync(buildingDB);
            return Created("", new BuildingDTO(buildingDB.Id, buildingDB.Level, buildingDB.Name, buildingDB.Save.Id, buildingDB.X, buildingDB.Y));
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = GameRoles.Tester)]
        public async Task<ActionResult<BuildingDTO>> Update(int saveId, int id, PostBuildingDTO buildingDTO)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var building = await _buildingRep.GetAsync(saveId, id);
            if (building == null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, building, PolicyNames.ResourceOwner);
            if (!authResult.Succeeded)
                return NotFound();

            building.Level = buildingDTO.level;
            building.Name = buildingDTO.name;

            await _buildingRep.UpdateAsync(building);
            return Ok(new BuildingDTO(building.Id, building.Level, building.Name, building.Save.Id, building.X, building.Y));
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = GameRoles.User)]
        public async Task<ActionResult> Delete(int saveId, int id)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var building = await _buildingRep.GetAsync(saveId, id);
            if (building == null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, building, PolicyNames.ResourceOwner);
            if (!authResult.Succeeded)
                return NotFound();

            await _buildingRep.DeleteAsync(building);

            return NoContent();
        }
    }
}
