using API.Data.Entities;

namespace API.Data.Dtos
{
    public record BuildingDTO (int id, int level, string name, int saveID);
    public record PostBuildingDTO (int level, string name, int saveID);
}
