using System;
using System.Linq;
using BrickPile.Core;
using BrickPile.Core.Conventions;
using StructureMap;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Initialiser), "PreStart")]
namespace BrickPile.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Initialiser
    {
        private static IBrickPileBootstrapper _brickPileBootstrapper;
        private static bool _initialised;

        /// <summary>
        /// Pres the start.
        /// </summary>
        /// <exception cref="System.Exception">Unexpected second call to PreStart</exception>
        public static void PreStart()
        {
            if (_initialised)
            {
                throw new Exception("Unexpected second call to PreStart");
            }

            _initialised = true;

            // Get the first non-abstract implementation of IBrickPileBootstrapper if one exists in the
            // app domain. If none exist then just use the default one.
            var bootstrapperInterface = typeof(IBrickPileBootstrapper);
            var defaultBootstrapper = typeof(DefaultBrickPileBootstrapper);

            var locatedBootstrappers =
                from asm in AppDomain.CurrentDomain.GetAssemblies() // TODO ignore known assemblies like m$ and such
                from type in asm.GetTypes()
                where bootstrapperInterface.IsAssignableFrom(type)
                where !type.IsInterface
                where type != defaultBootstrapper
                select type;

            var bootStrapperType = locatedBootstrappers.FirstOrDefault() ?? defaultBootstrapper;

            _brickPileBootstrapper = (IBrickPileBootstrapper) Activator.CreateInstance(bootStrapperType);

            _brickPileBootstrapper.Initialise();

        }
    }
}