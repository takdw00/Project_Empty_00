using UnityEngine.Events;

public class CharacterAnimEvents_Playable : CharacterAnimEvents
{
    public UnityEvent Event_OnDodgeEnd;

    public void OnDodgeEnd()
    {
        Event_OnDodgeEnd.Invoke();
    }

}
