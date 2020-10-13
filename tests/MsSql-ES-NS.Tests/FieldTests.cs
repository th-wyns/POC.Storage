using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
        public async void CreateWithNumber()
        {
            await Storage.Metadata.Field.CreateAsync(FieldUtilities.GetRandomField(FieldType.Number), CancellationToken.None);
        }

        [Fact]
        public async void CreateWithCode()
        {
            var field = FieldUtilities.GetRandomField(FieldType.Code);
            await Storage.Metadata.Field.CreateAsync(field, CancellationToken.None);
            var field2 = await Storage.Metadata.Field.FindByIdAsync(field.Id, CancellationToken.None);
            Assert.True(field.CodeConfiguration.SameAs(field2.CodeConfiguration));
        }
    }
}
