﻿using Microsoft.Extensions.Configuration;

using System;

using Xunit;

namespace pdi.Asset.Tests
{
    public class HostTests
    {
        [Fact]
        public void HostTest()
        {
            var i = new Host();
            Assert.Null(i.Name);
            Assert.Null(i.Address);
            Assert.Equal(22, i.Port);
            Assert.Null(i.UserName);
            Assert.Null(i.Password);
            Assert.Null(i.PrivateKey);
            Assert.Null(i.PrivateKeyPassword);
        }

        [Fact]
        public void ConnectExceptionTest()
        {
            using (var i = new Host())
            {
                Assert.Throws<ArgumentNullException>(nameof(Host.Address), () => i.Connect());
            }

            using (var i = new Host()
            {
                Address = ""
            })
            {
                Assert.Throws<ArgumentNullException>(nameof(Host.UserName), () => i.Connect());
            }

            using (var i = new Host()
            {
                Address = "",
                UserName = ""
            })
            {
                Assert.Throws<ArgumentNullException>(nameof(Host.PrivateKey), () => i.Connect());
            }
        }

        [Fact]
        public void ConnectPasswordTest()
        {
            var secrets = new ConfigurationBuilder().AddUserSecrets<HostTests>().Build();

            var i = new Host()
            {
                Address = secrets["SSH_ADDRESS"],
                Port = Convert.ToInt32(secrets["SSH_PORT"]),
                UserName = secrets["SSH_USERNAME"],
                Password = secrets["SSH_PASSWORD"]
            };

            i.Connect();
        }

        [Fact(Skip = "Current private key is not supported in SSH.NET 2020.0.1. See https://github.com/sshnet/SSH.NET/pull/614")]
        public void ConnectPrivateKeyTest()
        {
            var secrets = new ConfigurationBuilder().AddUserSecrets<HostTests>().Build();

            var i = new Host()
            {
                Address = secrets["SSH_ADDRESS"],
                Port = Convert.ToInt32(secrets["SSH_PORT"]),
                UserName = secrets["SSH_USERNAME"],
                PrivateKey = secrets["SSH_PRIVATE_KEY"],
                PrivateKeyPassword = secrets["SSH_PRIVATE_KEY_PASSWORD"]
            };

            i.Connect();
        }
    }
}