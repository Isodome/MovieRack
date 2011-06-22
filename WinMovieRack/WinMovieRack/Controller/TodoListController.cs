using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.GUI;

namespace WinMovieRack.Controller {
    public class TodoListController {
        private TodoList todoList;
        private Controller controller;
        private bool firstLoad = true;

        public TodoListController(Controller c) {
            this.controller = c;
        }


        public void setTodoList(TodoList todoList) {
            this.todoList = todoList;
        }

        public void activated() {

            if (firstLoad) {
                firstLoad = false;
            }
        }

    }
}
