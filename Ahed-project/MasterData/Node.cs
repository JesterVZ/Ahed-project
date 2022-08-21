using System.Collections.ObjectModel;

namespace Ahed_project.MasterData
{
    public class Node
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public ObservableCollection<Node> Nodes { get; set; }
    }
}
