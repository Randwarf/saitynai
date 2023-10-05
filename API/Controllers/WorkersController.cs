using API.Data.Dtos;
using API.Data.Entities;
using API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/saves/{saveId}/buildings/{buildingId}/workers")]
    public class WorkersController : ControllerBase
    {
        private IWorkerRepository _workerRep;
        private IBuildingRepository _buildingRep;
        private ISaveRepository _saveRep;

        public WorkersController(IWorkerRepository wRep, IBuildingRepository bRep, ISaveRepository saveRep)
        {
            _workerRep = wRep;
            _buildingRep = bRep;
            _saveRep = saveRep;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkerDTO>>> GetAll(int saveId, int buildingId)
        {
            var save = await _saveRep.GetAsync(saveId);
            if (save == null)
                return NotFound();

            var building = await _buildingRep.GetAsync(saveId, buildingId);
            if (building == null)
                return NotFound();

            var workers = await _workerRep.GetAllAsync(saveId, buildingId);
            return Ok(workers.Select(w => new WorkerDTO(w.Id, w.Level, w.Name, w.Building.Id)));
        }

        [HttpGet]
        [Route("{workerId}")]
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

            return new WorkerDTO(worker.Id, worker.Level, worker.Name, worker.Building.Id);
        }

        [HttpPost]
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
                Building = building
            };

            await _workerRep.CreateAsync(worker);
            return Created("", new WorkerDTO(worker.Id, worker.Level, worker.Name, worker.Building.Id));
        }

        [HttpPut]
        [Route("{workerId}")]
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

            worker.Level = workerDTO.level;
            worker.Name = workerDTO.name;

            await _workerRep.UpdateAsync(worker);
            return Ok(new WorkerDTO(worker.Id, worker.Level, worker.Name, worker.Building.Id));
        }

        [HttpDelete]
        [Route("{workerId}")]
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

            await _workerRep.DeleteAsync(worker);
            return NoContent();
        }
    }
}
