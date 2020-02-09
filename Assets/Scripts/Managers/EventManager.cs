using System;

public class EventManager
{
    //SUBSCRIBER EVENTS
    //Subscribe (+=) to be notified when an event is broadcast
    public static event Action PlayerDied;
    public static event Action EnemyDied;


    //BROADCASTER METHODS
    //Call to broadcast event
    public static void NotifyPlayerDied()
    { PlayerDied?.Invoke(); }
    public static void NotifyEnemyDied()
    { EnemyDied?.Invoke(); }
}