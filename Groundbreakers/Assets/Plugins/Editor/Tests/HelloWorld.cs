using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class HelloWorld
    {
        [Test]
        public void HelloWorldSimplePasses()
        {
            const bool IsActive = false;

            Assert.AreEqual(false, IsActive);
        }

        [Test]
        public void CatchingErrors()
        {
            var gameObject = new GameObject("test");

            Assert.Throws<MissingComponentException>(
                () => gameObject.GetComponent<Rigidbody>().velocity = Vector3.one);
        }
    }
}
