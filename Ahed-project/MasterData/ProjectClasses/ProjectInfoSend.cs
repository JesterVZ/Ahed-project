﻿namespace Ahed_project.MasterData.ProjectClasses
{
    public class ProjectInfoSend
    {
        public int Id { get; set; }
        public int Revision { get; set; }
        public string Name { get; set; }
        public string Customer { get; set; }
        public string Contact { get; set; }
        public string CustomerReference { get; set; }
        public string Description { get; set; }
        public string Units { get; set; }
        public int NumberOfDecimals { get; set; }
        public string Category { get; set; }
        public string Keywords { get; set; }
        public string Comments { get; set; }
    }
}
