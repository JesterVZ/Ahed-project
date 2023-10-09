namespace Ahed_project.MasterData.ProjectClasses
{
    public class ProjectInfoGet
    {
        public int project_id { get; set; }
        public int? revision { get; set; }
        public string name { get; set; }
        public string customer { get; set; }
        public string contact { get; set; }
        public string customer_reference { get; set; }
        public string description { get; set; }
        public string units { get; set; }
        public int? number_of_decimals { get; set; }
        public string category { get; set; }
        public string keywords { get; set; }
        public string comments { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string user_id { get; set; }
        public string owner { get; set; }
    }
}
