using System;
using System.Collections.Generic;
using Xunit;

namespace TrainEngine.Tests
{
    public class TrackOrmTests
    {
        //[Fact]
        //public void When_OnlyAStationIsProvided_Expect_TheResultOnlyToContainAStationWithId1()
        //{
        //    // Arrange
        //    string track = "[1]";
        //    TrackOrm trackOrm = new TrackOrm();

        //    // Act
        //    var result = trackOrm.ParseTrackDescription(track);

        //    // Assert
        //    //Assert.IsType<Station>(result.TackPart[0]);
        //    //Station s = (Station)result.TackPart[0];
        //    //Assert.Equal(1, s.Id);
        //}

        //[Fact]
        //public void When_ProvidingTwoStationsWithOneTrackBetween_Expect_TheTrackToConcistOf3Parts()
        //{
        //    // Arrange
        //    string track = "[1]-[2]";
        //    TrackOrm trackOrm = new TrackOrm();
            
        //    // Act
        //    var result = trackOrm.ParseTrackDescription(track);

        //    // Assert
        //    Assert.Equal(3, result.NumberOfTrackParts);
        //}

        //[Fact]
        //public void When_Using_TrainPlanner_Expect_ITravelPlan()
        //{
        //    Train train = new Train("Lapplandståget");
        //    Station station = new Station("Grand Retro");
        //    Station endStation = new Station("Uppsala");

        //    var travelPlan = new TrainPlanner(train, station).HeadTowards(station).StartTrainAt().StopTrainAt(endStation, "12:00").GeneratePlan();
        //}

        [Fact]
        public void When_Using_StartAt_ArriveAt_GeneratePlan_Expect_ITravelPlan()
        {
            var travelplan = new TravelPlan().StartAt("station1", "tid1").ArriveAt("station2", "tid2").GeneratePlan();

            Assert.IsType<TravelPlan>(travelplan);
        }

        [Fact]
        public void When_Reading_Stations_Expect_ListOfStations()
        {
            var stations = TravelPlan.GetStations(@"Data\stations.txt");
            Assert.IsType<List<Station>>(stations);
        }

        [Fact]
        public void When_Reading_Trains_Expect_ListOfTrains()
        {
            var trains = TravelPlan.GetTrains(@"Data\trains.txt");
            Assert.IsType<List<Train>>(trains);
        }

        [Fact]
    }
}
