namespace MinimalAPIDemo.Requests;
public record UserDTO
{
    public int RollNo { get; set; }
    public string UserName { get; set; } = string.Empty;
}
