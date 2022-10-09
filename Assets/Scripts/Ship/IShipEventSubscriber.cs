using System;

public interface IShipEventSubscriber
{
    public void OnReset(object sender, EventArgs e);
}