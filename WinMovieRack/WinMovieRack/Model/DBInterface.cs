using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WinMovieRack.Model
{
	interface DBInterface
	{
		 void initDb();
		

		 void checkTables();
		

		 List<Movie> getMovieList();
	}
}
