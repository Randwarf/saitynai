namespace API.Data.Dtos
{
    public record WorkerDTO(int id, int level, string name, int buildingId);
    public record PostWorkerDTO(int level, string name, int buildingID);
}
