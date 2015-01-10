using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BrickPile.Core;
using BrickPile.Core.Extensions;

namespace BrickPile.UI
{
	public partial class Startup
	{
        private static IBrickPileBootstrapper brickPileBootstrapper;

        public static void InitialiseBootstrapper()
        {			
            // Get the first non-abstract implementation of IBrickPileBootstrapper if one exists in the
            // app domain. If none exist then just use the default one.
            var bootstrapperInterface = typeof(IBrickPileBootstrapper);
            var defaultBootstrapper = typeof(DefaultBrickPileBootstrapper);

            var locatedBootstrappers =
                from asm in AppDomain.CurrentDomain.GetAssemblies() // TODO ignore known assemblies like m$ and such
                from type in asm.TryGetTypes()
                where bootstrapperInterface.IsAssignableFrom(type)
                where !type.IsInterface
                where type != defaultBootstrapper
                select type;

            var bootStrapperType = locatedBootstrappers.FirstOrDefault() ?? defaultBootstrapper;

            brickPileBootstrapper = (IBrickPileBootstrapper) Activator.CreateInstance(bootStrapperType);

            brickPileBootstrapper.Initialise();

        }
	}
}