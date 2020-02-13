using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Assets.MonogoScripts
{
    //TODO Define Key Attributes
    [BsonIgnoreExtraElements]
    public class Report
    {
        // public string _id { get; set; }
        [Key]
        public string RunName { get; set; }
        [Key]
        public string ScenarioName { get; set; }
        [Key]
        public string ConfigurationName { get; set; }
        public DateTime ScenarioStartTime { get; set; }
        public DateTime ScenarioEndTime { get; set; }

        public string ScenarioEndedReason { get; set; }//TODO Enum collision \ end of way points \ configuration flipped
        public string ScenarioEndedConfigurationPosition { get; set; }

        public string ScenarioEndedConfigurationOrientation { get; set; }

        public List<ReceivedCommand> ReceivedCommands { get; set; } = new List<ReceivedCommand>(); //TODO List of commands - ODSU Send a stop message - events

        public List<Obstacle> Obstacles { get; set; } = new List<Obstacle>();

        public List<WayPoint> WayPoints { get; set; } = new List<WayPoint>();

        public Report(string runName, string scenarioName, string configurationName)
        {
            RunName = runName;
            ScenarioName = scenarioName;
            ConfigurationName = configurationName;
        }

        public Report()
        {

        }
    }
}
