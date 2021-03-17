using System.Collections.Generic;
using Xunit;

namespace TrainEngine.Tests
{
    public class TrackOrmTests
    {
        
        [Fact]
        public void When_Reading_Stations_Expect_ListOfStations()
        {
            var stations = StationsOrm.GetStations(@"Data\stations.txt");
            Assert.IsType<List<Station>>(stations);
        }

        [Fact]
        public void When_Reading_Trains_Expect_ListOfTrains()
        {
            var trains = TrainOrm.GetTrains(@"Data\trains.txt");
            Assert.IsType<List<Train>>(trains);
        }

        [Fact]
        public void When_Using_ParseTrackDescription_Expect_TrackDescription()
        {
            var trackDescription = TrackOrm.ParseTrackDescription(@"Data\traintrack1.txt");
            Assert.IsType<TrackDescription>(trackDescription);
        }

        [Fact]
        public void When_Using_ParseTrackDescriptionForTrainTrack2_Expect_TrackDescriptionWith28Parts()
        {
            var trackDescription = TrackOrm.ParseTrackDescription(@"Data\traintrack2.txt");
            Assert.Equal(28, trackDescription.NumberOfTrackParts);
        }
    }
}

