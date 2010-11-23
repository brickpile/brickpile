using StructureMap;

namespace Stormbreaker.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class StructureMapBootstrapper
    {
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public static void Bootstrap(IContainer container)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public static void Bootstrap(IContainer container)
        {
            container.Configure(x => x.Scan(a =>
                                                {
                                                    a.AssembliesFromApplicationBaseDirectory();
                                                    a.LookForRegistries();                                                    
                                                }));
        }
        #endregion
    }
}