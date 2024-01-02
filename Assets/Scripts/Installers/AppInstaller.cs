using Sandbox.Utility;
using Zenject;

namespace Sandbox.Installers
{
	public class AppInstaller : MonoInstaller
    {
		public override void InstallBindings()
		{
			BindInput();
		}

		private void BindInput()
		{
			Container.Bind<InputResolver>()
				.AsSingle();

			Container.Bind<Rewired.Player>()
				.FromResolveGetter<InputResolver>( resolver => resolver.GetInput( 0 ) )
				.AsSingle();
		}
	}
}
