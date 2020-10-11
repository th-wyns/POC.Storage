using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace POC.Storage.MsSqlESNS.Tests
{

    public class FieldTests
    {
        static Connection Connection { get; }
        static public Storage Storage { get; }

        static FieldTests()
        {
            // make sure Connection is initialized first since it is Dropping the DB
            Connection = Connection.Instance;
            Storage = new Storage(Connection.ProjectId);
        }

        [Fact]
        public void InitSucceeded()
        {
            Assert.True(Connection.TableExists("Field"));
        }


        [Fact]
        public void Create()
        {
            
        }
    }
}
