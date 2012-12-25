using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace MMIRC
{
    public partial class MainForm : Form
    {
        public static List<Connection> Connections;
        public static List<Module> Modules;

        public MainForm()
        {
            InitializeComponent();
            Connections = new List<Connection>();
            Modules = new List<Module>();

            foreach (Type T in GetClasses(typeof(Module)))
            {
                if (T.IsClass)
                {
                    object boxedModule = Activator.CreateInstance(Type.GetType(T.FullName));
                    Module unboxedModule = (Module)boxedModule;
                    Modules.Add(unboxedModule);
                }
            }

            foreach (Module M in Modules)
            {
                Console.WriteLine("Module " + M.Name() + " Version " + M.Version() + " Initializing!");
                M.Init();
            }
            new Thread(() =>
                {
                    for (int i = 1; true; i++)
                    {
                        foreach (Module M in Modules)
                            M.OnTick(i);
                    }
                }).Start();
        }

        public List<Type> GetClasses(Type baseType)
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(type => type.IsSubclassOf(baseType)).ToList();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Connection C = new Connection("irc.freenode.net", 6697, true, "freenode");
            C.Connect();
        }

        public void NickList_Clear()
        {
            this.listView1.Clear();
        }

        public void NickList_Add(string Name)
        {
            ListViewItem Item = new ListViewItem();
            Item.UseItemStyleForSubItems = false;

            Color C = Color.White;
            switch (Name.Remove(1))
            {
                case "+":
                    C = Color.Yellow;
                    break;
                case "%":
                    C = Color.Cyan;
                    break;
                case "@":
                    C = Color.Green;
                    break;
                case "&":
                    C = Color.Red;
                    break;
                case "~":
                    C = Color.Purple;
                    break;
                case "!":
                    C = Color.Pink;
                    break;
                case "*":
                    C = Color.Maroon;
                    break;
            }
            Item.SubItems.Add(Name, C, Color.Black, DefaultFont);
        }

        public void NickList_Remove(string Name)
        {
            foreach (ListViewItem item in this.listView1.Items)
            {
                if (item.Text == Name)
                {
                    this.listView1.Items.Remove(item);
                    break;
                }
            }
        }
    }
}
