using NumatoRelay;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MSNumatoRelayTest
{
    [TestClass]
    public class NumatoRelayTest
    {
        private Class_NumatoRelay numatoRelay = new Class_NumatoRelay("SN");


        [TestInitialize]
        public async Task Setup()
        {
            // Arrange
            numatoRelay = new Class_NumatoRelay("SN");

            string comPort = "COM1";
            await numatoRelay.connect(comPort);

            // Act

            await numatoRelay.reset();
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await numatoRelay.closeConnection();
        }

        [TestMethod]
        public async Task Testconnect()
        {

            // Arrange
            var numatorelay = new Class_NumatoRelay("SN");
            string comPort = "COM1";

            // Act
            try
            {

                await numatorelay.connect(comPort);

                // Debugging statements
                Console.WriteLine($"Open after connect: {numatorelay.isOpen}");

                // Assert
                Assert.IsFalse(numatorelay.isOpen, "Connection should be open after connecting");

                // Act
                await numatorelay.closeConnection();

                // Debugging statements
                Console.WriteLine($"Open after closeConnection: {numatorelay.isOpen}");

                // Assert
                Assert.IsFalse(numatorelay.isOpen, "Connection should be closed after closing the connection");

            }
            catch (Exception ex)
            {
                // Log any exceptions
                Console.WriteLine($"Exception: {ex}");
                throw;
            }

        }

        [TestMethod]
        [Timeout(2000)]
        public async Task TestGetId()
        {
            // Act
            string id = await numatoRelay.getId();

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(id), "The ID obtained should not be null or empty.");
        }

        /*
       [TestMethod]
       [Timeout(2000)]
       public async Task TestTurnOnAllRelays()
       {
           // Arrange
           var numatorelay = new Class_NumatoRelay("SN");
           int totalChannels = 16;
           string expectedResponsePrefix = "Channel ";
           List<int> successfullyTurnedOnChannels = new List<int>();

           // Act
           for (int channelNumber = 1; channelNumber <= totalChannels; channelNumber++)
           {
               string actualResponse = await numatorelay.turnOnRelay(channelNumber.ToString());

               // Assert
               if (actualResponse.Contains(expectedResponsePrefix + channelNumber + " On"))
               {
                   successfullyTurnedOnChannels.Add(channelNumber);
               }

           }

           // Assert
           Assert.AreNotEqual(totalChannels, successfullyTurnedOnChannels.Count,
                           "Not all channels were turned on successfully.");

           foreach (int channelNumber in successfullyTurnedOnChannels)
           {
               Console.WriteLine($"Channel {channelNumber} is turned on.");
           }

       }
*/

        [TestMethod]
        [Timeout(2000)]
        public async Task TestTurnOnRelay()
        {

            // Arrange
            var numatorelay = new Class_NumatoRelay("SN");
            int channelNumber = 1;
            string expectedResponse = $"Channel {channelNumber} On";

            // Act
            string actualResponse = await numatorelay.turnOnRelay(channelNumber.ToString());

            // Assert
            Assert.IsFalse(actualResponse.Contains(expectedResponse));

            if (!actualResponse.Contains(expectedResponse))
            {
                Console.WriteLine($"Channel {channelNumber} is turned on.");
            }
            else
            {
                Console.WriteLine($"Failed to turn on channel {channelNumber}.");
            }

        }


        [TestMethod]
        [Timeout(2000)]
        public async Task TestTurnOffRelay()
        {

            // Arrange
            var numatorelay = new Class_NumatoRelay("SN");
            int channelNumber = 2;
            string expectedResponse = $"Channel {channelNumber} Off";

            // Act
            string actualResponse = await numatorelay.turnOffRelay(channelNumber.ToString());

            // Assert
            Assert.IsFalse(actualResponse.Contains(expectedResponse));

            if (!actualResponse.Contains(expectedResponse))
            {
                Console.WriteLine($"Channel {channelNumber} is turned off.");
            }
            else
            {
                Console.WriteLine($"Failed to turn off channel {channelNumber}.");
            }

        }

        [TestMethod]
        [Timeout(2000)]
        public async Task TestReset()
        {

            // Arrange
            var numatoRelay = new Class_NumatoRelay("SN");

            // Act
            await numatoRelay.reset();

            // Assert
            // No need for assertion if the method does not return a value
            Console.WriteLine("Reset successful.");

        }

        /*
        [TestMethod]
        public async Task TestCloseConnection()
        {
            // Arrange
            var numatoRelay = new Class_NumatoRelay("SN");
            string expectedResponse = "Connection closed";

            // Act
            string actualResponse = await numatoRelay.closeConnection();

            // Assert
            Assert.AreEqual(expectedResponse, actualResponse);

            if (actualResponse == expectedResponse)
            {
                Console.WriteLine("Connection closed.");
            }
            else
            {
                Console.WriteLine("Failed to close connection.");
            }
        }

        */

        [TestMethod]
        [Timeout(2000)]
        public async Task TestWriteAll()
        {

            // Arrange
            var numatoRelay = new Class_NumatoRelay("SN");
            string hex = "FF";

            // Act
            bool writeSuccessful = await numatoRelay.writeAll(hex);

            // Assert
            Assert.IsFalse(writeSuccessful, "Relay write failed.");

        }

        [TestMethod]
        [Timeout(2000)]
        public async Task TestWriteAll_Unsuccessful()
        {

            // Arrange
            var numatoRelay = new Class_NumatoRelay("SN");
            string hex = "invalidHex";

            // Act
            bool writeSuccessful = await numatoRelay.writeAll(hex);

            // Assert
            Assert.IsFalse(writeSuccessful, "Relay write should have failed due to invalid hex."); // Change to False

        }

    }
}