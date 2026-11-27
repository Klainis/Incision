using System.Linq;
using System.Text;

namespace Incision.Room.Runtime
{
    public static class RoomGraphDebug
    {
        public static string Dump(this RoomGraph graph)
        {
            var names = graph.Layers
                .Select(l => l.Select(n => n.Template.Type.ToString()).ToList())
                .ToList();

            var cell = names.SelectMany(l => l).Max(s => s.Length) + 2;
            var width = cell * 2;
            var sb = new StringBuilder();

            for (var i = 0; i < names.Count; i++)
            {
                var cur = names[i];
                sb.AppendLine((cur.Count == 1
                    ? Center(cur[0], width)
                    : Center(cur[0], cell) + Center(cur[1], cell)).TrimEnd());

                if (i == names.Count - 1) break;
                sb.AppendLine(Connector(cur.Count, names[i + 1].Count, cell).TrimEnd());
            }

            return sb.ToString();
        }

        private static string Center(string s, int w) =>
            (new string(' ', (w - s.Length + 1) / 2) + s).PadRight(w);

        private static string Connector(int from, int to, int cell)
        {
            var row = Enumerable.Repeat(' ', cell * 2).ToArray();
            var mid = cell;
            var l = cell / 2;
            var r = cell + cell / 2;

            if (from == 1 && to == 1)
            {
                row[mid] = '│';
            }
            else
            {
                var open = from == 1;
                for (var i = l; i <= r; i++) row[i] = '─';
                row[l] = open ? '┌' : '└';
                row[r] = open ? '┐' : '┘';
                row[mid] = open ? '┴' : '┬';
            }

            return new string(row);
        }
    }
}