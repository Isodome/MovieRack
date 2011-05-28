import java.io.File;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

public class SQLite {
	String inceptionPlot = "Dom Cobb is a skilled thief, the absolute best in the dangerous art of extraction, stealing valuable secrets from deep within the subconscious during the dream state, when the mind is at its most vulnerable. Cobb's rare ability has made him a coveted player in this treacherous new world of corporate espionage, but it has also made him an international fugitive and cost him everything he has ever loved. Now Cobb is being offered a chance at redemption. One last job could give him his life back but only if he can accomplish the impossible-inception. Instead of the perfect heist, Cobb and his team of specialists have to pull off the reverse: their task is not to steal an idea but to plant one. If they succeed, it could be the perfect crime. But no amount of careful planning or expertise can prepare the team for the dangerous enemy that seems to predict their every move. An enemy that only Cobb could have seen coming.";
	String shutterIslandPlot = "It's 1954, and up-and-coming U.S. marshal Teddy Daniels is assigned to investigate the disappearance of a patient from Boston's Shutter Island Ashecliffe Hospital. He's been pushing for an assignment on the island for personal reasons, but before long he wonders whether he hasn't been brought there as part of a twisted plot by hospital doctors whose radical treatments range from unethical to illegal to downright sinister. Teddy's shrewd investigating skills soon provide a promising lead, but the hospital refuses him access to records he suspects would break the case wide open. As a hurricane cuts off communication with the mainland, more dangerous criminals escape in the confusion, and the puzzling, improbable clues multiply, Teddy begins to doubt everything - his memory, his partner, even his own sanity.";
	String shutterIslandPlotnew = "";
	String inceptionPlotnew = "";

