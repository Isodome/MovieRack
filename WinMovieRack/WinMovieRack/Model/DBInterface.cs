using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WinMovieRack.Model
{
	interface DBInterface
	{
		public void initDb();
		

		public void checkTables();
		

		public List<Movie> getMovieList();
	}
}
