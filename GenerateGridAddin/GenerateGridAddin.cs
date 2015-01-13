using System;
using Pinta.Core;

namespace GenerateGridAddin
{
	[Mono.Addins.Extension]
	public class GenerateGridAddin : Pinta.Core.IExtension
	{
		public void Initialize()
		{
			PintaCore.Effects.RegisterEffect (new GridEffect ());
		}
			
		public void Uninitialize()
		{
			PintaCore.Effects.UnregisterInstanceOfEffect (typeof(GridEffect));
		}

	}
}