	SQLite() throws SQLException {
		try {
			Class.forName("org.sqlite.JDBC");
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		Connection c = DriverManager.getConnection("jdbc:sqlite:test.db");
		Statement statemen = c.createStatement();
		try {
			statemen.executeQuery("SELECT ID FROM Filme");
		} catch (Exception e) {
			create();
		}
		c.close();
	}

	void create() throws SQLException {
		try {
			Class.forName("org.sqlite.JDBC");
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		// Connection c1 = DriverManager.getConnection(
		// "jdbc:hsqldb:file:/opt/db/testdb", "sa", "");
		Connection c = DriverManager.getConnection("jdbc:sqlite:test.db");
		Statement statemen = c.createStatement();

		statemen.executeUpdate("CREATE TABLE Filme ( ID INT NOT NULL , Titel LONGVARCHAR , Inhalt LONGVARCHAR, Aka LONGVARCHAR, Gesehen BOOLEAN);");
		statemen.executeUpdate("CREATE TABLE Schauspieler ( ID INT NOT NULL , Name LONGVARCHAR, Age TINYINT);");
		statemen.executeUpdate("CREATE TABLE Genres (ID INT NOT NULL ,Genre LONGVARCHAR);");
		statemen.executeUpdate("CREATE TABLE FilmeActor (MovieID INT NOT NULL ,SchauspielerID INT NOT NULL);");
		c.close();
	}

	void plot() {
		String[] result = inceptionPlot.split("\'");

		for (int i = 0; i < result.length; i++) {
			inceptionPlotnew = inceptionPlotnew + result[i];
			if (i < result.length - 1) {
				inceptionPlotnew = inceptionPlotnew + "''";
			}
		}

		String[] result1 = shutterIslandPlot.split("\'");

		for (int i = 0; i < result1.length; i++) {
			shutterIslandPlotnew = shutterIslandPlotnew + result1[i];
			if (i < result1.length - 1) {
				shutterIslandPlotnew = shutterIslandPlotnew + "''";
			}
		}
	}

	void add() throws SQLException {
		plot();

		try {
			Class.forName("org.sqlite.JDBC");
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		Connection c = DriverManager.getConnection("jdbc:sqlite:test.db");
		Statement statemen = c.createStatement();

		statemen.execute("INSERT INTO  Filme (ID,Titel,Inhalt,Aka,Gesehen) VALUES ('1375666','Inception','"
				+ inceptionPlotnew
				+ "','Inception: The IMAX Experience','TRUE');");
		statemen.execute("INSERT INTO  Filme VALUES ('1130884','Shutter Island','"
				+ shutterIslandPlotnew + "','La isla siniestra','TRUE');");

		statemen.execute("BEGIN TRANSACTION");
		for (int i = 0; i < 1000; i++) {
			statemen.addBatch("INSERT INTO  Schauspieler (ID,Name,Age) VALUES ('0000138','Leonardo DiCaprio"
					+ i + "','40');");
			statemen.addBatch("INSERT INTO  Schauspieler (ID,Name,Age) VALUES ('0330687','Joseph Gordon-Levitt"
					+ i + "','30');");
			statemen.addBatch("INSERT INTO  Schauspieler (ID,Name,Age) VALUES ('0680983','Ellen Page"
					+ i + "','22');");
			statemen.addBatch("INSERT INTO  Schauspieler (ID,Name,Age) VALUES ('0001426','Ben Kingsley"
					+ i + "','70');");
			statemen.addBatch("INSERT INTO  Schauspieler (ID,Name,Age) VALUES ('0749263','Mark Ruffalo"
					+ i + "','30');");
		}
		statemen.executeBatch();
		statemen.execute("COMMIT");

		statemen.executeUpdate("INSERT INTO  Genres (ID,Genre) VALUES ('1375666','Action');");
		statemen.executeUpdate("INSERT INTO  Genres (ID,Genre) VALUES ('1375666','Mystery');");
		statemen.executeUpdate("INSERT INTO  Genres (ID,Genre) VALUES ('1375666','Sci-Fi');");
		statemen.executeUpdate("INSERT INTO  Genres (ID,Genre) VALUES ('1375666','Thriller');");
		statemen.executeUpdate("INSERT INTO  Genres (ID,Genre) VALUES ('1130884','Drama');");
		statemen.executeUpdate("INSERT INTO  Genres (ID,Genre) VALUES ('1130884','Thriller');");
		statemen.executeUpdate("INSERT INTO  Genres (ID,Genre) VALUES ('1130884','Mystery');");

		statemen.execute("INSERT INTO  FilmeActor (MovieID,SchauspielerID) VALUES ('1375666','0000138');");
		statemen.execute("INSERT INTO  FilmeActor (MovieID,SchauspielerID) VALUES ('1375666','0330687');");
		statemen.execute("INSERT INTO  FilmeActor (MovieID,SchauspielerID) VALUES ('1375666','0680983');");
		statemen.execute("INSERT INTO  FilmeActor (MovieID,SchauspielerID) VALUES ('1130884','0000138');");
		statemen.execute("INSERT INTO  FilmeActor (MovieID,SchauspielerID) VALUES ('1130884','0001426');");
		statemen.execute("INSERT INTO  FilmeActor (MovieID,SchauspielerID) VALUES ('1130884','0749263');");

		c.close();
	}

	void read() throws SQLException {
		try {
			Class.forName("org.sqlite.JDBC");
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		// Connection c3 = DriverManager.getConnection(
		// "jdbc:hsqldb:file:/opt/db/testdb", "sa", "");

		Connection c1 = DriverManager.getConnection("jdbc:sqlite:test.db");
		Statement statemen1 = c1.createStatement();
		ResultSet test = statemen1
				.executeQuery("SELECT ID FROM Genres WHERE Genre = 'Mystery';");

		ArrayList<Integer> array = new ArrayList<Integer>();
		while (test.next()) {
			array.add(test.getInt("ID"));
		}
		Iterator<Integer> iterator1 = array.iterator();
		while (iterator1.hasNext()) {
			ResultSet test3 = statemen1
					.executeQuery("SELECT Titel FROM Filme WHERE ID = '"
							+ iterator1.next() + "';");
			while (test3.next()) {
				System.out.println(test3.getString("Titel"));
			}
		}
		ResultSet test2 = statemen1
				.executeQuery("SELECT SchauspielerID FROM FilmeActor WHERE MovieID = '1375666';");

		ArrayList<Integer> array1 = new ArrayList<Integer>();
		while (test2.next()) {
			array1.add(test2.getInt("SchauspielerID"));
		}

		Iterator<Integer> iterator = array1.iterator();
		while (iterator.hasNext()) {
			ResultSet test3 = statemen1
					.executeQuery("SELECT Name FROM Schauspieler WHERE ID = '"
							+ iterator.next() + "';");
			while (test3.next()) {
				System.out.println(test3.getString("Name"));
			}
		}
		c1.close();
	}

	public void listDir(File dir) {

		File[] files = dir.listFiles();
		if (files != null) {
			for (int i = 0; i < files.length; i++) {
				System.out.print(files[i].getAbsolutePath());
				if (files[i].isDirectory()) {
					System.out.print(" (Ordner)\n");
					listDir(files[i]); // ruft sich selbst mit dem 
						// Unterverzeichnis als Parameter auf
					}
				else {
					System.out.print(" (Datei)\n");
				}
			}
		}
	}
	
	public static void main(String[] args) throws SQLException,
			ClassNotFoundException {
		SQLite test = new SQLite();
		File dir = new File("./");
		test.listDir(dir);
		 test.read();
	}

}
