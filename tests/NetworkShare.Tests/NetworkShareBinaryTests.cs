using Xunit;

namespace POC.Storage.NetworkShare.Tests
{
    public class NetworkShareBinaryTests
    {
        static Connection Connection { get; } = Connection.Instance;
        public NetworkShareBinaryProvider Provider { get; } = new NetworkShareBinaryProvider(Connection.ConnectionString, Connection.ProjectId);

        [Fact]
        public void Create()
        {
            var inStream = BinaryUtilities.GenerateRandomStream(1024);
            var reference = Provider.CreateRandomBinary(stream: inStream);
            using var outStream = BinaryUtilities.GetStream(reference);
            Assert.True(BinaryUtilities.GetSize(reference) == inStream.Length);
            inStream.Seek(0, System.IO.SeekOrigin.Begin);
            Assert.True(outStream.StreamEquals(inStream));
        }

        [Fact]
        public void GetStream()
        {
            var inStream = BinaryUtilities.GenerateRandomStream(1024);
            var reference = Provider.CreateRandomBinary(stream: inStream);
            using var outStream = Provider.GetStream(reference);
            Assert.True(outStream.Length == inStream.Length);
            inStream.Seek(0, System.IO.SeekOrigin.Begin);
            Assert.True(outStream.StreamEquals(inStream));
        }

        [Fact]
        public void Delete()
        {
            var reference = Provider.CreateRandomBinary();
            Assert.True(BinaryUtilities.Exists(reference));
            Provider.Delete(reference);
            Assert.True(!BinaryUtilities.Exists(reference));
        }
    }
}
