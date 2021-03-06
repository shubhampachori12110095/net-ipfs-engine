﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Ipfs.Engine
{
    [TestClass]
    public class GenericApiTest
    {
        const string marsId = "QmSoLMeWqB7YGVLJN3pNLQpmmEk35v6wYtsMGLzSr5QBU3";

        [TestMethod]
        public async Task Local_Info()
        {
            var ipfs = TestFixture.Ipfs;
            var peer = await ipfs.Generic.IdAsync();
            Assert.IsInstanceOfType(peer, typeof(Peer));
            Assert.IsNotNull(peer.Addresses);
            StringAssert.StartsWith(peer.AgentVersion, "net-ipfs/");
            Assert.IsNotNull(peer.Id);
            StringAssert.StartsWith(peer.ProtocolVersion, "ipfs/");
            Assert.IsNotNull(peer.PublicKey);

            Assert.IsTrue(peer.IsValid());
        }

        [TestMethod]
        [Ignore()]
        public async Task Mars_Info()
        {
            var ipfs = TestFixture.Ipfs;
            var peer = await ipfs.Generic.IdAsync(marsId);
            Assert.IsInstanceOfType(peer, typeof(Peer));
        }

        [TestMethod]
        public async Task Version_Info()
        {
            var ipfs = TestFixture.Ipfs;
            var versions = await ipfs.Generic.VersionAsync();
            Assert.IsNotNull(versions);
            Assert.IsTrue(versions.ContainsKey("Version"));
            Assert.IsTrue(versions.ContainsKey("Repo"));
        }

    }
}

