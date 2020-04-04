// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Collections;
using System.Collections.Generic;
using ModbusTCP.Model;
using NUnit.Framework;

namespace ModbusTCP.Tests
{
    [TestFixture]
    public class MBTCPConnTest
    {
        private MBTCPConn _mbtcpConn;

        [SetUp]
        public void BaseSetUp()
        {
            _mbtcpConn = new MBTCPConn();
        }

        [Test]
        public void WrongIpAddressInputTest()
        {
            Assert.That(0, Is.Not.EqualTo(_mbtcpConn.SetSlaveIPv4Address("notGood")), "IP address set instead of wrong input");
        }

        [Test]
        public void WrongIpPortInputTest()
        {
            Assert.That(0, Is.Not.EqualTo(_mbtcpConn.SetSlaveIPPort(-1)), "IP port set instead of wrong input");
        }

        [Test]
        public void WrongIpPortInputTest2()
        {
            Assert.That(0, Is.Not.EqualTo(_mbtcpConn.SetSlaveIPPort(70000)), "IP port set instead of wrong input");
        }
    }
}
