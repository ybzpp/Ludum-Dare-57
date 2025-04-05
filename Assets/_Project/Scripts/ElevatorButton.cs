public class ElevatorButton : InteractButton
{
    public int FloorNumber;
    private string _elevatorKey;

    public override void Use()
    {
        base.Use();
        PadikService.CallElevator(_elevatorKey, FloorNumber);
    }

    public void SetElevatorKey(string value)
    {
        _elevatorKey = value;
    }
}
