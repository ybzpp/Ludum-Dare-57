public class Doorbell : InteractButton
{
    public override void Use()
    {
        base.Use();
        AudioHelper.PlaySound("Doorbell", transform.position);
    }
}
