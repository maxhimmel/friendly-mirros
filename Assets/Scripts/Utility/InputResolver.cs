namespace Sandbox.Utility
{
	public class InputResolver
    {
        public Rewired.Player GetInput( int id )
		{
			return Rewired.ReInput.players.GetPlayer( id );
		}
    }
}
