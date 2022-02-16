using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.GenericClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.GenericClasses.Tests
{
    [TestClass()]
    public class IndexedGuidTests
    {
        [TestMethod]
        public void IndexedGuidFromGuid()
        {
            var g = new Guid("ab0f0402-97d0-43c2-8763-7d5e0cc00409");
            var i = new IndexedGuid(g);
            Assert.IsTrue(i.Guid == g);
        }

        [TestMethod]
        public void IndexedGuidFromString()
        {
            var g = new Guid("ab0f0402-97d0-43c2-8763-7d5e0cc00409");
            var i = new IndexedGuid(g.ToString());
            Assert.IsTrue(i.Guid == g);
        }

        [TestMethod]
        public void IndexedGuidWithIndex()
        {
            var indexedGuid = new IndexedGuid("ab0f0402-97d0-43c2-8763-7d5e0cc00409#1");
            Assert.IsTrue(indexedGuid.Index == 1);
        }
        
        [TestMethod]
        public void IndexedGuidShouldNotBeLessThanOne()
        {
            Assert.ThrowsException<ArgumentException>(() => new IndexedGuid("ab0f0402-97d0-43c2-8763-7d5e0cc00409#0"));
            Assert.ThrowsException<ArgumentException>(() => new IndexedGuid("ab0f0402-97d0-43c2-8763-7d5e0cc00409#"));
        }

        [TestMethod]
        public void IndexedGuidEquals()
        {
            string g = "ab0f0402-97d0-43c2-8763-7d5e0cc00409";
            Assert.IsTrue(new Guid(g) == new Guid(g), "Two Guids should be equal");
            Assert.IsTrue(new IndexedGuid(g) == new IndexedGuid(g), "Two IndexedGuids should be equal");
            Assert.IsTrue(new IndexedGuid(g) == new Guid(g), "A Guid and a IndexedGuid with the same value should be equal");

            Assert.IsFalse(new IndexedGuid(new Guid(g), 1) == new Guid(g), "IndexedGuid with index should never be equal to a Guid");
            Assert.IsFalse(new IndexedGuid(g + "#9999") == new Guid(g), "IndexedGuid with index should never be equal to a Guid");
        }
    }
}