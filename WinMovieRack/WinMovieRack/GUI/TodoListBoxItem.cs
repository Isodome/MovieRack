using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.GUI {
    class TodoListBoxItem {
        public int id;
        public string name;
        public string description;

        public TodoListBoxItem(int id, string name, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }

        public string labelOne
        {
            get { return name; }
            set { name = value; }
        }
        public string labelTwo
        {
            get { return description; }
            set { description = value; }
        }

        public int getId
        {
            get { return id; }
        }

    }
}
