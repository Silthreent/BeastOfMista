
public class DropOffState : IState
{
    Building DropoffPoint;
    Item DropoffItem;

    string DebugLastCheckAmount;

    public DropOffState(Building target, Item item)
    {
        DropoffPoint = target;
        DropoffItem = item;
    }

    public void Start(Character target)
    {
    }

    public void Process(Character target, float delta)
    {
        if (target.GlobalPosition.DistanceTo(DropoffPoint.GlobalPosition) > 10)
        {
            target.AI.InterruptState(new MovingState(DropoffPoint.GlobalPosition));
        }
        else
        {
            var amount = 5 * delta;
            if (target.Inventory[DropoffItem] < amount)
                amount = target.Inventory[DropoffItem];

            target.Inventory.LoseItem(DropoffItem, amount);
            DropoffPoint.Storage.GainItem(DropoffItem, amount);
            DebugLastCheckAmount = target.Inventory[DropoffItem].ToString("#.##");

            if (target.Inventory[DropoffItem] <= 0)
                target.AI.FinishState();
        }
    }

    public void End(Character target)
    {
    }

    public string GetDebugInfo()
    {
        return DebugLastCheckAmount + "|" + DropoffPoint.Storage[DropoffItem].ToString("#.##");
    }
}
