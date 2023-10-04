using API.Data.Dtos;
using API.Data.Entities;
using API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/saves/{saveId}/buildings")]
    public class BuildingsController : ControllerBase
    {
        private IBuildingRepository _buildingRep;
        private ISaveRepository _saveRep;

        public BuildingsController(IBuildingRepository repository, ISaveRepository saveRep)
        {
            _buildingRep = repository;
            _saveRep = saveRep;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuildingDTO>>> GetAll(int saveId)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var buildings = await _buildingRep.GetAllAsync(saveId);
            return Ok(buildings.Select(x => new BuildingDTO(x.Id, x.Level, x.Name, x.Save.Id)));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<BuildingDTO>> Get(int saveId, int id)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var building = await _buildingRep.GetAsync(saveId, id);
            if (building == null)
                return NotFound();

            return new BuildingDTO(building.Id, building.Level, building.Name, building.Save.Id);
        }

        [HttpPost]
        public async Task<ActionResult> Create(int saveId, PostBuildingDTO buildingDTO)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var buildingDB = new Building()
            {
                Name = buildingDTO.name,
                Level = buildingDTO.level,
                Save = save
            };

            await _buildingRep.CreateAsync(buildingDB);
            return Created("", new BuildingDTO(buildingDB.Id, buildingDB.Level, buildingDB.Name, buildingDB.Save.Id));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<BuildingDTO>> Update(int saveId, int id, PostBuildingDTO buildingDTO)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var building = await _buildingRep.GetAsync(saveId, id);
            if (building == null)
                return NotFound();

            building.Level = buildingDTO.level;
            building.Name = buildingDTO.name;

            await _buildingRep.UpdateAsync(building);
            return Ok(new BuildingDTO(building.Id, building.Level, building.Name, building.Save.Id));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int saveId, int id)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var building = await _buildingRep.GetAsync(saveId, id);
            if (building == null)
                return NotFound();

            await _buildingRep.DeleteAsync(building);

            return NoContent();
        }
    }
}
