using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData
{
    public class Node
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public ObservableCollection<Node> Nodes { get; set; }
    }
}
