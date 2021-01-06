/*
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security;
using System.Diagnostics;

namespace UnitTests
{
    [TestClass]
    public class HasherTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // arrange
            string FullName = "Alex Chlabicz";
            string password = "Password12345";
            string expectedHash = "148250143217662491659244514718712621202151538513214177101193240141168932021609611";
            
            // act 
            Hasher hasher = new Hasher(FullName, password);
            string hash = hasher.computeHash();

            // assert
            Assert.AreEqual(hash, expectedHash);
        }
    }
}
*/