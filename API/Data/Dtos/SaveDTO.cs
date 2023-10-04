namespace API.Data.Dtos
{
    public record SaveDTO(int id, int money, DateTime timestamp);
    public record PostSaveDTO(int money);

}
