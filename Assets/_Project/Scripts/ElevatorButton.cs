public class ElevatorButton : InteractButton
{
    public string ElevatorKey;
    public int FloorNumber;

    public override void Use()
    {
        base.Use();

        PadikService.CallElevator(ElevatorKey, FloorNumber);
    }
}
