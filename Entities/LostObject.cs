namespace objetosPerdidos;
public class LostObject
{
    private int id; 
    private string description; 
    private string category; 
    private string placeFound; 
    private DateOnly dateFound; 
    private bool claimed; 
    private string ownerName;

    public int Id { get => id; set => id = value; }
    public string Description { get => description; set => description = value; }
    public string Category { get => category; set => category = value; }
    public string PlaceFound { get => placeFound; set => placeFound = value; }
    public DateOnly DateFound { get => dateFound; set => dateFound = value; }
    public bool Claimed { get => claimed; set => claimed = value; }
    public string OwnerName { get => ownerName; set => ownerName = value; }
}