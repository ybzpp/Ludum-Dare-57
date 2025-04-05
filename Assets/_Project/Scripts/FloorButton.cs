public class FloorButton : InteractButton
{
    public string ElevatorKey;
    private int _floorNumber;

    public override void Use()
    {
        base.Use();
        PadikService.CallElevator(ElevatorKey, _floorNumber);
    }

    public void SetFloorNumber(int value)
    {
        _floorNumber = value;
    }
}
