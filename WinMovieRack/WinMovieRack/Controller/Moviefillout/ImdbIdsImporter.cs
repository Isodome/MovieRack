using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WinMovieRack.Model;
using WinMovieRack.Model.Enums;
using WinMovieRack.GUI;

namespace WinMovieRack.Controller.Moviefillout {
	public class ImdbIdsImporter {

		private string file;
		private const string findIdsRegex = @"(?<id>\d+)";

		public ImdbIdsImporter(string file) {
			this.file = file;
		}

		public void import(object importToTodoListObject) {
            Boolean importToTodoList = (Boolean)importToTodoListObject;
			MatchCollection mc = Regex.Matches(file, findIdsRegex);
            if (!importToTodoList) {
                foreach (Match m in mc) {
                    string idString = m.Groups["id"].Value;
                    uint id = uint.Parse(idString);
                    MovieFillOut f = new MovieFillOut(id);
                    f.startFillout();
                    Thread.Sleep(2000);
                }
            } else {
                foreach (Match m in mc) {
                    string idString = m.Groups["id"].Value;
                    TodoListData data = new TodoListData(TodoType.INSERT_MOVIE_BY_IMDB_ID, idString, "IMDB Movie importieren", "Den Film mit der ID " + idString + " importieren.");
                    SQLiteConnectorTodo.db.insertTodo(data);
                }
                Controller.controller.updateView(View.TODO_LIST);
            }
		}
	}
}
