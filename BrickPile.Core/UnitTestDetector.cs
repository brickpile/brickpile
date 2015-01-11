using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickPile.Core
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Detect if we are running as part of a nUnit unit test.
    /// This is DIRTY and should only be used if absolutely necessary 
    /// as its usually a sign of bad design.
    /// </summary>    
    static class UnitTestDetector
    {

        private static bool _runningFromNUnit = false;

        static UnitTestDetector()
        {
            foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Can't do something like this as it will load the nUnit assembly
                // if (assem == typeof(NUnit.Framework.Assert))

                if (assem.FullName.ToLowerInvariant().StartsWith("nunit.framework"))
                {
                    _runningFromNUnit = true;
                    break;
                }
            }
        }

        internal static bool IsRunningFromNunit
        {
            get { return _runningFromNUnit; }
        }
    }
}
