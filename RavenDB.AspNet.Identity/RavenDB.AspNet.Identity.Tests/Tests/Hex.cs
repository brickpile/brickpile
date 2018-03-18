using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Util = RavenDB.AspNet.Identity.Util;

namespace RavenDB.AspNet.Identity.Tests
{
    public class Hex
    {
        [Fact]
        public void Test_Hex_Roundtrip()
        {
            byte[] randomBytes = new byte[4096];
            Random rand = new Random();
            rand.NextBytes(randomBytes);

            string hex = Util.ToHex(randomBytes);
            Assert.Equal(hex.Length, 4096*2);

            byte[] roundtrip = Util.FromHex(hex);

            Assert.Equal(roundtrip, randomBytes);
        }

        [Fact]
        public void Hex_case_doesnt_matter()
        {
            byte[] b1 = Util.FromHex("0123456789ABCDEFabcdef0123456789");
            byte[] b2 = Util.FromHex("0123456789abcdefABCDEF0123456789");

            Assert.Equal(b1, b2);
        }
    }
}
