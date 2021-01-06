using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit;
using NUnit.Framework;
using POS;

namespace POS_Tests.security_Tests
{
    [TestClass]
    public class Hasher_Tests
    {
        private POS.Security.Hasher hasher;

        [TestMethod]
        public void computeHash_Test()
        {
            // arrange
            this.hasher = new POS.Security.Hasher();
            this.hasher.fullName = "John Smith";
            this.hasher.password = "Password12345";
            string expectedHash = "148250143217662491659244514718712621202151538513214177101193240141168932021609611";

            // act
            string resultingHash = hasher.computeHash();

            // assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedHash, resultingHash);
        }
    }
}
